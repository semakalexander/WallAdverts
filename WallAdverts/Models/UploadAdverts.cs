using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WallAdverts.Models
{
    public class UploadAdverts
    {
        public HttpPostedFileBase MyFile { get; set; }
        public List<Advert> Adverts { get; set; }
    }
}