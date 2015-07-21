using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using VideoLib.Domian.Entities.AuthEntities;
using VideoLib.WebUI.Models.Auth;
using TweetSharp;
using Parse;
using Facebook;
using VideoLib.Domian.Concrete.ConfigClasses;
using VideoLib.Domian.Abstract;

namespace VideoLib.WebUI.Controllers
{
    public class AuthController : Controller
    {
                                            #region Managers
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        private MyUserManager UserManager
        {
            get { return  HttpContext.GetOwinContext().GetUserManager<MyUserManager>(); }
        }
        private SignInManagerIntPK SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManagerIntPK>(); }
        }
        private AuthServices ExternalAuthManaher
        {
            get { return HttpContext.GetOwinContext().Get<AuthServices>(); }
        }
        private IVideoLibRepository Repository
        {
            get { return HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }  
                                                #endregion

                                    #region Admin Authentification
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("~/Views/Auth/AdminLogin.cshtml", new AdminLoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]

        public ActionResult Login(AdminLoginViewModel model )
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Auth/AdminLogin.cshtml", model);
            }
            var user = UserManager.Find(model.UserName, model.Password);
            if(user != null)
            {
                var roles = UserManager.GetRoles(user.Id);
                if (roles.Contains("Admin"))
                {
                    var identity = new ClaimsIdentity(UserManager.GetClaims(user.Id), DefaultAuthenticationTypes.ApplicationCookie);

                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, identity);

                    return RedirectToAction("Main", "Admin");
                }
                ModelState.AddModelError("", "Вы не являетесь администратором.");
                return View(model);
            }            
            
            else
            {
                ModelState.AddModelError("", "Не правильное имя пользавателя или пароль.");
                return View("~/Views/Auth/AdminLogin.cshtml", model);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Auth");
        }

        // GET: /Auth/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("~/Views/Auth/RegisterAdmin.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult> Register(RegisterAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new MyUser {Id = "admin", UserName = model.Email, Email = model.Email, Name = model.Email };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    result = await UserManager.AddToRoleAsync(user.Id, "Admin");

                    return RedirectToAction("Main", "Admin");
                }
            }
                return View("~/Views/Auth/RegisterAdmin.cshtml", model);
        }
                                                #endregion
                                    #region Android Client Auth CallBacks

        [AllowAnonymous]

        //Post: Auth/EmailLoginCallBack
        [HttpPost]
        public async Task<ActionResult> EmailLoginCallBack(string object_id)
        {
            bool success = false;
            var parse_user = await ParseUser.Query.GetAsync(object_id);
            if (parse_user != null)
            {
                var my_user = await UserManager.FindByIdAsync(object_id);
                if (my_user != null)
                {
                    success = SetCurrentUser(AuthType.EmailPassword, my_user);
                    return Json(new { OK = success });  
                }
                success = await RegisterUser(
                    new RegisterUserModel
                    {
                        Email = (parse_user.Keys.Contains("email"))? parse_user.Get<string>("email") : null , 
                        Login = (parse_user.Keys.Contains("username"))? parse_user.Get<string>("username") : null,
                        Name = (parse_user.Keys.Contains("name")) ? parse_user.Get<string>("name") : null,
                        PhoneNumber = (parse_user.Keys.Contains("phone"))? parse_user.Get<string>("phone") : null,
                        Object_id = object_id
                    });
                return Json(new { OK = success });
            }

            return new JsonResult
            {
                Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMassege = string.Format("User with id {0} does not exist at Parse.com", object_id)
                    }
            };            
        }
        //Post: Auth/FacebookLoginCallBack
        [HttpPost]
        public async Task<ActionResult> FacebookLoginCallBack(string object_id, string access_token, string fb_key)
        {
            bool success = false;
             var fbLoginInfo = new UserLoginInfo("Facebook",fb_key);

             MyUser my_user = await UserManager.FindByIdAsync(object_id);
             if (my_user != null)
             {
                 success = SetCurrentUser(AuthType.Facebook, my_user, access_token);
                 return Json(new { OK = success });  
             }
             var fb_client = new FacebookClient(access_token);
             fb_client.AppId = ExternalAuthManaher.FacebookService.AppID;
             fb_client.AppSecret = ExternalAuthManaher.FacebookService.AppSecret;

             dynamic userInfo = await fb_client.GetTaskAsync("me", new { fields = "first_name, last_name, link, id, email" });
             if(userInfo != null)
             {
                 success = await RegisterUser(
                     new RegisterUserModel
                     {
                         RegisterType = AuthType.Facebook,
                         LoginProvider = fbLoginInfo,
                         UserClaims = new List<Claim> 
                            {
                                new Claim("FacebookAccessToken", access_token),
                                new Claim("FacebookPageLink", userInfo.link),
                                new Claim("FacebookId", userInfo.id)
                            },
                         Login = "FacebookUser",
                         Name = userInfo.first_name + " " + userInfo.last_name,
                         Object_id = object_id
                     });
                 return Json(new { OK = success });   
             }
            
             return Json(new { OK = false });          
        }

        //Post: Auth/FacebookLoginCallBack
        [HttpPost]
        public async Task<ActionResult> TwitterLoginCallBack(string object_id, string access_token, string access_token_secret, string tw_key)
        {
            bool success = false;
            var twLoginInfo = new UserLoginInfo("Twitter", tw_key);
            MyUser my_user = await UserManager.FindByIdAsync(object_id);
            if (my_user != null)
            {
                success = SetCurrentUser(AuthType.Twitter, my_user, access_token, access_token_secret);
                return Json(new { OK = success });  
            }
            var tw_service = new TwitterService
            (
                consumerKey: ExternalAuthManaher.TwitterService.AppID,
                consumerSecret: ExternalAuthManaher.TwitterService.AppSecret,
                token: access_token,
                tokenSecret: access_token_secret
            );
            var userInfo =  tw_service.GetUserProfile(new GetUserProfileOptions());
            if(userInfo != null)
            {
                
                success = await RegisterUser(
                     new RegisterUserModel
                     {
                         RegisterType = AuthType.Twitter,
                         LoginProvider = twLoginInfo,
                         UserClaims = new List<Claim> 
                            {
                                new Claim("TwitterAccessToken", access_token),
                                new Claim("TwitterAccessTokenSecret", access_token_secret),
                                new Claim("TwitterPageLink", ("https://twitter.com/" + userInfo.ScreenName)),
                                new Claim("TwitterId", userInfo.Id.ToString())
                            },
                         Name = userInfo.ScreenName,
                         Login = "TwitterUser",
                         Object_id = object_id
                     });
                return Json(new { OK = success }); 
            }
            return Json(new { OK = false });  
        }
        
        private async Task<bool> RegisterUser(RegisterUserModel model)
        {
            MyUser newUser = new MyUser
               {
                   Id = model.Object_id,
                   Email = model.Email,
                   UserName = model.Login,
                   Name = model.Name,
               };

            var createResult = await UserManager.CreateAsync(newUser);
            if(createResult.Succeeded)
            {
                if(model.RegisterType == AuthType.Facebook || model.RegisterType == AuthType.Twitter)
                {
                    foreach(Claim claim in model.UserClaims)
                    {
                        createResult = UserManager.AddClaim(newUser.Id, claim);
                    }
                    createResult = UserManager.AddLogin(newUser.Id, model.LoginProvider);
                }
                
                createResult = UserManager.AddToRole(newUser.Id, "User");
                return true;
            }
            return false;
        }

        private bool SetCurrentUser(AuthType authType ,MyUser user, string access_token = null, string access_token_secret = null)
        {           
            try
            {
                
                if (authType == AuthType.Facebook)
                    Repository.UpdateClaim("FacebookAccessToken", access_token, user.Id);
                if (authType == AuthType.Twitter)
                {
                    Repository.UpdateClaim("TwitterAccessToken", access_token, user.Id);
                    Repository.UpdateClaim("TwitterAccessTokenSecret", access_token_secret, user.Id);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //GET:Auth/GetBackgroundImage
        public JsonResult GetBackgroundImage()
        {
            return Json(new
            {
                url = Repository.AdditionData.FirstOrDefault(data => data.Type == "BackgroundUrl").Value
            },
                JsonRequestBehavior.AllowGet
            );
        }
                                                                      #endregion
    }
}