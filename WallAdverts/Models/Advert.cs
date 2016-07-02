﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WallAdverts.Models
{
    public class Advert
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Це обов'язкове поле!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Це обов'язкове поле!")]
        public string Description { get; set; }


        public DateTime DateCreate { get; set; }


        public int AuthorId { get; set; }

        public string AuthorName { get; set; }


        public string ImageSrc { get; set; }
    }
}