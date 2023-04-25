using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext<AppUser>
    {
        public EFDbContext() : base("EFDbContext")
        {
        }

        static EFDbContext()
        {
            //Database.SetInitializer<EFDbContext>(new IdentityDbInit());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, Helpdesk.Domain.Migrations.Configuration>());
        }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }

        public class IdentityDbInit : MigrateDatabaseToLatestVersion<EFDbContext, DbMigrationsConfiguration<EFDbContext>>
        {
            public override void InitializeDatabase(EFDbContext context)
            {
                base.InitializeDatabase(context);
            }
        }

        #region Requests
        public DbSet<Request> Requests { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<ComponentCall> ComponentCalls { get; set; }
        public DbSet<Status> Statuses { get; set; }
        #endregion

        #region Computers
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<ComponentTypeCategory> TypeCategories { get; set; }
        #endregion

        #region Users
        //public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().ToTable("Statuses");

            modelBuilder.Entity<Call>().HasRequired(c => c.Status).WithMany(s => s.Calls).WillCascadeOnDelete(false);
            modelBuilder.Entity<Call>().HasRequired(c => c.Request).WithMany(r => r.Calls).WillCascadeOnDelete(true);

            modelBuilder.Entity<Request>().HasRequired(r => r.Status).WithMany(s => s.Requests).WillCascadeOnDelete(false);
            modelBuilder.Entity<Request>().HasRequired(r => r.CreatedBy).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Consultant>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(true);
            //modelBuilder.Entity<Request>().HasMany(r => r.Tags).WithMany(t => t.Requests).Map(k => k.MapLeftKey("RequestID").MapRightKey("RequestTagID").ToTable("RequestsTags"));

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}
