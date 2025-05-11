using Eticaret.Core.Entities;
using Eticaret.Data;
using Eticaret.Service.Abstract;
using Eticaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IService<Product> _service;

        public ProductsController(IService<Product> service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(string search = "") // string(search)-> sitedeki search butonunda ürün arama için, _Header alanında input name="search" olarak yakalıyor     
        {
            var databaseContext = _service.GetAllAsync(x => x.IsActive && x.Name.Contains(search) || x.Description.Contains(search)) ;
            return View(await databaseContext );
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _service.GetQueryable()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var model = new ProductDetailViewModel()
            {
                Product = product,
                RelatedProducts = _service.GetQueryable().Where(x => x.IsActive && x.CategoryId == product.CategoryId && x.Id != product.Id)
            };
            return View(model);
        }
    }
}
