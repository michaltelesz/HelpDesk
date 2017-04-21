using Helpdesk.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Domain.Concrete
{
    public class EFRequestRepository : IRequestRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Request> Requests
        {
            get { return context.Requests; }
        }

        public IEnumerable<Computer> Computers
        {
            get { return context.Computers; }
        }
    }
}
