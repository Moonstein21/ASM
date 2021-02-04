using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testad.Models
{
    public class Tablelaw
    {
        public int id { get; set; }

        [Display(Name = "Информационная система")]

        public string isystem { get; set; }

        [Display(Name = "Должностной список")]

        public string pos { get; set; }

        [Display(Name = "Структурное подразделение")]

        public string spod { get; set; }

        [Display(Name = "Роли")]

        public string role { get; set; }
    }
}
