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
        public ComponentType Type { get; set; }
    }
}
