using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using VideoLib.Domian.Entities.AuthEntities;
using VideoLib.WebUI.Models.Auth;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using TweetSharp;

namespace VideoLib.WebUI.Controllers
{
    public class AuthController : Controller
    {
                                            #region Managers
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        private UserManagerIntPK UserManager
        {
            get { return  HttpContext.GetOwinContext().GetUserManager<UserManagerIntPK>(); }
        }
        public SignInManagerIntPK SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManagerIntPK>(); }
        }
                                                #endregion

                                    #region Server OWIN-based Authentification
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //GET: VideoLib/Login
        public ActionResult Login(string provider, string returnUrl)
        {
            // Запрос перенаправления к внешнему поставщику входа
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl }));
        }
        //POST: VideoLib/ExternalLoginCallback
        [OverrideAuthentication]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {            
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var user = await UserManager.FindAsync(loginInfo.Login);
            var LoginSuccess =  await ExternalLogin(user, loginInfo);
            if(LoginSuccess)
            {
                return Redirect(returnUrl);
            }

            return View("Login Failure");
        }

        private async Task<bool> ExternalLogin(UserIntPK user, ExternalLoginInfo loginInfo)
        {
            bool loginSuccess = false;
            if (user != null)
            {
                loginSuccess = await SignInAsync(loginInfo, false);
            }
            else
            {
                user = new UserIntPK()
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.ExternalIdentity.Name ?? loginInfo.Email
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (result.Succeeded)
                    {
                        loginSuccess = await SignInAsync(loginInfo, false);
                    }
                }
            }
            return loginSuccess;
        }

        private async Task<bool> SignInAsync(ExternalLoginInfo loginInfo, bool isPersistent)
        {
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            if (result == SignInStatus.Success)
                return true;
            return false;
        }

        // VideoLub/Logout
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Auth");
        }
                                                #endregion
        //Helper Auth class
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary["XsrfId"] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

                            #region Mobile Token-based Authentification
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginMobile (string loginOptionsJson)
        {
            LoginOptionsViewModel options = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginOptionsViewModel>(loginOptionsJson);
           
           /* new LoginOptionsViewModel
            {
                Provider = "Twitter",
                Token = "3354818793-IqNx4En3lxwDKzGBo6mBRtFCOLS0yfh7IMXo1I5",
                TwiterTokenSecret = "aS94TY5fdPa5Z6UZMsQM84UQU5HROdyNyHqGMxlAGkKxT"

            };*/

            var tokenExpirationTimeSpan = TimeSpan.FromDays(14);
            UserViewModel userModel = await VerifyAccessToken(options);
            if(userModel != null)
            {
                ExternalLoginInfo loginInfo = new ExternalLoginInfo()
                {
                    Email = userModel.email,
                    Login = new UserLoginInfo("Facebook", userModel.id),
                };

                var user = await UserManager.FindAsync(loginInfo.Login);
                var LoginSuccess = await ExternalLogin(user, loginInfo);
                if (LoginSuccess)
                {
                    await UserManager.AddClaimAsync(user.Id, new Claim("FacebookAccessToken", options.Token));
                    return RedirectToAction("Index", "VideoLib");
                }
            }            

            return View("Login Failure");          
        }

        private async Task<UserViewModel> VerifyAccessToken(LoginOptionsViewModel options)
        {
            switch (options.Provider)
            {
                case "Facebook" :
                    return await FacebookUser(options.Token);
                case "Twitter"  :
                    return TwitterUser(options.Token, options.TwiterTokenSecret);
                case "Vk" :
                    return await VkUser(options.Token, options.VkUserId);
            }
            return null;            
        }

        private async Task<UserViewModel> FacebookUser(string accessToken)
        {
            UserViewModel UserModel = null;
            var path = "https://graph.facebook.com/me?access_token=" + accessToken;
            var client = new HttpClient();
            var uri = new Uri(path);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                UserModel = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(content);
            }
            return UserModel;
        }
        private UserViewModel TwitterUser(string accessToken, string accessTokenSecret)
        {
            UserViewModel UserModel = new UserViewModel();
            var service = new TwitterService(
                "MVRlIUFGPoBtwsoSsEmppRwch",
                "kPlQJwhg6MczpzLqRdbiF1WraPc1vaOlw8eujgxPMqGvFudJMK",
                accessToken,
                accessTokenSecret
            );
            var profile = service.GetUserProfile(new GetUserProfileOptions());
            UserModel.id =  profile.Id.ToString();
            UserModel.username = profile.Name ?? profile.ScreenName;            
            return UserModel;
        }
        //Допилить правильную десереализацию!
        private async Task<UserViewModel> VkUser(string accessToken, int user_id)
        {
            UserViewModel UserModel = null;
            string VkUserInfoRequest = string.Format("https://api.vk.com/method/users.get?user_id={0}&access_token={1}",
                                                            user_id, accessToken);
            HttpClient client = new HttpClient();
            Uri uri = new Uri(VkUserInfoRequest);
            var response = await client.GetAsync(uri);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                UserModel = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(content);
            }
            return UserModel;
        }
                                            #endregion
    }
}