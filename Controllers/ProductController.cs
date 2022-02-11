#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lebonanimal.Datas;
using lebonanimal.Models;

namespace lebonanimal.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbConnect _context;
        private readonly IWebHostEnvironment _he;

        public ProductController(DbConnect context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _he = hostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Bind("Id,Title,Price,ImgPath,Description,Certificat,Enabled,Deleted")] avant Product product
        public async Task<IActionResult> Create([Bind("Id,Title,Price,ImgPath,Description,Certificat,Enabled,Deleted")] Product product, List<IFormFile> ImgPath, List<IFormFile> Certificat)
        {
  

            // pour les photos 
            
            string wwwPath = this._he.WebRootPath;
            string contentPath = this._he.ContentRootPath;

            string pathPhotos = Path.Combine(this._he.WebRootPath, "files/photos");
            if (!Directory.Exists(pathPhotos))
            {
                Directory.CreateDirectory(pathPhotos);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in ImgPath)
            {
                //string fileName = Path.GetFileName(product.ImgPath);
                string fileName = postedFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(pathPhotos, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message1 += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    product.ImgPath = fileName;
                }
            }

            // pour les certificats
            string pathCertificates = Path.Combine(this._he.WebRootPath, "files/certificates");
            if (!Directory.Exists(pathCertificates))
            {
                Directory.CreateDirectory(pathCertificates);
            }

            List<string> uploadedCerts = new List<string>();
            foreach (IFormFile postedFile in Certificat)
            {
                // string fileName = Path.GetFileName(product.Certificat);
                string fileName = postedFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(pathCertificates, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedCerts.Add(fileName);
                    ViewBag.Message2 += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    product.Certificat = fileName;
                }
            }

            product.User = _context.Users.Find(2);
            product.Category = _context.Categories.Find(int.Parse(Request.Form["Category.Id"]));

            TempData["Message"] = "File successfully uploaded to File System.";

           // if (ModelState.IsValid)
            // {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            // }
            //return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,ImgPath,Description,Certificat,Enabled,Deleted")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
