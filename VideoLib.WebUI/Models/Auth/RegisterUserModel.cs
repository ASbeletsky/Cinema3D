using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using VideoLib.Domian.Entities.AuthEntities;

namespace VideoLib.WebUI.Models.Auth
{
    public enum AuthType
    {
        Email,
        Facebook,
        Twitter
    }
    public class RegisterUserModel
    {
        public AuthType RegisterType { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string PhoneNumber { get; set; }
        public UserLoginInfo LoginProvider { get; set; }
        public List<Claim> UserClaims { get; set; }
        public string Object_id { get; set; }
    }
}