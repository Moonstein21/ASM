using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace testad.Models
{
    public class DocumentsIB
    {
        public  int id { get; set; }

        [Display(Name = "Наименование документа")]
        public string name { get; set; }

        [Display(Name = "Текст")]
        public string text { get; set; }
      

}
}
