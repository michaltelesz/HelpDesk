using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Customers
{
    public class CustomerIndexViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public class Customer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ComputersCount { get; set; }
            public int RequestsCount { get; set; }
        }
    }
}