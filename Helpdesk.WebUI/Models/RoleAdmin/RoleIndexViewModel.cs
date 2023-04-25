using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.RoleAdmin
{
    public class RoleIndexViewModel
    {
        public IEnumerable<Role> Roles { get; set; }

        public class Role
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int UsersCount { get; set; }
        }
    }
}