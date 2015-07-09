using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Auth
{
    public class LoginOptionsViewModel
    {
        public string Provider { get; set; }
        public string Token { get; set; }
        public string TwiterTokenSecret { get; set; }
        public int VkUserId { get; set; }
    }
}