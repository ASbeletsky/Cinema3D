using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace VideoLib.Domian.Entities.AuthEntities
{
    public class AppIdentityContext : IdentityDbContext<MyUser, MyRole, string, MyUserLogin,MyUserRole,MyUserClaim>
    {
        public AppIdentityContext()
            : base("IdentityDbContext")
        {
        }
        public static AppIdentityContext Create()
        {
            return new AppIdentityContext();
        }

         protected override void OnModelCreating(DbModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);


             // Map Entities to their tables.
             modelBuilder.Entity<MyUser>().ToTable("users");
             modelBuilder.Entity<MyRole>().ToTable("roles");
             modelBuilder.Entity<MyUserClaim>().ToTable("userclaims");
             modelBuilder.Entity<MyUserLogin>().ToTable("userlogins");
             modelBuilder.Entity<MyUserRole>().ToTable("userroles");
             // Set AutoIncrement-Properties
             //modelBuilder.Entity<MyUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             //modelBuilder.Entity<MyUserClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             //modelBuilder.Entity<MyRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             // Override some column mappings that do not match our default
             //modelBuilder.Entity<MyUser>().Property(r => r.PasswordHash).HasColumnName("Password");
             modelBuilder.Entity<MyUser>().Property(r => r.UserName).HasColumnName("Login");
             modelBuilder.Entity<MyUser>().Property(r => r.Name).HasColumnName("Name");
             
         }
         
    }
}

