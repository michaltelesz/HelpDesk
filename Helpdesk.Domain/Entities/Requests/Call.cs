using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Requests
{
    public class Call
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public virtual AppUser User { get; set; }
        public string Description { get; set; }
        public virtual Status Status { get; set; }
        public virtual Request Request { get; set; }

        public virtual ICollection<ComponentCall> ComponentCalls { get; set; }

        public string UserID { get; set; }
        public int StatusID { get; set; }
        public int RequestID { get; set; }

    }
}
