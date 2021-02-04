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
    public class SubdivitionsController : Controller
    {
        private readonly MvcRequestContext _context;

        public SubdivitionsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: Subdivitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subdivitions.ToListAsync());
        }

        // GET: Subdivitions/Details/5
      

        // GET: Subdivitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subdivitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,subdiv,group")] Subdivition subdivition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subdivition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subdivition);
        }

        // GET: Subdivitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subdivition = await _context.Subdivitions.FindAsync(id);
            if (subdivition == null)
            {
                return NotFound();
            }
            return View(subdivition);
        }

        // POST: Subdivitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,subdiv,group")] Subdivition subdivition)
        {
            if (id != subdivition.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subdivition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubdivitionExists(subdivition.id))
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
            return View(subdivition);
        }

        // GET: Subdivitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subdivition = await _context.Subdivitions
                .FirstOrDefaultAsync(m => m.id == id);
            if (subdivition == null)
            {
                return NotFound();
            }

            return View(subdivition);
        }

        // POST: Subdivitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subdivition = await _context.Subdivitions.FindAsync(id);
            _context.Subdivitions.Remove(subdivition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubdivitionExists(int id)
        {
            return _context.Subdivitions.Any(e => e.id == id);
        }
    }
}
