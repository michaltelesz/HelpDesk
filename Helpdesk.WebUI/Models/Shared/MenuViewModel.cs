using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Shared
{
    public class MenuViewModel
    {
        public bool IsAuthenticated { get; set; }
        public bool IsCustomer { get; set; }
        public int? CustomerId { get; set; }

        public bool IsConsultant { get; set; }

        public bool IsAdmin { get; set; }
    }
}