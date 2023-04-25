using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Customers
{
    public class CustomerEditViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
    }
}