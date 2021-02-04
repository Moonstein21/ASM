using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testad.Data;
using testad.Models;

namespace testad.Controllers
{
    public class examplesController : Controller
    {
        private readonly MvcRequestContext _context;

        public examplesController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: examples
        public async Task<IActionResult> Index()
        {
            return View(await _context.example.ToListAsync());
        }

        // GET: examples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.example
                .FirstOrDefaultAsync(m => m.id == id);
            if (example == null)
            {
                return NotFound();
            }

            return View(example);
        }

        // GET: examples/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: examples/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,examp")] example example)
        {
            if (ModelState.IsValid)
            {
                _context.Add(example);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(example);
        }

        // GET: examples/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.example.FindAsync(id);
            if (example == null)
            {
                return NotFound();
            }
            return View(example);
        }

        // POST: examples/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,examp")] example example)
        {
            if (id != example.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(example);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!exampleExists(example.id))
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
            return View(example);
        }

        // GET: examples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var example = await _context.example
                .FirstOrDefaultAsync(m => m.id == id);
            if (example == null)
            {
                return NotFound();
            }

            return View(example);
        }

        // POST: examples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var example = await _context.example.FindAsync(id);
            _context.example.Remove(example);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool exampleExists(int id)
        {
            return _context.example.Any(e => e.id == id);
        }
    }
}
