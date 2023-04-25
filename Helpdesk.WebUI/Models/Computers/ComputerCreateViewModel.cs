using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Computers
{
    public class ComputerCreateViewModel
    {
        public string CustomerName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNo { get; set; }
    }
}