using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Shop.Models.HomeViewModels;
using System.Net.Mail;
using Shop.Models.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Shop.Models.Domain.Enum;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(IItemsRepository itemsRepository, ICategoryRepository categoryRepository, ISellerRepository sellerRepository, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager)
        {
            _itemsRepository = itemsRepository;
            _categoryRepository = categoryRepository;
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            return View(new IndexViewModel(_itemsRepository.GetTop30(_itemsRepository.GetAllApproved().ToList()).ToList(), _itemsRepository.GetCouponOfferSlider(_itemsRepository.GetAllApproved().ToList()).ToList(), _categoryRepository.GetTop9WithAmount()));
        }

        public IActionResult Offers()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(_itemsRepository.GetItemsOfferStandardAndSlider(_itemsRepository.GetAllApproved().ToList()).Select(b => new SearchViewModel(b)).ToList());
        }

        public IActionResult About()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View();
        }

        public IActionResult Error()
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string SearchType = null, string SearchKey = null, string Category = null, string Location = null, string MaxStartPrice = null )
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            IEnumerable<Items> filteredItems;


            if (!string.IsNullOrEmpty(SearchKey))

            {
                switch (SearchType)
                {
                    case "All":
                        filteredItems = _itemsRepository.GetAll(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                    case "Company":
                        filteredItems = _itemsRepository.GetByLocation(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                    case "FirstName":
                        filteredItems = _itemsRepository.GetByName(SearchKey, _itemsRepository.GetAllApproved());
                        break;
                        
                    default:
                        filteredItems = _itemsRepository.GetAllApproved();
                        break;
                }
                ViewData["SearchAssignment"] = SearchKey + " trong " + SearchType;

                if (!string.IsNullOrEmpty(Category) && Category != "*")
                {
                    string input = Category;
                    filteredItems = _itemsRepository.GetByCategory(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredItems = _itemsRepository.GetByLocation(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredItems = _itemsRepository.GetByPrice(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa " + input;
                }
            }
            else
            {
                filteredItems = _itemsRepository.GetAllApproved();
                ViewData["SearchAssignment"] = "Tổng quan";

                if (!string.IsNullOrEmpty(Category) && Category != "*")
                {
                    string input = Category;
                    filteredItems = _itemsRepository.GetByCategory(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với thể loại " + input;
                }
                if (!string.IsNullOrEmpty(Location) && Location != "*")
                {
                    string input = Location;
                    filteredItems = _itemsRepository.GetByLocation(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với địa điểm " + input;
                }
                if (!string.IsNullOrEmpty(MaxStartPrice))
                {
                    int input = int.Parse(MaxStartPrice);
                    filteredItems = _itemsRepository.GetByPrice(input, filteredItems);
                    ViewData["SearchAssignment"] = ViewData["SearchAssignment"] + ", với giá tối đa " + input;
                }
            }

            return View(filteredItems.Select(b => new SearchViewModel(b)).ToList());
        }

        [Route("/Detail/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            //Items clickedItems = _itemsRepository.GetByItemsId(Id);
            Items clickedItems = _itemsRepository.GetByItemsSlug(slug);
            
            if (clickedItems == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["GetTop30"] = _itemsRepository.GetByCategory(clickedItems.Category.Name,_itemsRepository.GetAllApproved().ToList()).ToList();

            ViewData["Comment"] = _itemsRepository.GetCommentsBySlug(slug);

            ViewData["Vote"] = CreateVoteModel(_itemsRepository.GetVotesBySlug(slug));

            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            //Da mua san pham nay hay chua
            ViewData["CanVote"] = await CheckIfThisUserHasBuyThisItem(clickedItems);

            //Da vote san pham nay hay chua
            ViewData["Voted"] = await CheckIfThisUserHasVoteThisItem(clickedItems, slug);

            return View(new DetailViewModel(clickedItems));
        }

      

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(string commentContent, string slug, int itemsId)
        {
            if (!string.IsNullOrEmpty(commentContent) && !string.IsNullOrEmpty(slug))
            {
                var user = await _userManager.GetUserAsync(User);
                var u = _userRepository.GetBy(user.Email);
                var comment = new Comment
                {
                    CommentContent = commentContent,
                    User = u,
                    Items = _itemsRepository.GetByItemsId(itemsId)
                };

                _itemsRepository.AddComment(comment);
                _itemsRepository.SaveChanges();
            }

            return RedirectToAction("Detail", new { slug = slug });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVote(DetailViewModel model, int itemsID, string slug)
        {
            if (itemsID>0 && slug.Length>0)
            {
                var user = await _userManager.GetUserAsync(User);
                var u = _userRepository.GetBy(user.Email);
                var vote = new Vote
                {
                    Value = model.VoteValue,
                    User = u,
                    Items = _itemsRepository.GetByItemsId(itemsID)
                };

                _itemsRepository.AddVote(vote);
                _itemsRepository.SaveChanges();
            }

            return RedirectToAction("Detail", new { slug = slug });
        }

        [HttpPost]
        public IActionResult SendEmail(SendEmailViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                message.From = new MailAddress(".shop.suport@gmail.com");
                message.To.Add("3bros.shop.suport@gmail.com");
                message.Subject = "tin nhắn từ liên hệ";
                message.Body = String.Format("Tên: {0}\n" +
                                        "Địa chỉ Email: {1}\n" +
                                        "Tiêu đề: {2}\n" +
                                        "Tin nhắn: {3}\n",
                                        model.Name, model.Email, model.Subject, model.Message);
                var SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(message);

                return RedirectToAction("Index");
            }
            return View(nameof(About), model);
        }

        [HttpGet]
        public IActionResult UpdateShoppingCartCount()
        {
            return PartialView("shoppingCartCountPartial");
        }

        private VoteViewModel CreateVoteModel(IEnumerable<Vote> votes)
        {
            var ratingCount = votes.Count();
            return new VoteViewModel
            {
                 RatingCount = votes.Count(),
                Rating1Count = (double)votes.Count(v => v.Value == 1)/ratingCount*100,
                Rating2Count = (double)votes.Count(v => v.Value == 2)/ratingCount*100,
                Rating3Count = (double)votes.Count(v => v.Value == 3)/ratingCount*100,
                Rating4Count = (double)votes.Count(v => v.Value == 4)/ratingCount*100,
                Rating5Count = (double)votes.Count(v => v.Value == 5)/ratingCount*100,
            };
        }

        private async Task<bool> CheckIfThisUserHasBuyThisItem(Items item)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                var _user = _userRepository.GetBy(user.Email);
                if (_user.Bills.Count != 0 && _user.Bills != null)
                {
                    foreach (Order b in _user.Bills)
                    {
                        var Order = _orderRepository.GetBy(b.BillId);
                        //if (Order.Orders.All(bl => bl.Validity != Validity.Invalid))
                        //    orders.Add(Order);
                        foreach (var mobile in Order.Orders)
                        {
                            if (mobile.Items?.ItemsId == item.ItemsId)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        private async Task<bool> CheckIfThisUserHasVoteThisItem(Items item, string slug)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var _user = _userRepository.GetBy(user.Email);
                var votes = _itemsRepository.GetVotesBySlug(slug);
                if (votes.Count()>0)
                {

                    foreach (var vote in votes)
                    {
                        if (vote.User.UserId == _user.UserId)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
