using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VideoLib.Domian.Entities.AuthEntities
{
    public class SignInManagerIntPK : SignInManager<UserIntPK, int>
    {
        public SignInManagerIntPK(UserManagerIntPK userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        { }
            
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserIntPK user)
        {
            return user.GenerateUserIdentityAsync((UserManagerIntPK)UserManager);
        }

        public static SignInManagerIntPK Create(IdentityFactoryOptions<SignInManagerIntPK> options, IOwinContext context)
        {
            return new SignInManagerIntPK(context.GetUserManager<UserManagerIntPK>(), context.Authentication);
        }
    }
}
