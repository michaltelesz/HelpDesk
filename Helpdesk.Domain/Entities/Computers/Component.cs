using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Computers
{
    public class Component
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public virtual ComponentType Type { get; set; }
        public virtual Computer Computer { get; set; }

        public int TypeID { get; set; }
        public int ComputerID { get; set; }
    }
}
