using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Domain.Entities.Requests
{
    public class Request
    {
        public int ID { get; set; }
        public string ReadableID { get; set; }
        public string Description { get; set; }
        public Computer Computer { get; set; }

        public DateTime? ReceivedDate { get; set; }
        //public User ReceiverUser { get; set; }

        public DateTime? ResolvedDate { get; set; }
        //public User ResolverUser { get; set; }

        public Status Status { get; set; }
    }
}