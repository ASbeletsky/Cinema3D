using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin.User
{
    public class UserIndexViewModel
    {
        public List<UserTableViewModel> UsersTable { get; set; }
        public UserShortInfoViewModel UserShortInfo { get; set; }
    }
}