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
using testad.Migrations;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace testad.Controllers
{
    public class TablelawsController : Controller
    {
        private readonly MvcRequestContext _context;

        public TablelawsController(MvcRequestContext context)
        {
            _context = context;
        }

        // GET: PostLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tablelaws.ToListAsync());
        }
        // GET: InformationSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var informationSystem = await _context.Tablelaws
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
            var informationSystem = await _context.Tablelaws.FindAsync(id);
            _context.Tablelaws.Remove(informationSystem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Tables/Create
        public async Task<IActionResult> Create()
        {
            var dolzh = HttpContext.Session.GetObjectFromJson<List<string>>("posit");
            var syst = HttpContext.Session.GetObjectFromJson<List<string>>("inf");
            string structur = HttpContext.Session.GetString("spodr");
            var mess = HttpContext.Session.GetObjectFromJson<List<string>>("mess");
            IndexViewModel2 req = new IndexViewModel2 { InformationSystems = await _context.InformationSystems.ToListAsync(), PostLists = await _context.PostLists.ToListAsync(), Subdivitions = await _context.Subdivitions.ToListAsync(), DocumentsIBs = await _context.DocumentsIB.ToListAsync(), Rools = await _context.Rools.ToListAsync() };
            return View(req);
        }

        // POST: PostLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<string> names, [Bind("id,isystem,pos,spod,role")] Tablelaw lw)
        {
            int id = lw.id;
            string prov = "";
            int znak = 0;
            int indexI = 0;
            int indexJ = 0;
            List<string> G = new List<string> { };
            var proverka2 = await _context.Tablelaws.ToListAsync();
            foreach(var GG in proverka2)
            {
                G.Add(GG.spod);
                G.Add(GG.pos);
                G.Add(GG.isystem);
            }

            for (int i = 0; HttpContext.Session.GetObjectFromJson<List<string>>("posit").Count > i; i++)
            {
                for(int j =0; HttpContext.Session.GetObjectFromJson<List<string>>("inf").Count > j;j++)
                {
                    for(int item =0; item <names.Count; item ++)
                    {
                        indexI = 0;
                        indexJ = 0;
                        znak = 0;
                       
                        foreach(var index in names[item])
                        {
                            if(index != ';')
                            {
                                prov = prov + index;
                            }
                            else
                            {
                                if (indexI == 1 && indexJ == 1)
                                {
                                    lw.role = lw.role + prov + ";";
                                }
                                if (znak == 1 && Convert.ToInt32(prov) == j)
                                {
                                    indexJ = 1;
                                }
                                if (znak == 0 && Convert.ToInt32(prov) == i)
                                {
                                    indexI = 1;
                                }
                                znak++;
                                prov = "";
                            }
                        }
                    }
                    lw.pos = HttpContext.Session.GetObjectFromJson<List<string>>("posit")[i];
                    lw.isystem = HttpContext.Session.GetObjectFromJson<List<string>>("inf")[j];
                    lw.spod = HttpContext.Session.GetString("spodr");
                    lw.id = 0;
                    int flag = 0;
                    var proverka1 = await _context.Tablelaws.ToListAsync();
                    if(proverka1.Count != 0)
                    {
                        for(int ced=0; ced < G.Count;ced = ced + 3)
                        {
                                if (G[ced] == lw.spod)
                                {
                                    if (G[ced+1] == lw.pos)
                                    {
                                        if (G[ced+2] == lw.isystem)
                                        {
                                            HttpContext.Session.SetObjectAsJson("errors", lw.role);
                                            flag = 1;
                                        }
                                    }
                                }
     
                        }
                        
                    }
                    else if(proverka1.Count == 0)
                    {
                        _context.Add(lw);
                        await _context.SaveChangesAsync();
                    }
                    if(flag == 0)
                    {
                        _context.Add(lw);
                        await _context.SaveChangesAsync();
                    }
                    lw.role = "";
                }

            }
            return RedirectToAction(nameof(Index));
            /* if (ModelState.IsValid)
             {
                 _context.Add(lw);
                 await _context.SaveChangesAsync();
                 return View("../DocumentsIBs/Index", await _context.DocumentsIB.ToListAsync());
                 // return RedirectToAction(nameof(Index));
             }*/
            // return View("../DocumentsIBs/Index", await _context.DocumentsIB.ToListAsync());
        }
    }
}
