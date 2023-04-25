using Helpdesk.Domain.Entities.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestChooseComputerViewModel
    {
        public int CustomerID { get; internal set; }
        public string CustomerName { get;set; }
        public IEnumerable<Computer> Computers { get; set; }
    }
}