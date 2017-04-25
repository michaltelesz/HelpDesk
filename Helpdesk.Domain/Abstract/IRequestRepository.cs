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
    public interface IRequestRepository
    {
        #region Requests
        IEnumerable<Request> Requests { get;}
        IEnumerable<Call> Calls { get; }
        IEnumerable<Status> Statuses { get;  }
        #endregion

        #region Computers
        IEnumerable<Computer> Computers { get;  }
        IEnumerable<Component> Components { get; }
        IEnumerable<ComponentType> ComponentTypes { get; }
        IEnumerable<ComponentTypeCategory> TypeCategories { get; }
        #endregion

        #region Users
        //IEnumerable<User> Users { get; set; }
        IEnumerable<Customer> Customers { get; }
        #endregion
    }
}
