using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoLib.WebUI.Models.Auth;

namespace VideoLib.WebUI.Models.Admin.User
{

    public class UserTableViewModel
    {

        public UserTableViewModel(string login)
        {
            this.AuthType = ConvertToAuthType(login);
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public AuthType AuthType { get; set; }
        public string Email { get; set; }
        public string PageInSN { get; set; }
   
        private AuthType ConvertToAuthType(string login)
        {
            if (login == "TwitterUser")
                return Auth.AuthType.Twitter;
            if (login == "FacebookUser")
                return Auth.AuthType.Facebook;
            return Auth.AuthType.Email;
        }
    }
}