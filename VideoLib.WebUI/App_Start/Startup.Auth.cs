using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Google;
using Owin;
using VideoLib.Domian.Entities.AuthEntities;
using Microsoft.Owin.Security.Facebook;
using Duke.Owin.VkontakteMiddleware;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using VideoLib.Domian.Concrete;


namespace VideoLib.WebUI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppIdentityContext>(AppIdentityContext.Create);
            app.CreatePerOwinContext<UserManagerIntPK>(UserManagerIntPK.Create);
            app.CreatePerOwinContext<SignInManagerIntPK>(SignInManagerIntPK.Create);
            app.CreatePerOwinContext<IVideoLibRepository>(VideoLibRepository.GetRepository);

            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwitterAuthentication(
               consumerKey: "MVRlIUFGPoBtwsoSsEmppRwch",
               consumerSecret: "kPlQJwhg6MczpzLqRdbiF1WraPc1vaOlw8eujgxPMqGvFudJMK"
            );

            var FacebookOptions = new FacebookAuthenticationOptions()
            {
               AppId = "100431063637918",
               AppSecret = "190c55c15d09a98762e46cf30bd4ba2c"
               
            };
            FacebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(FacebookOptions);

            var VkOptions = new VkAuthenticationOptions()
            {
                AppId = "4980565",
                AppSecret = "ygP2LPMJB5SjxEJX3Cly"                
            };
            VkOptions.Scope = "Email";
            app.UseVkontakteAuthentication(VkOptions);
        }
    }

    
}
