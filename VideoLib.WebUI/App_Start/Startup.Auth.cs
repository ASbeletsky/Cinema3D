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
using Parse;
using System.Configuration;
using VideoLib.Domian.Concrete.ConfigClasses;

namespace VideoLib.WebUI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppIdentityContext>(AppIdentityContext.Create);
            app.CreatePerOwinContext<MyUserManager>(MyUserManager.Create);
            app.CreatePerOwinContext<SignInManagerIntPK>(SignInManagerIntPK.Create);
            app.CreatePerOwinContext<IVideoLibRepository>(VideoLibRepository.GetRepository);
            app.CreatePerOwinContext<AuthServices>(AuthServices.Create);

            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            AuthServices services = AuthServices.Create();
            ParseClient.Initialize(services.ParseService.AppID, services.ParseService.DotNetKey);
        }
    }

    
}
