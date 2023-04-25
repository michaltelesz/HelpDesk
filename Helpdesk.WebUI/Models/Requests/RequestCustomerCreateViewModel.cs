using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestCustomerCreateViewModel
    {
        public string CustomerName { get; set; }

        public IEnumerable<Computer> Computers { get; set; }

        [Display(Name = "Komentarz")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Komputer")]
        public int? ComputerID { get; set; }

        public class Computer
        {
            public int? ID { get; set; }
            public string Name { get; set; }
        }
    }
}