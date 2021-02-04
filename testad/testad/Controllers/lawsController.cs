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
    public class lawsController : Controller
    {
        private readonly MvcRequestContext _context;

        public lawsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: PostLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.laws.ToListAsync());
        }



        // GET: PostLists/Create
        public async Task<IActionResult> Create()
        {
            IndexViewModel1 req = new IndexViewModel1 { InformationSystems = await _context.InformationSystems.ToListAsync(), PostLists = await _context.PostLists.ToListAsync(), Subdivitions = await _context.Subdivitions.ToListAsync(), DocumentsIBs = await _context.DocumentsIB.ToListAsync() };
            return View(req);
        }

        // POST: PostLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,infsystem ,position,sp")] law lw)
        {
            if (ModelState.IsValid)
            {
                string inform = "";
                int item1 = 0;
                List<string> inf = new List<string> { };
                List<int> numberinf = new List<int> { };
                for (int i = 0; lw.infsystem.Length >i; i++)
                { 
                    if(lw.infsystem.Substring(i, 1)!=";")
                    {
                        inform = inform + lw.infsystem.Substring(i, 1);
                    }
                    else
                    {
                        inf.Add(inform);
                        numberinf.Add(item1);
                        item1++;
                        inform = "";
                    }
                }
                string pos = "";
                List<string> posit = new List<string> { };
                List<int> numberposit = new List<int> { };
                int item = 0;
                for (int i = 0; lw.position.Length > i; i++)
                {
                    if (lw.position.Substring(i, 1) != ";")
                    {
                        pos = pos + lw.position.Substring(i, 1);
                    }
                    else
                    {
                        posit.Add(pos);
                        numberposit.Add(item);
                        item ++;
                        pos = "";
                    }
                }
                HttpContext.Session.SetString("spodr", lw.sp);
                HttpContext.Session.SetObjectAsJson("numberposit", numberposit);
                HttpContext.Session.SetObjectAsJson("nemberinf", numberinf);
                HttpContext.Session.SetObjectAsJson("posit",posit);
                HttpContext.Session.SetObjectAsJson("inf", inf);
                _context.Add(lw);
                await _context.SaveChangesAsync();
                return Redirect("../Tablelaws/Create");
               // return RedirectToAction(nameof(Index));
            }
            return View("../DocumentsIBs/Index", await _context.DocumentsIB.ToListAsync());
        }
       

        // GET: PostLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mail = await _context.Mails.FindAsync(id);
            if (mail == null)
            {
                return NotFound();
            }
            return View(mail);
        }

        // POST: PostLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,address,port,login,password")] Mail mail)
        {
            if (id != mail.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailExists(mail.id))
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
            return View(mail);
        }

        // GET: PostLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailList = await _context.Mails
                .FirstOrDefaultAsync(m => m.id == id);
            if (mailList == null)
            {
                return NotFound();
            }

            return View(mailList);
        }

        // POST: PostLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailList = await _context.Mails.FindAsync(id);
            _context.Mails.Remove(mailList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailExists(int id)
        {
            return _context.Mails.Any(e => e.id == id);
        }
    }
}
