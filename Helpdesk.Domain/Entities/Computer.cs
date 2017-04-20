using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Domain.Entities
{
    public class Computer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public bool Temporary { get; set; }

    }
}