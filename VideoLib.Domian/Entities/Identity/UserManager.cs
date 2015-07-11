using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using VideoLib.Domian.Concrete;


namespace VideoLib.Domian.Entities.AuthEntities
{
    public class UserManagerIntPK : UserManager<UserIntPK, int>
    {
        public UserManagerIntPK(IUserStore<UserIntPK, int> store)
            : base(store)
        {
        }
        public async Task<UserIntPK> FindByParseIdAsync(string object_id)
        {
            AppIdentityContext con = new AppIdentityContext();
            return await con.Users.FirstOrDefaultAsync(x => x.Parse_Id == object_id);
        }

        
            
        
        public static UserManagerIntPK Create(IdentityFactoryOptions<UserManagerIntPK> options, IOwinContext context)
        {
            UserManagerIntPK manager = new UserManagerIntPK(new UserStore<UserIntPK, RoleIntPK, int, UserLoginIntPK, UserRoleIntPK, UserClaimIntPK>(context.Get<AppIdentityContext>()));
            return manager;
        }
    }
}