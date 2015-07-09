using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;


namespace VideoLib.Entities.AuthEntities
{
    public class UserManagerIntPK : UserManager<UserIntPK, int>
    {
        public UserManagerIntPK(IUserStore<UserIntPK, int> store)
            : base(store)
        {
        }

        public static UserManagerIntPK Create(IdentityFactoryOptions<UserManagerIntPK> options, IOwinContext context)
        {
            UserManagerIntPK manager = new UserManagerIntPK(new UserStore<UserIntPK, RoleIntPK, int, UserLoginIntPK, UserRoleIntPK, UserClaimIntPK>(context.Get<MyIdentityContext>()));
            return manager;
        }
    }
}