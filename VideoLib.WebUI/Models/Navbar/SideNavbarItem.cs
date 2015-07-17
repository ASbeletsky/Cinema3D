using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Navbar
{
    public class SideNavbarItem
    {
        public string nameOption { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string imageClass { get; set; }
        public string itemClass { get; set; }
    }
}