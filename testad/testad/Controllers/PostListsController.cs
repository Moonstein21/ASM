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
    public class PostListsController : Controller
    {
        private readonly MvcRequestContext _context;

        public PostListsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: PostLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostLists.ToListAsync());
        }

       

        // GET: PostLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Position,postlistlvl")] PostList postList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postList);
        }

        // GET: PostLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postList = await _context.PostLists.FindAsync(id);
            if (postList == null)
            {
                return NotFound();
            }
            return View(postList);
        }

        // POST: PostLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Position,postlistlvl")] PostList postList)
        {
            if (id != postList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostListExists(postList.id))
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
            return View(postList);
        }

        // GET: PostLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postList = await _context.PostLists
                .FirstOrDefaultAsync(m => m.id == id);
            if (postList == null)
            {
                return NotFound();
            }

            return View(postList);
        }

        // POST: PostLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postList = await _context.PostLists.FindAsync(id);
            _context.PostLists.Remove(postList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostListExists(int id)
        {
            return _context.PostLists.Any(e => e.id == id);
        }
    }
}
