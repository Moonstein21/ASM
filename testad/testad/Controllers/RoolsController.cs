using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testad.Data;
using testad.Models;

namespace testad.Controllers
{
    [Authorize(Policy = "adminmsa")]
    public class RoolsController : Controller
    {
        private readonly MvcRequestContext _context;

        public RoolsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: InformationSystems
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rools.ToListAsync());
        }



        // GET: InformationSystems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InformationSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,rollic")] Rool rool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rool);
        }

        // GET: InformationSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rool = await _context.Rools.FindAsync(id);
            if (rool == null)
            {
                return NotFound();
            }
            return View(rool);
        }

        // POST: InformationSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,rollic")] Rool rool)
        {
            if (id != rool.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoolExists(rool.id))
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
            return View(rool);
        }

        // GET: InformationSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rool = await _context.Rools
                .FirstOrDefaultAsync(m => m.id == id);
            if (rool == null)
            {
                return NotFound();
            }

            return View(rool);
        }

        // POST: InformationSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rool = await _context.Rools.FindAsync(id);
            _context.Rools.Remove(rool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoolExists(int id)
        {
            return _context.Rools.Any(e => e.id == id);
        }
    }
}
