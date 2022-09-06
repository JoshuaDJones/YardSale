using Core.YardSale.Contracts;
using Core.YardSale.Orders;
using Core.YardSale.Products;
using Core.YardSale.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using YardSale.Models;
using YardSale.ViewModels;

namespace YardSale.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;   
        }

        public IActionResult Index()
        {
            return View();
        }         
        
        public async Task<IActionResult> Browse(int categoryId = 0)
        {
            BrowseViewModel vm = new();
            List<Product> products = new();

            var response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/GetProducts?categoryId=" + categoryId);
            string apiResponse = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

            vm.Products = products;
            vm.CategoryId = categoryId;
            return View(vm);
        }

        public async Task <IActionResult> Cart(string productKeys, bool isPurchaseSuccess = false)
        {
            CartViewModel vm = new();

            if(productKeys != null)
            {
                string[] splittedKeys = productKeys.Split(new[] { "," }, StringSplitOptions.None);

                for (int i = 0; i < splittedKeys.Length; i++)
                {
                    var response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/GetProduct?productId=" + splittedKeys[i]);
                    Product product = new();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);

                    vm.Total = vm.Total + product.ProductCost;
                    vm.CartProducts.Add(product);
                }
            }

            vm.OrderSuccess = isPurchaseSuccess;

            return View(vm);
        }

        [HttpPost]
        public async Task <ActionResult> CompleteTransaction([FromBody] PurchaseInfo data)
        {
            string[] splittedKeys = data.productKeys.Split(new[] { "," }, StringSplitOptions.None);

            for (int i = 0; i < splittedKeys.Length; i++)
            {
                Product product = new();

                var response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/GetProduct?productId=" + splittedKeys[i]);
                string apiResponse = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<Product>(apiResponse);

                response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/MarkProductSold?productId=" + splittedKeys[i]);

                LkOrderStatus status = new()
                {
                    LkOrderStatusId = 1
                };

                Order order = new()
                {
                    Product = product,
                    LkOrderStatus = status,
                    OrderDateTime = DateTime.Now,
                    OrderCustomerAddress = data.customerAddress,
                    OrderTotal = product.ProductCost
                };

                StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/Order/CreateOrder", content);
            }

            Response.Cookies.Delete("_cartKeys");
            return View("Index");
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }

        public IActionResult Register()
        {
            User user = new();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var httpClient = new HttpClient();

            if (ModelState.IsValid)
            {
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);

                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/User/RegisterNewUser", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Validate(Credentials creds)
        {
            if(ModelState.IsValid)
            {
                User user = new();

                var response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/User/GetUserByUserName?username=" + creds.Username);
                string apiResponse = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(apiResponse);

                if (user != null)
                {
                    bool result = BCrypt.Net.BCrypt.Verify(creds.Password, user.UserPassword);
                    if (result)
                    {

                        var claims = new List<Claim>();
                        //claims are properties that describe the user 
                        claims.Add(new Claim("UserId", user.UserId.ToString()));
                        claims.Add(new Claim("UserFirstName", user.UserFirstName));
                        claims.Add(new Claim("UserLastName", user.UserLastName));
                        claims.Add(new Claim("UserEmail", user.UserEmail));
                        claims.Add(new Claim("UserUserName", user.UserUserName));
                        //Identity of the user with the claims attached to them and the scheme that its bound too
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        //Authentication Ticket is a claims principal
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        //
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction( actionName: "Index", controllerName: "Seller");
                    }
                    else
                    {
                        ViewBag.InvalidCredentialsMsg = "The credentials you have entered are not recognized.";
                        return View("login");
                    }
                }
                else
                {
                    ViewBag.InvalidCredentialsMsg = "The credentials you have entered are not recognized.";
                    return View("login");
                }
            }
            else
            {
                ViewBag.InvalidCredentialsMsg = "The credentials you have entered are not recognized.";
                return View("login");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}