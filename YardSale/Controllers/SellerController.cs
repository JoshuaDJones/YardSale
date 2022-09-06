using Core.YardSale.Contracts;
using Core.YardSale.Orders;
using Core.YardSale.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using YardSale.ViewModels;

namespace YardSale.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private readonly HttpClient _httpClient;
        public SellerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index(int productId = 0)
        {
            SellerProductsViewModal vm = new();
            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            var response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/GetSellersProducts?userId=" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            vm.products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

            if (productId > 0)
            {
                response = await _httpClient.GetAsync("https://yardsaleapi.azurewebsites.net/api/Product/GetProduct?productId=" + productId);
                apiResponse = await response.Content.ReadAsStringAsync();
                vm.productEdit = JsonConvert.DeserializeObject<Product>(apiResponse);
                vm.IsEdit = true;
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(SellerProductsViewModal productVM)
        {

            ProductThumbnailData thumbnailData = new();

            if (productVM.productCreateViewModel.Photo != null)
            {

                var formData = new MultipartFormDataContent();
                HttpContent fileStreamContent = new StreamContent(productVM.productCreateViewModel.Photo.OpenReadStream());
                fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "ThumbNailPhoto", FileName = productVM.productCreateViewModel.Photo.FileName };
                var contentType = productVM.productCreateViewModel.Photo.FileName.ToLower().Contains("png") ? "image/png" : "image/jpeg";
                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                formData.Add(fileStreamContent);
                
                var aMessage = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://yardsaleapi.azurewebsites.net/api/Product/CreateThumbnailPhoto"),
                    Content = formData,
                    Method = HttpMethod.Post
                };
                
                var response = await _httpClient.SendAsync(aMessage);
                var apiResponse = await response.Content.ReadAsStringAsync();
                thumbnailData = JsonConvert.DeserializeObject<ProductThumbnailData>(apiResponse);

                productVM.productCreateViewModel.Product.ProductThumbnailPhotoFilename = thumbnailData.ThumbnailPhotoFilename;
                productVM.productCreateViewModel.Product.ProductThumbnailPhotoUrl = thumbnailData.ThumbnailPhotoUrl;
            }

            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            productVM.productCreateViewModel.Product.UserId = id;
            productVM.productCreateViewModel.Product.ProductIsActive = true;
            productVM.productCreateViewModel.Product.ProductIsSold = false;

            StringContent content = new StringContent(JsonConvert.SerializeObject(productVM.productCreateViewModel.Product), Encoding.UTF8, "application/json");
            var apiReponse = await _httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/Product/CreateProduct", content);

            return RedirectToAction("Index");
        }

        public IActionResult SetEditProduct(int productId)
        {
            return RedirectToAction("Index", productId);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(SellerProductsViewModal productVM)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(productVM.productEdit), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://yardsaleapi.azurewebsites.net/api/Product/UpdateProduct", content);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var response = await _httpClient.DeleteAsync("https://yardsaleapi.azurewebsites.net/api/Product/DeleteProduct?productId=" + productId);
            string apiResponse = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrdersList(int LkOrderStatusId = 0)
        {
            OrdersViewModel vm = new();

            vm.LkOrderStatusId = LkOrderStatusId;

            int id = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);

            OrderData od = new()
            {
                UserId = id,
                StatusId = LkOrderStatusId
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(od), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/Order/GetUserOrders", content);
            string apiResponse = await response.Content.ReadAsStringAsync();
            vm.Orders = JsonConvert.DeserializeObject<List<Order>>(apiResponse);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] MarkOrderData data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/Order/ChangeOrderStatus", content);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderStatusId, int orderId, int searchStatusId)
        {
            MarkOrderData data = new()
            {
                OrderId = orderId,
                StatusId = orderStatusId
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://yardsaleapi.azurewebsites.net/api/Order/ChangeOrderStatus", content);
            return RedirectToAction("OrdersList", searchStatusId);
        }
    }
}
