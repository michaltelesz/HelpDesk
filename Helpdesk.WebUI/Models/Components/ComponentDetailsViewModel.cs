using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Components
{
    public class ComponentDetailsViewModel
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public int ComputerID { get; set; }
        public string ComputerName { get; set; }
        public IEnumerable<Request> Requests { get; set; }

        public class Request
        {
            public int ID { get; set; }
            public string ReadableID { get; set; }
            public DateTime ReceivedDate { get; set; }
            public string StatusLevel { get; set; }
            public string StatusDescription { get; set; }
        }
    }
}