using Eticaret.Core.Entities;
using Eticaret.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Controllers
{
    [Authorize]
    public class MyAddressesController : Controller
    {

        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Adress> _serviceAdress;

        public MyAddressesController(IService<AppUser> service, IService<Adress> serviceAdress)
        {
            _serviceAppUser = service;
            _serviceAdress = serviceAdress;
        }
        public async Task<IActionResult> Index()
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı");
            }
            var model = await _serviceAdress.GetAllAsync(u => u.AppUserId == appUser.Id);
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Adress adress)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    if (appUser != null)
                    {
                        adress.AppUserId = appUser.Id;
                        _serviceAdress.Add(adress);
                        await _serviceAdress.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
            ModelState.AddModelError("", "Kayıt Başarısız");
            return View(adress);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı");
            }
            var model = await _serviceAdress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adress Bilgisi Bulunamadı");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Adress adress)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı");
            }
            var model = await _serviceAdress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adress Bilgisi Bulunamadı");
            }

            model.Title = adress.Title;
            model.District = adress.District;
            model.City = adress.City;
            model.OpenAddress = adress.OpenAddress;
            model.IsActive = adress.IsActive;
            model.IsBillingAddress = adress.IsBillingAddress;
            model.IsDeliveryAddress = adress.IsDeliveryAddress;
            var otherAddresses = await _serviceAdress.GetAllAsync(x => x.AppUserId == appUser.Id && x.Id != model.Id);
            foreach (var item in otherAddresses)//diğer adresleri getirmesin 
            {
                item.IsDeliveryAddress = false;
                item.IsBillingAddress = false;
            _serviceAdress.Update(item);
            }
            try
            {
                _serviceAdress.Update(model);
                await _serviceAdress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Hata Oluştu");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı");
            }
            var model = await _serviceAdress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adress Bilgisi Bulunamadı");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, Adress adress)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.Guid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı");
            }
            var model = await _serviceAdress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adress Bilgisi Bulunamadı");
            }
            try
            {
                _serviceAdress.Delete(model);
                await _serviceAdress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu");
            }
            return View(model);
        }
    }
}