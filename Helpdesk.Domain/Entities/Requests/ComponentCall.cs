using Helpdesk.Domain.Entities.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Requests
{
    public class ComponentCall
    {
        public int ID { get; set; }
        public virtual Component Component { get; set; }
        public virtual Call Call { get; set; }
        public string Description { get; set; }

        public int ComponentID { get; set; }
        public int CallID { get; set; }
    }
}
