using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WallAdverts.Models
{
    public class Advert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public int AuthorId { get; set; }
        public string ImageSrc { get; set; }
    }
}