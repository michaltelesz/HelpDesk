using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Customers
{
    public class CustomerDetailsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public IEnumerable<Computer> Computers { get; set; }
        public IEnumerable<RequestsStatus> RequestsStatuses { get; set; }

        public class Computer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string SerialNo { get; set; }
            public bool Temporary { get; set; }
        }

        public class RequestsStatus
        {
            public string Level { get; set; }
            public string Description { get; set; }
            public int Count { get; set; }
        }
    }
}