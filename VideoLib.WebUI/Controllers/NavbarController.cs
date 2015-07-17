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
                new SideNavbarItem {nameOption = "Главная", controller = "Admin", action = "Index", imageClass = "", itemClass = ""},
                new SideNavbarItem {nameOption = "Фильмы", controller = "", action = "", imageClass = "", itemClass = ""},
                new SideNavbarItem {nameOption = "Настройки категорий", controller = "", action = "", imageClass = "", itemClass = ""},
                new SideNavbarItem {nameOption = "Пользователи", controller = "", action = "", imageClass = "", itemClass = ""},            
            };
            sideItems.Where(item => item.controller == controller && item.action == action)
                     .Select(item =>
                     {
                         item.itemClass += "active";
                         return item;
                     });

            return PartialView("_navbarSide", sideItems);
        }

        public ActionResult NavbarTop()
        {
            return PartialView("_navbarTop");
        }
    }
}