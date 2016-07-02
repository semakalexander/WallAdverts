using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WallAdverts.Models
{
    public class User
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Remote("CheckLogin", "Home", ErrorMessage = "Цей логін вже зайнятий")]
        [Required(ErrorMessage = "Це обов'язкове поле!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Це обов'язкове поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(12, ErrorMessage = "Недопустима довжина телефону")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Це обов'язкове поле!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Введіть коректну адресу")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Це обов'язкове поле!")]
        [DataType(DataType.Date)]
        public DateTime DateBirthday { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateRegister { get; set; }

        public string ImageSrc { get; set; }
    }
    }