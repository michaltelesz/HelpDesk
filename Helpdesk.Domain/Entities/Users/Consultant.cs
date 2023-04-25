using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Users
{
    public class Consultant
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual AppUser User { get; set; }

        public string UserID { get; set; }
    }
}
