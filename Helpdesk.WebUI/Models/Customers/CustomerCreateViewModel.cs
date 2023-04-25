using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Customers
{
    public class CustomerCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public string PhoneNo { get; set; }
    }
}