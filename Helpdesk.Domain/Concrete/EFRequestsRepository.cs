using Helpdesk.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Users;

namespace Helpdesk.Domain.Concrete
{
    public class EFRequestsRepository : IRequestsRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Request> Requests
        {
            get { return context.Requests; }
        }

        public void SaveRequest(Request request)
        {
            if (request.ID == 0)
            {
                context.Requests.Add(request);
            }
            else
            {
                Request dbEntry = context.Requests.Find(request.ID);
                if (dbEntry != null)
                {
                    //dbEntry.Description = request.Description;
                    //...
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Computer> Computers
        {
            get { return context.Computers; }
        }

        public IEnumerable<Status> Statuses
        {
            get { return context.Statuses; }
        }

        public IEnumerable<Component> Components
        {
            get { return context.Components; }
        }

        public IEnumerable<ComponentType> ComponentTypes
        {
            get { return context.ComponentTypes; }
        }

        public IEnumerable<Customer> Customers
        {
            get { return context.Customers; }
        }

        public IEnumerable<ComponentTypeCategory> TypeCategories
        {
            get { return context.TypeCategories; }
        }

        public IEnumerable<Call> Calls
        {
            get { return context.Calls; }
        }
    }
}
