using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testad.Data;
using testad.Models;
using testad.Models.ViewModels;
using System.Web;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using System.Management.Automation.Runspaces;
using System.Net.Mail;
using System.Net; 

namespace testad.Controllers
{
    [Authorize(Policy = "adminmsa")]
    public class InformationSystemsController : Controller
    {
        private readonly MvcRequestContext _context;

        public InformationSystemsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: InformationSystems
        public async Task<IActionResult> Index()
        {
            return View(await _context.InformationSystems.ToListAsync());
        }



        // GET: InformationSystems/Create
        public async Task<IActionResult> Create()
        {
            IndexViewModel3 req = new IndexViewModel3 { PostLists = await _context.PostLists.ToListAsync(), Subdivitions = await _context.Subdivitions.ToListAsync(), DocumentsIBs = await _context.DocumentsIB.ToListAsync(), Rools = await _context.Rools.ToListAsync() };
            return View(req);
        }

        // POST: InformationSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ListSystem,lvrol")] InformationSystem informationSystem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(informationSystem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(informationSystem);
        }

        // GET: InformationSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationSystem = await _context.InformationSystems.FindAsync(id);
            if (informationSystem == null)
            {
                return NotFound();
            }
            IndexViewModel3 req = new IndexViewModel3 { PostLists = await _context.PostLists.ToListAsync(), Subdivitions = await _context.Subdivitions.ToListAsync(), DocumentsIBs = await _context.DocumentsIB.ToListAsync(), Rools = await _context.Rools.ToListAsync() };
            return View(informationSystem);
        }

        // POST: InformationSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ListSystem,lvrol")] InformationSystem informationSystem)
        {
            if (id != informationSystem.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(informationSystem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InformationSystemExists(informationSystem.id))
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
            return View(informationSystem);
        }

        // GET: InformationSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationSystem = await _context.InformationSystems
                .FirstOrDefaultAsync(m => m.id == id);
            if (informationSystem == null)
            {
                return NotFound();
            }

            return View(informationSystem);
        }

        // POST: InformationSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var informationSystem = await _context.InformationSystems.FindAsync(id);
            _context.InformationSystems.Remove(informationSystem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InformationSystemExists(int id)
        {
            return _context.InformationSystems.Any(e => e.id == id);
        }
    }
}
