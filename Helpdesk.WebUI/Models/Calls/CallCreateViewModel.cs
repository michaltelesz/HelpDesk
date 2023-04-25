using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Calls
{
    public class CallCreateViewModel
    {
        public int RequestID { get; set; }

        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Component> Components { get; set; }

        public int StatusID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int[] ComponentID { get; set; }
        [DataType(DataType.MultilineText)]
        public string[] ComponentDescription { get; set; }

        public class Component
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string DataGroup { get; set; }
        }
    }
}