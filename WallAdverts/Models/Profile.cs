using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WallAdverts.Models
{
    public class MyProfile
    {
        public User User { get; set; }
        public List<Advert> Adverts { get; set; }
    }
}