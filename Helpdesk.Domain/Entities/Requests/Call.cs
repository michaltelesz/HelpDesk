using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Requests
{
    public class Call
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; }
        //public User User { get; set; }
        public string Description { get; set; }
        public virtual Status Status { get; set; }
        public virtual Request Request { get; set; }

        public int StatusID { get; set; }
        public int RequestID { get; set; }

    }
}
