using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testad.Models
{
    public class DLPDark
    {
        public int id { get; set; }

        [Display(Name = "Полное имя")]

        public string name { get; set; }

        [Display(Name = "Идентификатор пользователя")]

        public string login { get; set; }

        [Display(Name = "Группы пользователя")]

        public string gro { get; set; }

        [Display(Name = "Расположение")]

        public string address { get; set; }

    }
}
