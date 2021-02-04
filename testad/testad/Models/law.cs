using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testad.Models
{
    public class law
    {
        public int id { get; set; }

        [Display(Name = "Информационная система")]

        public string infsystem { get; set; }

        [Display(Name = "Должностной список")]

        public string position { get; set; }

        [Display(Name = "Структурное подразделение")]

        public string sp { get; set; }
    }
}
