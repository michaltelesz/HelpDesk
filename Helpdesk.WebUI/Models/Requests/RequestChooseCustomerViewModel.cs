using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpdesk.Domain.Entities.Users;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestChooseCustomerViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}