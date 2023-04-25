using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpdesk.Domain.Entities.Users;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Helpdesk.WebUI.Models.Account
{
    public class UserDetailsViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public Customer Customer { get; set; }
        public Consultant Consultant { get; set; }
    }
}