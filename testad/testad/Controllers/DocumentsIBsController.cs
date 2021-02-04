using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testad.Data;
using testad.Models;

namespace testad.Controllers
{
    [Authorize(Policy = "adminmsa")]

    public class DocumentsIBsController : Controller
    {
        private readonly MvcRequestContext _context;

        IHostingEnvironment _env;


        public DocumentsIBsController(MvcRequestContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
        }

        // GET: DocumentsIBs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DocumentsIB.ToListAsync());
        }



        // GET: DocumentsIBs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentsIB = await _context.DocumentsIB
                .FirstOrDefaultAsync(m => m.id == id);
            if (documentsIB == null)
            {
                return NotFound();
            }

            return View(documentsIB);
        }

        // GET: DocumentsIBs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocumentsIBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,text")] DocumentsIB documentsIB)
        {
            if (ModelState.IsValid)
            {
                _context.Add(documentsIB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(documentsIB);
        }

        // GET: DocumentsIBs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentsIB = await _context.DocumentsIB.FindAsync(id);
            if (documentsIB == null)
            {
                return NotFound();
            }
            return View(documentsIB);
        }

        // POST: DocumentsIBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,text")] DocumentsIB documentsIB)
        {
            if (id != documentsIB.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documentsIB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentsIBExists(documentsIB.id))
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
            return View(documentsIB);
        }

        // GET: DocumentsIBs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentsIB = await _context.DocumentsIB
                .FirstOrDefaultAsync(m => m.id == id);
            if (documentsIB == null)
            {
                return NotFound();
            }

            return View(documentsIB);
        }

        // POST: DocumentsIBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documentsIB = await _context.DocumentsIB.FindAsync(id);
            _context.DocumentsIB.Remove(documentsIB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentsIBExists(int id)
        {
            return _context.DocumentsIB.Any(e => e.id == id);
        }


        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file, [Bind("id,name,text")] DocumentsIB documentsIB)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = @"\Upload\Files";
                var uploadPath = _env.WebRootPath + filePath;

                //Create
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //Create Uniq file name
                var uniqFileName = Guid.NewGuid().ToString();
                var filename = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".")[1].ToLower());
                string fullPath = uploadPath + filename;

                filePath = filePath + @"\";
                var filesPath = @".." + Path.Combine(filePath, filename);
                documentsIB.name = file.FileName;
                documentsIB.text = Url.Content(fullPath);
                _context.DocumentsIB.Add(documentsIB);
                _context.SaveChanges();
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


                ViewData["FileLocation"] = filePath;
            }
            return View("../DocumentsIBs/Index", await _context.DocumentsIB.ToListAsync());
        }

        public async Task<IActionResult> GetFile(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var documentsIB = await _context.DocumentsIB.FindAsync(id);
            if (documentsIB == null)
            {
                return NotFound();
            }
            // Путь к файлу
            string file_path = Path.Combine(_env.ContentRootPath, documentsIB.text);
            // Тип файла - content-type
            string file_type = "application/pdf";
            return PhysicalFile(file_path, file_type);
        }
    }
}
