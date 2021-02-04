using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testad.Models
{
    public class Rool
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public string rollic { get; set; }

    }
}
