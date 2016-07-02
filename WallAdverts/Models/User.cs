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

        [RegularExpression("[a-zA-Z]+[a-zA-z0-9]{3,19}", ErrorMessage = "Логін повинен складатись з латинських літер(починатись обов'язково з літери) та цифр, розмірністю від 4 до 20 символів")]
        [Remote("CheckLogin", "Home", ErrorMessage = "Цей логін вже зайнятий")]
        [Required(ErrorMessage = "Це обов'язкове поле!")]
        public string Login { get; set; }

        [RegularExpression("[a-zA-z0-9]{6,18}", ErrorMessage = "Пароль повинен складатись з латинських літер та цифр, розмірністю від 6 до 18 символів")]
        [Required(ErrorMessage = "Це обов'язкове поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RegularExpression("[0-9]{0,12}", ErrorMessage = "Номер повинен складатись з цифр та не перевищувати 12 символів")]
        [StringLength(12, ErrorMessage = "Недопустима довжина телефону")]
        public string Number { get; set; }

        [Remote("CheckEmail", "Home", ErrorMessage = "Цей email уже використовується")]
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