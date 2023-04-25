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
using System.Data.Entity.Validation;

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
                    dbEntry.Description = request.Description;
                    dbEntry.ComputerID = request.ComputerID;
                    dbEntry.StatusID = request.StatusID;
                    dbEntry.ResolvedDate = request.ResolvedDate;
                    dbEntry.CreatedByID = request.CreatedByID;
                    dbEntry.AssignedToID = request.AssignedToID;
                    dbEntry.ClosedByID = request.ClosedByID;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Computer> Computers
        {
            get { return context.Computers; }
        }
        public void SaveComputer(Computer computer)
        {
            if (computer.ID == 0)
            {
                context.Computers.Add(computer);
            }
            else
            {
                Computer dbEntry = context.Computers.Find(computer.ID);
                if (dbEntry != null)
                {
                    //dbEntry.Components = computer.Components;
                    dbEntry.Name = computer.Name;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Status> Statuses
        {
            get { return context.Statuses; }
        }

        public IEnumerable<Component> Components
        {
            get { return context.Components; }
        }
        public void SaveComponent(Component component)
        {
            if (component.ID == 0)
            {
                context.Components.Add(component);
            }
            else
            {
                Component dbEntry = context.Components.Find(component.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = component.Name;
                    dbEntry.SerialNo = component.SerialNo;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<ComponentType> ComponentTypes
        {
            get { return context.ComponentTypes; }
        }

        public IEnumerable<AppUser> Users
        {
            get { return context.Users; }
        }

        public IEnumerable<Customer> Customers
        {
            get { return context.Customers; }
        }

        public void SaveCustomer(Customer customer)
        {
            if (customer.ID == 0)
            {
                context.Customers.Add(customer);
            }
            else
            {
                Customer dbEntry = context.Customers.Find(customer.ID);
                if (dbEntry != null)
                {
                    context.Entry(dbEntry).Reference(c => c.User).Load();

                    dbEntry.Name = customer.Name;
                    dbEntry.Address = customer.Address;
                    dbEntry.PhoneNo = customer.PhoneNo;
                }
            }

            context.SaveChanges();
        }

        public IEnumerable<ComponentTypeCategory> TypeCategories
        {
            get { return context.TypeCategories; }
        }

        public IEnumerable<Call> Calls
        {
            get { return context.Calls; }
        }

        public void SaveCall(Call call)
        {
            if (call.ID == 0)
            {
                context.Calls.Add(call);
            }
            else
            {
                Call dbEntry = context.Calls.Find(call.ID);
                if (dbEntry != null)
                {
                    dbEntry.Description = call.Description;
                    dbEntry.StatusID = call.StatusID;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<ComponentCall> ComponentCalls
        {
            get { return context.ComponentCalls; }
        }

        public void SaveComponentCall(ComponentCall componentCall)
        {
            if (componentCall.ID == 0)
            {
                context.ComponentCalls.Add(componentCall);
            }
            else
            {
                ComponentCall dbEntry = context.ComponentCalls.Find(componentCall.ID);
                if (dbEntry != null)
                {
                    dbEntry.Description = componentCall.Description;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Consultant> Consultants
        {
            get { return context.Consultants; }
        }
    }
}
