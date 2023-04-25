using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestDetailsViewModel
    {
        public int ID { get; set; }
        public string ReadableID { get; set; }
        public string Description { get; set; }
        public int? ComputerID { get; set; }
        public string ComputerName { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }

        public DateTime ReceivedDate { get; set; }
        //public AppUser CreatedBy { get; set; }
        public Consultant CreatorConsultant { get; set; }
        public Customer CreatorCustomer { get; set; }

        public Consultant AssignedTo { get; set; }

        public DateTime? ResolvedDate { get; set; }
        public Consultant ClosedBy { get; set; }

        public Status Status { get; set; }
        public bool IsOpened { get; set; }
        public bool IsClosed { get; set; }

        public IEnumerable<Call> Calls { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public bool CanAssign { get; set; }

        public class Call
        {
            public int ID { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public string StatusLevel { get; set; }
            public string StatusDescription { get; set; }
            public bool HasComponents { get; set; }
        }
    }
}