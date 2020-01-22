using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Models;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Shop.Models.Domain.Interface;
using Shop.Models.Payment;

namespace Shop.Controllers
{
    public class PaypalExpressController : Controller
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly PaypalExpressConfig _setting;

        public PaypalExpressController(IItemsRepository itemsRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository)
        {
            _itemsRepository = itemsRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpPost("PaypalExpress/CreatePayment")]
        public async Task<ActionResult> CreatePayment()
        {
            var hostingDomain = Request.Host.Value;
            var accessToken = await GetAccessToken();
            var user = await RetrieveUser();
            Order order = _orderRepository.GetBy(user.Bills.Last().BillId);
            //var currentUser = await _workContext.GetCurrentUser();
            //var cart = await _cartService.GetActiveCartDetails(currentUser.Id);
            if (order == null)
            {
                return NotFound();
            }

            var regionInfo = new RegionInfo(new CultureInfo("en-US").LCID);
            var experienceProfileId = await CreateExperienceProfile(accessToken);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var paypalAcceptedNumericFormatCulture = CultureInfo.CreateSpecificCulture("en-US");
            var paymentCreateRequest = new PaymentCreateRequest
            {
                experience_profile_id = experienceProfileId,
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new Transaction[]
                {
                    new Transaction {
                        amount = new Amount
                        {
                            total = (order.BillTotal/23000 + CalculatePaymentFee(order.BillTotal/23000)).ToString("N2", paypalAcceptedNumericFormatCulture),
                            currency = regionInfo.ISOCurrencySymbol,
                            details = new Details
                            {
                                handling_fee = CalculatePaymentFee(order.BillTotal/23000).ToString("N2", paypalAcceptedNumericFormatCulture),
                                subtotal = (order.BillTotal/23000).ToString("N2", paypalAcceptedNumericFormatCulture),
                                tax = "0",
                                shipping =  "0"
                            }
                        }
                    }
                },
                redirect_urls = new Redirect_Urls
                {
                    cancel_url = $"http://{hostingDomain}/PaypalExpress/Cancel", //Haven't seen it being used anywhere
                    return_url = $"http://{hostingDomain}/PaypalExpress/Success", //Haven't seen it being used anywhere
                }
            };

            var response = await httpClient.PostAsJsonAsync($"https://api{PaypalExpressConfig.EnvironmentUrlPart}.paypal.com/v1/payments/payment", paymentCreateRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic payment = JObject.Parse(responseBody);
            if (response.IsSuccessStatusCode)
            {
                string paymentId = payment.id;
                return Ok(new { PaymentId = paymentId });
            }

            return BadRequest(responseBody);
        }

        [HttpPost("PaypalExpress/ExecutePayment")]
        public async Task<ActionResult> ExecutePayment(PaymentExecuteVm model)
        {
            var accessToken = await GetAccessToken();
            var user = await RetrieveUser();
            //var currentUser = await _workContext.GetCurrentUser();
            Order order = _orderRepository.GetBy(user.Bills.Last().BillId);
            if (order == null)
            {
                return NotFound();
            }

            //var order = orderCreateResult.Value;
            var payment = new Payment()
            {
                OrderId = order.BillId,
                PaymentFee = 1m,
                Amount = order.BillTotal/23000,
                PaymentMethod = "Paypal Express",
                CreatedOn = DateTimeOffset.UtcNow,
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var paymentExecuteRequest = new PaymentExecuteRequest
            {
                payer_id = model.payerID
            };

            var response = await httpClient.PostAsJsonAsync($"https://api{PaypalExpressConfig.EnvironmentUrlPart}.paypal.com/v1/payments/payment/{model.paymentID}/execute", paymentExecuteRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JObject.Parse(responseBody);
            if (response.IsSuccessStatusCode)
            {
                // Has to explicitly declare the type to be able to get the propery
                string payPalPaymentId = responseObject.id;
                ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);
                OrderItem.ToList().ForEach(bl => bl.Validity = Validity.Valid);
                _userRepository.SaveChanges();
                //return RedirectToAction(nameof(CheckoutController.Thanks), "Checkout", new{ Id=order.BillId});
                return Ok(new { Status = "success", OrderId = order.BillId });
            }

            //payment.Status = PaymentStatus.Failed;
            //payment.FailureMessage = responseBody;
            //_paymentRepository.Add(payment);
            //order.OrderStatus = OrderStatus.PaymentFailed;
            //await _paymentRepository.SaveChangesAsync();

            string errorName = responseObject.name;
            string errorDescription = responseObject.message;
            return BadRequest($"{errorName} - {errorDescription}");
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

        private async Task<string> GetAccessToken()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{PaypalExpressConfig.ClientId}:{PaypalExpressConfig.ClientSecret}")));
            var requestBody = new StringContent("grant_type=client_credentials");
            requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await httpClient.PostAsync($"https://api{PaypalExpressConfig.EnvironmentUrlPart}.paypal.com/v1/oauth2/token", requestBody);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic token = JObject.Parse(responseBody);
            string accessToken = token.access_token;
            return accessToken;
        }

        private async Task<string> CreateExperienceProfile(string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var experienceRequest = new ExperienceProfile
            {
                name = $"simpl_{Guid.NewGuid()}",
                input_fields = new InputFields
                {
                    no_shipping = 1
                },
                temporary = true
            };
            var response = await httpClient.PostAsJsonAsync($"https://api{PaypalExpressConfig.EnvironmentUrlPart}.paypal.com/v1/payment-experience/web-profiles", experienceRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic experience = JObject.Parse(responseBody);
            // Has to explicitly declare the type to be able to get the propery
            string profileId = experience.id;
            return profileId;
        }

        private decimal CalculatePaymentFee(decimal total)
        {
            var percent = PaypalExpressConfig.PaymentFee;
            return (total / 100) * percent;
        }
    }
}