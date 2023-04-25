using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.UserAdmin
{
    public class UserRoleEditViewModel
    {
        public AppUser User { get; set; }

        public IEnumerable<AppRole> CurrentRoles { get; set; }
        public IEnumerable<AppRole> RolesToAdd { get; set; }
    }
}