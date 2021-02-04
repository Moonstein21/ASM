using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace testad.Models
{
    public class PostList
    {
        public int id { get; set;}

        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Уровень должности")]

        public int postlistlvl { get; set; }
    }
}
