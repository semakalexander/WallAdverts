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

        [RegularExpression("[a-zA-Z]+[a-zA-z0-9]{3,19}", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRegExLogin")]
        [Remote("CheckLogin", "Home", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLoginBusy")]
        [Required(ErrorMessageResourceType =typeof(Resources.Resource),ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name ="UserLogin",ResourceType = typeof(Resources.Resource))]
        public string Login { get; set; }

        [RegularExpression("[a-zA-z0-9]{6,18}", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRegExPassword")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRequiredField")]
        [Display(Name ="Password",ResourceType =typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RegularExpression("[0-9]{0,12}", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRegExNumber")]
        [StringLength(12, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorLengthNumber")]
        [Display(Name = "Number", ResourceType = typeof(Resources.Resource))]
        public string Number { get; set; }

        [Remote("CheckEmail", "Home", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorEmailBusy")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRequiredField")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorTypeEmail")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "ErrorRequiredField")]
        [DataType(DataType.Date)]
        [Display(Name = "DateBirthday", ResourceType = typeof(Resources.Resource))]
        public DateTime DateBirthday { get; set; }

        [Display(Name = "DateRegister", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Date)]
        public DateTime DateRegister { get; set; }

        [Display(Name = "Role", ResourceType = typeof(Resources.Resource))]
        public string Role { get; set; }

        public string ImageSrc { get; set; }
    }
}