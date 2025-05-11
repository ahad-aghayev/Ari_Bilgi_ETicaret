using Eticaret.Core.Entities;
using Eticaret.Service.Abstract;
using Eticaret.Service.Concrete;
using Eticaret.WebUI.ExtensionMethods;
using Eticaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Eticaret.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Product> _service;
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Adress> _serviceAdress;
        private readonly IService<Order> _serviceOrder;

        public CartController(IService<Product> service, IService<Adress> serviceAdress, IService<AppUser> serviceAppUser, IService<Order> serviceOrder)
        {
            _service = service;
            _serviceAdress = serviceAdress;
            _serviceAppUser = serviceAppUser;
            _serviceOrder = serviceOrder;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel()
            {
                CartLines = cart.CartLines,
                TotalPrice = cart.TotalPrice()
            };
            return View(model);
        }
        public IActionResult Add(int ProductId, int quantity = 1)
        {
            var product = _service.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int ProductId, int quantity = 1)
        {
            var product = _service.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int ProductId)
        {
            var product = _service.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            var addresses = await _serviceAdress.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Adresses = addresses
            };
            return View(model);
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> Checkout(string CardNumber, string CardMonth, string CardYear, string CVV, string DeliveryAddress, string BillingAddress)
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            var addresses = await _serviceAdress.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Adresses = addresses
            };
            if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(CardMonth) || string.IsNullOrWhiteSpace(CardYear) || string.IsNullOrWhiteSpace(CVV) || string.IsNullOrWhiteSpace(DeliveryAddress) || string.IsNullOrWhiteSpace(BillingAddress))
            {
                return View(model);
            }
            var faturaAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == BillingAddress);
            var teslimatAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == DeliveryAddress);


            //Ödeme Çekme işlemi
            var siparis = new Order
            {
                AppUserId = appUser.Id,
                //BillingAddress
                BillingAddress = $"{faturaAdresi.OpenAddress} {faturaAdresi.District} {faturaAdresi.City}",
                //DeliveryAddress
                DeliveryAddress = $"{faturaAdresi.OpenAddress} {faturaAdresi.District} {faturaAdresi.City}",
                CustomerId = appUser.Guid.ToString(),
                OrderDate = DateTime.Now,
                TotalPrice = cart.TotalPrice(),
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState=0,
                OrderLines = []
            };
            foreach (var item in cart.CartLines)
            {

                siparis.OrderLines.Add(new OrderLine
                {
                    ProductId = item.Product.Id,
                    OrderId = siparis.Id,
                    Quantity = item.Quantity,
                    UnitePrice = item.Product.Price
                });
            }
            try
            {
                await _serviceOrder.AddAsync(siparis);
                var sonuc = await _serviceOrder.SaveChangesAsync();
                if (sonuc > 0)
                {
                    HttpContext.Session.Remove("Cart");
                    return RedirectToAction("Thanks"); //Sipariş sonrası Thansk sayfasına git
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Hata Oluştu";
            }
            return View(model);
        }

        public IActionResult Thanks()
        {
            return View();
        }
        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }
    }
}
