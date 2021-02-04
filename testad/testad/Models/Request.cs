using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using testad.Models;

namespace testad.Models
{
    public class Request
    {
        //класс для управления заявками

        
        public int id { get; set; }

     
        [Display(Name = "ФИО")]
        [Required]
        [StringLength(60)]
        public string fio { get; set; }//фамилия, имя и отчетсво пользователя
        [Required]
        [StringLength(60)]
        [Display(Name = "Подразделение")]
        public string subdivision { get; set; }//подразделение
        [Display(Name = "Телефон")]
        [Phone]
        [Required]
        public string numberphone { get; set; }//Номер телефона пользователя
        [Display(Name = "Кабинет")]
        public int numberroom { get; set; }// Номер кабинета 
        [Display(Name = "Инв № ПЭВМ")]
        public string inventorynumber { get; set; }// инвентарный номер ПЭВМ
        [Display(Name = "Должность")]
        [Required]
        [StringLength(60)]
        public string position { get; set; }// Занимаемая должность
        [Display(Name = "Ознакомлен с памяткой по ИБ")]
        [Required]
        public bool documents { get; set; }// Параметр ознакомления с документами по Информационной безопасности
        [Display(Name = "Базовые сервисы")]
        public string baseservers { get; set; }//Перечень базовых сервисов
        [Display(Name = "Соглосование с руководителем структурного подразделения")]
        public bool leadersubdivision { get; set; } // подпись руководителя структурного подразделения
        [Display(Name = "Информармационные системы")]
        public string informationsystem { get; set; } // Наименование информационных систем
        [Display(Name = "Соглосование с ответственным по ИБ")]
        public bool departmentib { get; set; }// индикатор подписи отдела информационной безопасности
        [Display(Name = "Соглосование с ответственными по ИСиС")]
        public bool  departmentoz { get; set; } //индикатор подписи подразделение жксплуатации ОЗ

        [Display(Name = "Статус")]
        public string status { get; set; }//Индикатор заявки

        [Display(Name = "Этап")]
        public int marh { get; set; }
        [Display(Name = "ФИО создателя заявки")]
        public string namecreated { get; set; }
        [Display(Name = "Структурное подразделение создателя заявки")]
        public string deportmencreated { get; set; }
        [Display(Name = "Должность создателя заявки")]
        public string positcreated { get; set; }
        [Display(Name = "Логин")]
        public string login { get; set; }
        
        [Display(Name = "Email")]
        public string emailcreated { get; set; }
        [Display(Name = "Соглосование руководителя отдела ИТиС")]
        public bool signIT { get; set; }
        [Display(Name = "Заявка на существующего пользователя")]
        public bool olduser { get; set; }
        [Display(Name ="Заявка на нового пользователя")]
        public bool newuser { get; set; }
        public bool example { get; set; }



    }
}
