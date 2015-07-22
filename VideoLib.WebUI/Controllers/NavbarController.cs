using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.WebUI.Models.Navbar;

namespace VideoLib.WebUI.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        public ActionResult NavbarSide(string controller, string action)
        {
            var sideItems = new List<SideNavbarItem>()
            {
                new SideNavbarItem {nameOption = "Главная", controller = "Admin", action = "Main", imageClass = "fa fa-home fa-fw", itemClass = ""},
                new SideNavbarItem {nameOption = "Фильмы", controller = "Admin", action = "FilmIndex", imageClass = "fa fa-film", itemClass = ""},
                new SideNavbarItem {nameOption = "Пользователи", controller = "Admin", action = "UserIndex", imageClass = "fa fa-user", itemClass = ""},
                new SideNavbarItem {nameOption = "Активность", controller = "", action = "", imageClass = "fa fa-child", itemClass = ""},            
            };
            foreach(var item in sideItems)
            {
                if(item.controller == controller && item.action == action )
                    item.itemClass = "active";
            }

            return PartialView("_navbarSide", sideItems);
        }

        public ActionResult NavbarTop()
        {
            return PartialView("_navbarTop");
        }
    }
}