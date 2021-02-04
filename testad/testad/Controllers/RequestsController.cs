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
    
    public class RequestsController : Controller
    {

        private readonly MvcRequestContext _context;
        IHostingEnvironment _env;
        public RequestsController(MvcRequestContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {

            var username = User.Identity.Name;
          
            //depending on your environment, you may need to specify a container along with the domain
            //ex: new PrincipalContext(ContextType.Domain, "yourdomain", "OU=abc,DC=xyz")
            using ( var context = new PrincipalContext(ContextType.Domain, "astroblgaz.local"))//из объекта userprincipal можно получить информацию из AD
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                var u = new UserPrincipal(context) { SamAccountName = Environment.UserName };
                using (var search = new PrincipalSearcher(u))
                {
                    var user1 = (UserPrincipal)search.FindOne();
                    DirectoryEntry dirEntry = (DirectoryEntry)user.GetUnderlyingObject();
                    string dept = dirEntry.Properties["Department"].Value.ToString();
                    ViewData["Deportment"] = dept;
                    string dol = dirEntry.Properties["Title"].Value.ToString();
                    ViewData["Dolzh"] = dol;
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetString("Deportment", dept);
                    HttpContext.Session.SetString("Dolzh", dol);
                    HttpContext.Session.SetString("Email", user.EmailAddress);
                }
                ViewBag.UserName = user.Name;
                ViewData["value"] = "";
                TempData["Name"] = user.Name;
                ViewBag.EmailAddress = user.EmailAddress;
                ViewBag.Logon = user.LastLogon;
                ViewBag.Name = user.GivenName;
                if (user != null)
                {
                    string gr = "";
                    // fetch the group list
                    PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();//отображает принадлежность к группам 
                    foreach (var i in groups)
                    {
                        gr = i + ";";
                        if (gr == "WriterMSA;")
                        {
                            HttpContext.Session.SetString("GroupWr", gr);
                        }
                        if (gr == "AdminMSA;")
                        {
                            HttpContext.Session.SetString("GroupAd", gr);
                        }
                        if (gr == "BossDepartmentMSA;")
                        {
                            HttpContext.Session.SetString("GroupBd", gr);
                        }
                        if (gr == "WriteIS;")
                        {
                            HttpContext.Session.SetString("GroupIS", gr);
                        }
                        if (gr == "WriteIB;")
                        {
                            HttpContext.Session.SetString("GroupIB", gr);
                        }
                        if (gr == "WriteIT;")
                        {
                            HttpContext.Session.SetString("GroupIT", gr);
                        }
                    }


                }
            }
            /*    var runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                var pipeline = runspace.CreatePipeline();

                using (PowerShell powerShellInstance = PowerShell.Create())
                {
                    var initial = InitialSessionState.CreateDefault();
                    Console.WriteLine("Importing ServerManager module");
                    initial.ImportPSModule(new[] { "ServerManager" });
                    powerShellInstance.Runspace = runspace;
                    powerShellInstance.AddScript("Set-ExecutionPolicy Unrestricted");
                    //powerShellInstance.AddScript("Get-ExecutionPolicy");
                    powerShellInstance.Invoke();
                    Command cmd = new Command("C:\\Users\\kozlov.aa\\source\\repos\\testad\\testad\\bin\\Debug\\netcoreapp3.1\\powershell\\test.ps1");
                    CommandParameter testParam1 = new CommandParameter("FirstName", "Alexander");
                    CommandParameter testParam2 = new CommandParameter("LastName", "Kozlov");
                    CommandParameter testParam3 = new CommandParameter("Department", "ИТиС");
                    cmd.Parameters.Add(testParam1);
                    cmd.Parameters.Add(testParam2);
                    cmd.Parameters.Add(testParam3);
                    pipeline.Commands.Add(cmd);
                    var results = pipeline.Invoke();
                    pipeline.Dispose();
                    runspace.Dispose();
                    return View(await _context.Requests.ToListAsync());
                }*/
            return View(await _context.Requests.ToListAsync());
        }

      
        //GET: Requests/Sign/5
        [Authorize(Policy = "writermsa")]
        public async Task<IActionResult> Sign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var request = await _context.Requests.FindAsync(id);
            ViewBag.Marh = request.marh;
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "writermsa")]
        public async Task<IActionResult> Sign(int id, [Bind("id, fio, subdivision, numberphone, numberroom, inventorynumber, position, documents, baseservers, leadersubdivision, informationsystem, departmentib, departmentoz, status, marh, namecreated,  deportmencreated, positcreated, login, emailcreated, signIT, olduser, neuser")] Request request)
        {
            if (id != request.id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    request.marh = request.marh + 1;
                    
                    if (request.marh == 0 || request.marh==2)
                    {
                        
                        request.status = "Закрыта";
                        string fio = "";
                        string name = "";
                        int flag = 0;
                        if (request.marh == 2)
                        {
                            //Формирование ФИО
                            for (int j = 0; j < request.fio.Length; j++)
                            {
                                if (request.fio.Substring(j, 1) == " ")
                                {
                                    flag = flag + 1;
                                }
                                if (flag == 0)
                                {
                                    fio = fio + request.fio.Substring(j, 1);
                                }
                                else
                                {
                                    if (request.fio.Substring(j, 1) == " " && flag == 1)
                                    { }
                                    else
                                    {
                                        name = name + request.fio.Substring(j, 1);
                                    }
                                }
                            }

                            //Формиривание Описания 
                            string subdiv = "";
                            flag = 0;
                            var item = await _context.Subdivitions.ToListAsync();
                            foreach (Subdivition it in item)
                            {
                                if (it.subdiv == request.subdivision)
                                {
                                    subdiv = it.group;
                                }
                            }

                            //Группы  относящиеся к базовым сервисми
                            string group = "";
                            string internet = "";
                            string netfold = "";
                            string mailuser = "";
                            for (int j = 0; j < request.baseservers.Length; j++)
                            {
                                if (request.baseservers.Substring(j, 1) != ";")
                                {
                                    group = group + request.baseservers.Substring(j, 1);
                                }
                                else
                                {
                                    if (group == "Доступ в сеть интернет - Ограниченный")
                                    {
                                        internet = "AOG-Internet-Restricted";
                                    }
                                    if (group == "Доступ в сеть интернет - Стандартный")
                                    {
                                        internet = "AOG-Internet-Standard";
                                    }
                                    if (group == "Доступ к сетевому файловому ресурсу")
                                    {
                                        var sub = await _context.Subdivitions.ToListAsync();
                                        foreach (Subdivition gr in sub)
                                        {
                                            if (request.subdivision == gr.subdiv)
                                            {
                                                netfold = gr.group;
                                            }
                                        }
                                    }
                                    if(group == "Электронный почтовый ящик")
                                    {
                                        mailuser = "Создать Электронный почтовый ящик";
                                    }
                                    group = "";
                                    j++;
                                }
                            }

                            //Группы ИСиС
                            List<string> isands = new List<string>() { };
                            string text = "";
                           
                            for (int j = 0; j < request.informationsystem.Length; j++)
                            {
                                if (request.informationsystem.Substring(j, 1) != "-")
                                {
                                    text = text + request.informationsystem.Substring(j, 1);
                                }
                                if (request.informationsystem.Substring(j, 1) == ";")
                                {
                                    text = "";
                                   
                                }
                                if (request.informationsystem.Substring(j, 1) == "-")
                                {
                                    isands.Add(text);
                                    text = "";
                                   
                                    j++;
                                }
                            }

                            if (netfold != "")
                            {
                                isands.Add(netfold);
                            }
                            if (internet != "")
                            {
                                isands.Add(internet);
                            }
                            var runspace = RunspaceFactory.CreateRunspace();
                            runspace.Open();
                            var pipeline = runspace.CreatePipeline();

                            using (PowerShell powerShellInstance = PowerShell.Create())
                            {
                                var initial = InitialSessionState.CreateDefault();
                                Console.WriteLine("Importing ServerManager module");
                                initial.ImportPSModule(new[] { "ServerManager" });
                                powerShellInstance.Runspace = runspace;
                                powerShellInstance.AddScript("Set-ExecutionPolicy Unrestricted");
                                //powerShellInstance.AddScript("Get-ExecutionPolicy");
                                powerShellInstance.Invoke();
                                Command cmd = new Command("C:\\Users\\kozlov.aa\\source\\repos\\testad\\testad\\bin\\Debug\\netcoreapp3.1\\powershell\\test.ps1");
                                CommandParameter testParam1 = new CommandParameter("FirstName", name);
                                CommandParameter testParam2 = new CommandParameter("LastName", fio);
                                CommandParameter testParam3 = new CommandParameter("Description", subdiv);
                                CommandParameter testParam4 = new CommandParameter("Office", request.numberroom);
                                CommandParameter testParam5 = new CommandParameter("TelephoneNumber", request.numberphone);
                                CommandParameter testParam6 = new CommandParameter("SAN", request.login);
                                CommandParameter testParam7 = new CommandParameter("Title", request.position);
                                CommandParameter testParam8 = new CommandParameter("Department", request.subdivision);
                                CommandParameter testParam9 = new CommandParameter("Company", "АО Газпром газораспределение Астрахань");
                                //CommandParameter testParam10 = new CommandParameter("Interner", internet);
                                //  CommandParameter testParam11 = new CommandParameter("NetFold", netfold);
                                // CommandParameter testParam12 = new CommandParameter("InformSyst", isands);
                                cmd.Parameters.Add(testParam1);
                                cmd.Parameters.Add(testParam2);
                                cmd.Parameters.Add(testParam3);
                                cmd.Parameters.Add(testParam4);
                                cmd.Parameters.Add(testParam5);
                                cmd.Parameters.Add(testParam6);
                                cmd.Parameters.Add(testParam7);
                                cmd.Parameters.Add(testParam8);
                                cmd.Parameters.Add(testParam9);
                                //  cmd.Parameters.Add(testParam10);
                                //  cmd.Parameters.Add(testParam11);
                                // cmd.Parameters.Add(testParam12);
                                pipeline.Commands.Add(cmd);
                                var results = pipeline.Invoke();
                                pipeline.Dispose();
                                runspace.Dispose();
                                // return View(await _context.Requests.ToListAsync());
                            }
                            //Добавление групп пользователю 
                            if (isands != null)
                            {
                                foreach (string ff in isands)
                                {
                                    var runspace1 = RunspaceFactory.CreateRunspace();
                                    runspace1.Open();
                                    var pipeline1 = runspace1.CreatePipeline();

                                    using (PowerShell powerShellInstance = PowerShell.Create())
                                    {
                                        var initial = InitialSessionState.CreateDefault();
                                        Console.WriteLine("Importing ServerManager module");
                                        initial.ImportPSModule(new[] { "ServerManager" });
                                        powerShellInstance.Runspace = runspace1;
                                        powerShellInstance.AddScript("Set-ExecutionPolicy Unrestricted");
                                        //powerShellInstance.AddScript("Get-ExecutionPolicy");
                                        powerShellInstance.Invoke();
                                        Command cmd = new Command("C:\\Users\\kozlov.aa\\source\\repos\\testad\\testad\\bin\\Debug\\netcoreapp3.1\\powershell\\test1.ps1");
                                        CommandParameter testParam12 = new CommandParameter("InformSyst", ff);
                                        CommandParameter testParam6 = new CommandParameter("SAN", request.login);
                                        cmd.Parameters.Add(testParam12);
                                        cmd.Parameters.Add(testParam6);
                                        pipeline1.Commands.Add(cmd);
                                        var results1 = pipeline1.Invoke();
                                        pipeline1.Dispose();
                                        runspace1.Dispose();
                                    }
                                }
                            }

                            //Добавление почты
                            var mail = await _context.Mails.ToListAsync();

                            foreach (Mail it in mail)
                            {
                                MailMessage message = new MailMessage();
                                message.IsBodyHtml = true;
                                message.From = new MailAddress("msa@astroblgaz.ru", "Информационная система доступа");
                                message.To.Add("kozlov.aa@astroblgaz.ru");
                                message.Subject = "Сообщение от ";
                                message.Body = "<div style=\"color: dark;\"><p style=\"text - align: center;\"><strong>Учетная запись:</strong>" + request.fio  + " Идентификатор аккаунта - " + request.login + "</p>" +
                                    "<p style=\"text - align: justify; \"><strong>Создана в Active Directory:</strong> - ДА</p>" +
                                    "<p style=\"text - align: justify; \"><strong>Информационные серивисы и ресурсы:&nbsp;</strong>" + request.informationsystem + " " + request.baseservers + "</p>" +
                                    "<p style=\"text - align: justify; \"><strong></strong></p>" +
                                    "<p style=\"text - align: justify; \"><strong>Необходимые работы </strong>" + mailuser + " - " + request.fio + "</p>" +
                                    "</div>";

                                using (SmtpClient client = new SmtpClient(it.address))
                                {
                                    client.Credentials = new NetworkCredential(it.login, it.password);
                                    client.Port = it.port;
                                    client.EnableSsl = false;
                                    client.Send(message);

                                }
                            }
                           
                        }
                    }
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.id))
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
            return View(request);
        }

        // GET: Requests/Details/5
        
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public async Task<IActionResult> Create()
        {
            var mir = await _context.Subdivitions.ToListAsync();
            int flagus = 0;
            foreach (var sub in mir)
            {
                if (sub.subdiv == HttpContext.Session.GetString("Deportment"))
                {
                    flagus = 1;
                }
            }
            if (flagus == 0)
            {
                return RedirectToAction("Block");
                        }
            IndexViewModel req = new IndexViewModel{InformationSystems = await _context.InformationSystems.ToListAsync(), PostLists = await _context.PostLists.ToListAsync(), Subdivitions = await _context.Subdivitions.ToListAsync(), DocumentsIBs = await _context.DocumentsIB.ToListAsync(), Tablelaws = await _context.Tablelaws.ToListAsync()};
            return View(req);
        }



        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

       
        public async Task<IActionResult> Create(List<string> names, [Bind("id, fio, subdivision, numberphone, numberroom, inventorynumber, position, documents, baseservers, leadersubdivision, informationsystem, departmentib, departmentoz, status, marh, namecreated,  deportmencreated, positcreated, login, emailcreated, signIT, olduser, neuser")] Request request)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PrincipalContext(ContextType.Domain, "astroblgaz.local"))//из объекта userprincipal можно получить информацию из AD
                {
                    var user = UserPrincipal.FindByIdentity(context, request.fio);
                    if (request.olduser == true)
                    {
                        if (user != null)
                        {
                            request.login = user.SamAccountName;
                            request.namecreated = HttpContext.Session.GetString("Name");
                            request.positcreated = HttpContext.Session.GetString("Dolzh");
                            request.deportmencreated = HttpContext.Session.GetString("Deportment");
                            request.emailcreated = HttpContext.Session.GetString("Email");
                            _context.Add(request);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        string ret = "";
                        int st = 0;
                        string[] rus = {"А","а","Б","б","В","в","Г","г","Д","д","Е","е","Ё","ё","Ж","ж", "З","з","И","и","Й","й","К","к","Л","л","М","м", "Н","н",
          "О","о","П","п","Р","р","С","с","Т","т","У","у","Ф","ф","Х","х", "Ц","ц", "Ч","ч", "Ш","ш", "Щ", "щ",  "Ъ","ъ", "Ы","ы","Ь","ь",
          "Э","э","Ю", "ю","Я","я" };
                        string[] eng = {"a","a","b","b","v","v","g","g","d","d","e","e","e","e","zh","zh","z","z","i","i","y","y","k","k","l","l","m","m","n","n",
          "o","o","p","p","r","r","s","s","t","t","u","u","f","f","kh","kh","c","c","ch","ch","sh","sh","shch","shch",null,null,"y","y",null,null,
          "e","e","yu","yu","ya","ya"};
                        string[] rus1 = {"А",null,"Б",null,"В",null,"Г",null,"Д",null,"Е",null,"Ё",null,"Ж",null, "З",null,"И",null,"Й",null,"К",null,"Л",null,"М",null, "Н",null,
          "О",null,"П",null,"Р",null,"С",null,"Т",null,"У",null,"Ф",null,"Х",null, "Ц",null, "Ч",null, "Ш",null, "Щ",null, "Ъ",null, "Ы",null,"Ь",null,
          "Э",null,"Ю", null,"Я",null};
                        string[] eng1 = {"a",null,"b",null,"v",null,"g",null,"d",null,"e",null,"e",null,"zh",null,"z",null,"i",null,"y",null,"k",null,"l",null,"m",null,"n",null,
          "o",null,"p",null,"r",null,"s",null,"t",null,"u",null,"f",null,"kh",null,"c",null,"ch",null,"sh",null,"dhch",null,null,null,"y",null,null,null,
          "e",null,"yu",null,"ya",null};
                        for (int j = 0; j < request.fio.Length; j++)
                        {
                            for (int i = 0; i < rus.Length; i++)
                            {
                                if (request.fio.Substring(j, 1) == " " && st == 0)
                                {
                                    ret += ".";
                                    st = 1;
                                }
                                if (request.fio.Substring(j, 1) == rus[i] && st == 0) ret += eng[i];
                                if (request.fio.Substring(j, 1) == rus1[i] && st == 1) ret += eng1[i];
                            }
                        }
                        request.login = ret;
                        request.namecreated = HttpContext.Session.GetString("Name");
                        request.positcreated = HttpContext.Session.GetString("Dolzh");
                        request.deportmencreated = HttpContext.Session.GetString("Deportment");
                        request.emailcreated = HttpContext.Session.GetString("Email");
                        string strok = "";
                        string strok2 = "";
                        int count = 0;
                        int first = 0;
                        for(int item =0; item < names.Count; item++)
                        {
                            count = 0;
                            foreach (var hh in names[item])
                            {
                                if(hh !='*')
                                {
                                    strok = strok + hh;
                                }
                                if(hh== ';' || hh == '*')
                                {
                                    count++;
                                    if(count == 1)
                                    {
                                        if(strok == request.position)
                                        {
                                            first = 1;

                                        }
                                    }
                                    if(first==1 && count == 2)
                                    {
                                        strok2 = strok2+strok + "-";
                                    }
                                    if (first == 1 && count == 3)
                                    {
                                        strok2 = strok2 + strok;
                                        first = 0;
                                    }
                                    strok = "";
                                }
                            }
                        }
                        request.informationsystem = strok2;
                        _context.Add(request);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                    }
                }
               
            return View(request);
        }

        // GET: Requests/Delete/5
        [Authorize(Policy = "adminmsa")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "adminmsa")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.id == id);
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
