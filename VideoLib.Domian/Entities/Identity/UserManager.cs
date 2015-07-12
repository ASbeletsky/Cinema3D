using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;
using System.Threading.Tasks;
using VideoLib.Domian.Concrete;


namespace VideoLib.Domian.Entities.AuthEntities
{
    public class MyUserManager : UserManager<MyUser, string>
    {
        public MyUserManager(IUserStore<MyUser, string> store)
            : base(store)
        {
        }

        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options, IOwinContext context)
        {
            MyUserManager manager = new MyUserManager(new UserStore<MyUser, MyRole, string, MyUserLogin, MyUserRole, MyUserClaim>(context.Get<AppIdentityContext>()));
            return manager;
        }

    }
}