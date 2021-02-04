using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace testad.Models
{
    public class InformationSystem
    {
        public int id { get; set; }

        [Display(Name = "Информационные системы")]
        public string ListSystem{ get; set; }

        [Display(Name = "Роли")]
        public string lvrol { get; set; }
    }
}
