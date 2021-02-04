using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testad.Models
{
    public class Mail
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "SMTP - адрес сервера")]
        public string address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль пользователя")]
        public string password { get; set; }

        [Required]
        [Display(Name = "SMTP - порт сервера")]
        public int port {get; set;}

        [Required]
        [Display(Name = "Пользователь")]
        public string login { get; set; }
    }
}
