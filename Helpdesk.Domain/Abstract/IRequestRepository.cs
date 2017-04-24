using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Abstract
{
    public interface IRequestRepository
    {
        IEnumerable<Request> Requests { get; }
        IEnumerable<Computer> Computers { get; }
    }
}
