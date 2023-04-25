using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Abstract
{
    public interface IRequestsRepository
    {
        #region Requests
        IEnumerable<Request> Requests { get; }
        void SaveRequest(Request request);

        IEnumerable<Call> Calls { get; }
        void SaveCall(Call call);

        IEnumerable<ComponentCall> ComponentCalls { get; }
        void SaveComponentCall(ComponentCall componentCall);

        IEnumerable<Status> Statuses { get; }
        #endregion

        #region Computers
        IEnumerable<Computer> Computers { get; }
        void SaveComputer(Computer computer);
        IEnumerable<Component> Components { get; }
        void SaveComponent(Component component);
        IEnumerable<ComponentType> ComponentTypes { get; }
        IEnumerable<ComponentTypeCategory> TypeCategories { get; }
        #endregion

        #region Users
        IEnumerable<AppUser> Users { get; }
        IEnumerable<Customer> Customers { get; }
        void SaveCustomer(Customer customer);
        IEnumerable<Consultant> Consultants { get; }

        #endregion
    }
}
