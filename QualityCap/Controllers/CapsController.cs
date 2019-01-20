using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using QualityCap.Data;
using QualityCap.Models;

namespace QualityCap.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostEnv;

        public CapsController(ApplicationDbContext context, IHostingEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Caps
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
            var caps = from c in _context.Caps.Include(m=>m.Supplier).Include(m=>m.Category)
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
            return View(await PaginatedList<Cap>.CreateAsync(caps.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Caps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps.Include (m=>m.Supplier).Include(m =>m.Category)              
                .SingleOrDefaultAsync(m => m.CapID == id);
            if (cap == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");
            return View(cap);
        }

        // GET: Caps/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: Caps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CapID,Name,Description,CategoryID,Image,Price,SupplierID")] Cap cap, IList<IFormFile> _files)
        {
            var path = "";
            var fName = "";

            if (_files.Count < 1)
            {
                path = "/images/none.jpg";
            }
            else
            {
                foreach (var i in _files)
                {
                    fName = ContentDispositionHeaderValue
                                      .Parse(i.ContentDisposition)
                                      .FileName
                                      .Trim('"');
                    //Path for localhost
                    path = "/images/cap/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fName;

                    using (FileStream fs = System.IO.File.Create(_hostEnv.WebRootPath + path))
                    {
                        await i.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
            }
            cap.Image = path;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }               
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }                     
            return View(cap);
        }

        // GET: Caps/Edit/5
        [Authorize(Roles = "Admin")]
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

        // POST: Caps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id, [Bind("CapID,Name,Description,CategoryID,Image,Price,SupplierID")] Cap cap, IList<IFormFile> _files)
        {
            if (id == null)
            {
                return NotFound();
            }
            var path = "";
            var fName = "";

            if (_files.Count < 1)
            {
                path = "/images/none.jpg";
            }
            else
            {
                foreach (var i in _files)
                {
                    fName = ContentDispositionHeaderValue
                                      .Parse(i.ContentDisposition)
                                      .FileName
                                      .Trim('"');
                    //Path for localhost
                    path = "/images/cap/" + fName;

                    using (FileStream fs = System.IO.File.Create(_hostEnv.WebRootPath + path))
                    {
                        await i.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
            }
            cap.Image = path;

            //var capToUpdate = await _context.Caps.SingleOrDefaultAsync(c => c.CapID == id);
            //if (await TryUpdateModelAsync<Cap>(
            //    capToUpdate,
            //    "",
            //    c => c.Name, c => c.Description, c => c.Price, c => c.Image, c => c.SupplierID, c => c.CategoryID))
            //{
            //    try
            //    {              
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(cap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(cap);
        }

       // GET: Caps/Delete/5
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Caps.Include(m=>m.Category).Include(m=>m.Supplier)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CapID == id);

            if (cap == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again, and if the problem persists " +
                "see your system administrator.";
            }
           
            return View(cap);
        }

        // POST: Caps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cap = await _context.Caps.AsNoTracking().SingleOrDefaultAsync(m => m.CapID == id);
        _context.Caps.Remove(cap);     

            try
            {                
                await _context.SaveChangesAsync();               
            }
            catch (DbUpdateException)
            {
            TempData["CapUsed"] = "The cap being deleted has been used in previous orders.Delete those orders before trying again.";
            return RedirectToAction("Delete");
            }
        return RedirectToAction("Index");
    }

        private bool CapExists(int id)
        {
            return _context.Caps.Any(e => e.CapID == id);
        }
    }
}
