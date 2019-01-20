using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using Microsoft.EntityFrameworkCore;
using QualityCap.Data;
using QualityCap.Models;

namespace QualityCap.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Member")]
    public class MemberCapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostEnv;

        public MemberCapsController(ApplicationDbContext context, IHostingEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: MemberCaps     
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? ID, int? page)
        {
            ViewData["Category"] = _context.Categories;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var caps = from c in _context.Caps.Include(m => m.Supplier).Include(m => m.Category)
                       select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                caps = caps.Where(s => (s.Name.Contains(searchString)));
            }
            if (ID != null)
            {
                caps = caps.Where(s => s.CategoryID == ID);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    caps = caps.OrderByDescending(c => c.Name);
                    break;

                default:
                    caps = caps.OrderBy(c => c.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await PaginatedList<Cap>.CreateAsync(caps.AsNoTracking(), pageNumber, pageSize));
        }

        // GET: MemberCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps
                .Include(c => c.Category)
                .Include(c => c.Supplier)
                .SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }

            return View(cap);
        }

        // GET: MemberCaps/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: MemberCaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CapID,Name,Description,CategoryID,Image,Price,SupplierID")] Cap cap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", cap.SupplierID);
            return View(cap);
        }

        // GET: MemberCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps.SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", cap.SupplierID);
            return View(cap);
        }

        // POST: MemberCaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CapID,Name,Description,CategoryID,Image,Price,SupplierID")] Cap cap)
        {
            if (id != cap.CapID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapExists(cap.CapID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", cap.SupplierID);
            return View(cap);
        }

        // GET: MemberCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps
                .Include(c => c.Category)
                .Include(c => c.Supplier)
                .SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }

            return View(cap);
        }

        // POST: MemberCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cap = await _context.Caps.SingleOrDefaultAsync(m => m.CapID == id);
            _context.Caps.Remove(cap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapExists(int id)
        {
            return _context.Caps.Any(e => e.CapID == id);
        }
    }
}
