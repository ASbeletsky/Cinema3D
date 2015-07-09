using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace VideoLib.Domian.Entities.AuthEntities
{
    public class AppIdentityContext : IdentityDbContext<UserIntPK,RoleIntPK,int,UserLoginIntPK,UserRoleIntPK,UserClaimIntPK>
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
             modelBuilder.Entity<UserIntPK>().ToTable("users");
             modelBuilder.Entity<RoleIntPK>().ToTable("roles");
             modelBuilder.Entity<UserClaimIntPK>().ToTable("userclaims");
             modelBuilder.Entity<UserLoginIntPK>().ToTable("userlogins");
             modelBuilder.Entity<UserRoleIntPK>().ToTable("userroles");
             // Set AutoIncrement-Properties
             modelBuilder.Entity<UserIntPK>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             modelBuilder.Entity<UserClaimIntPK>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             modelBuilder.Entity<RoleIntPK>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             // Override some column mappings that do not match our default
             //modelBuilder.Entity<MyUser>().Property(r => r.PasswordHash).HasColumnName("Password");
             //modelBuilder.Entity<UserIntPK>().Property(r => r.UserName).HasColumnName("Login");
             //modelBuilder.Entity<UserIntPK>().Property(r => r.Name).HasColumnName("Name");
        
         }
         
    }
}

