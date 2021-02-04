using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace testad.Models
{
    public class Subdivition
    {
        public  int id { get; set; }
        
        [Display(Name = "Подразделение")]
        public string subdiv { get; set; }
       
        [Display(Name = "Наименование группы")]
        public string group { get; set; }
    }
}
