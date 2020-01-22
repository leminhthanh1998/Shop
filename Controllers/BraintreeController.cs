using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Shop.Models.Domain.Interface;
using Shop.Models.Payment;

namespace Shop.Controllers
{
    public class BraintreeController : Controller
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public BraintreeController(IItemsRepository itemsRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository)
        {
            _itemsRepository = itemsRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Charge(string nonce)
        {
            var gateway = new BraintreeGateway("sandbox", "ncsh7wwqvzs3cx9q", "6j4d7qspt5n48kx4", "bd1c26e53a6d811243fcc3eb268113e1");

            var user = await RetrieveUser();
            Order order = _orderRepository.GetBy(user.Bills.Last().BillId);
            //var currentUser = await _workContext.GetCurrentUser();
            //var cart = await _cartService.GetActiveCartDetails(currentUser.Id);
            if (order == null)
            {
                return NotFound();
            }



            var regionInfo = new RegionInfo(new CultureInfo("en-US").LCID);
            var payment = new Payment()
            {
                OrderId = order.BillId,
                Amount = order.BillTotal,
                PaymentMethod = "Braintree",
                CreatedOn = DateTimeOffset.UtcNow
            };

            var lineItemsRequest = new List<TransactionLineItemRequest>();

            //TODO: Need validation
            //foreach(var item in order.OrderItems)
            //{
            //    lineItemsRequest.Add(new TransactionLineItemRequest
            //    {
            //        Description = item.Product.Description.Substring(0, 255),
            //        Name = item.Product.Name,
            //        Quantity = item.Quantity,
            //        UnitAmount = item.ProductPrice,
            //        ProductCode = item.ProductId.ToString(),
            //        TotalAmount = item.ProductPrice * item.Quantity

            //    });
            //}

            //TODO: See how customer id works
            var request = new TransactionRequest
            {
                Amount = order.BillTotal/23000,
                PaymentMethodNonce = nonce,
                OrderId = order.BillId.ToString(),
                //LineItems = lineItemsRequest.ToArray(),
                //CustomerId = order.CustomerId.ToString(),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    SkipAdvancedFraudChecking = false,
                    SkipCvv = false,
                    SkipAvs = false,
                }
            };

            var result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                var transaction = result.Target;

                payment.GatewayTransactionId = transaction.Id;
                payment.Status = PaymentStatus.Succeeded;
                ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);
                OrderItem.ToList().ForEach(bl => bl.Validity = Validity.Valid);
                _userRepository.SaveChanges();
                //return RedirectToAction(nameof(CheckoutController.Thanks), "Checkout", new{ Id=order.BillId});
                return Ok(new { Status = "success", OrderId = order.BillId });
            }
            else
            {
                string errorMessages = "";
                foreach (var error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }

                return BadRequest(errorMessages);
            }
        }


        private async Task<User> RetrieveUser()
        {
            var user = _userRepository.GetBy("nguoidung@gmail.com");
            if (_signInManager.IsSignedIn(User))
            {
                var _user = await _userManager.GetUserAsync(User);
                if (_userRepository.GetBy(_user.Email) != null)
                    user = _userRepository.GetBy(_user.Email);
            }
            return user;
        }

        private ICollection<OrderItem> RetrieveOrderItem(Order order)
        {
            ICollection<OrderItem> OrderItem = new HashSet<OrderItem>();

            foreach (OrderItem or in order.Orders)
            {
                OrderItem.Add(_orderItemRepository.GetById(or.OrderItemId));
            }

            return OrderItem;
        }

    }
}