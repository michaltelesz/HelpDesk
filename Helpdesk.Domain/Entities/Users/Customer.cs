using Helpdesk.Domain.Entities.Computers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Users
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public string PhoneNo { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }

        public string UserID { get; set; }
    }
}
