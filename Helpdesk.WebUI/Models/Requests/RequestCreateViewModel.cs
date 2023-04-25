using Helpdesk.Domain.Entities.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestCreateViewModel
    {
        public int ComputerID { get; set; }
        public Computer Computer { get; set; }
        public string Description { get; set; }
    }
}