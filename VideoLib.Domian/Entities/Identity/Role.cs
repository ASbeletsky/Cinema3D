using Microsoft.AspNet.Identity.EntityFramework;

namespace VideoLib.Domian.Entities.AuthEntities
{
    public class MyRole : IdentityRole<string,MyUserRole>
    {
    }
}
