using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VideoLib.Entities.AuthEntities
{
    public class MyIdentityContext : IdentityDbContext<UserIntPK, RoleIntPK, int, UserLoginIntPK, UserRoleIntPK, UserClaimIntPK>
    {
        public MyIdentityContext() : base("IdentityDbContext")
        {
        }
        public static MyIdentityContext Create()
        {
            return new MyIdentityContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Map Entities to their tables.
            modelBuilder.Entity<UserIntPK>().ToTable("user");
            modelBuilder.Entity<RoleIntPK>().ToTable("roles");
            modelBuilder.Entity<UserClaimIntPK>().ToTable("userclaims");
            modelBuilder.Entity<UserLoginIntPK>().ToTable("userlogins");
            modelBuilder.Entity<UserRoleIntPK>().ToTable("userrole");
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
