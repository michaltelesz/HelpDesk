using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        #region Requests
        public DbSet<Request> Requests { get; set; }
        public DbSet<Call> Calls { get; set; }
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
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Request>().HasMany(r => r.Tags).WithMany(t => t.Requests).Map(k => k.MapLeftKey("RequestID").MapRightKey("RequestTagID").ToTable("RequestsTags"));
        }
    }
}
