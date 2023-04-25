using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;

namespace Helpdesk.WebUI.Models.Calls
{
    public class CallDetailsViewModel
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public string RequestReadableID { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public int? ComputerID { get; set; }
        public string ComputerName { get; set; }
        public Consultant CreatorConsultant { get; set; }
        public Customer CreatorCustomer { get; set; }
        public IEnumerable<ComponentCall> ComponentCalls { get; set; }
        public Status Status { get; set; }

        public class ComponentCall
        {
            public int ComponentID { get; set; }
            public string ComponentName { get; set; }
            public string ComponentSerialNo { get; set; }
            public string Description { get; set; }
        }
    }
}