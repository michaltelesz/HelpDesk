using Helpdesk.Domain.Entities;
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
        public DbSet<Request> Requests { get; set; }
        public DbSet<Computer> Computers { get; set; }
    }
}
