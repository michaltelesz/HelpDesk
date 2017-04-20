using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Domain.Entities
{
    public class Request
    {
        public int ID { get; set; }
        public string Descrption { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ResolvedDate { get; set; }
    }
}