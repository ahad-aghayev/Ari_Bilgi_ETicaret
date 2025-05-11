using Eticaret.Core.Entities;
using Eticaret.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")] 
    public class AdressesController : Controller
    {
        private readonly DatabaseContext _context;
        public AdressesController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Addresses.Include(a => a.AppUser);
            return View(await databaseContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }

        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Adress adress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Email", adress.AppUserId);
            return View(adress);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Addresses.FindAsync(id);
            if (adress == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Email", adress.AppUserId);
            return View(adress);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Adress adress)
        {
            if (id != adress.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdressExists(adress.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Email", adress.AppUserId);
            return View(adress);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adress = await _context.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adress == null)
            {
                return NotFound();
            }

            return View(adress);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adress = await _context.Addresses.FindAsync(id);
            if (adress != null)
            {
                _context.Addresses.Remove(adress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AdressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
