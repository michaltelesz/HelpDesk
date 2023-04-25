using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Computers
{
    public class ComputerDetailsViewModel
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public IEnumerable<ComponentCategory> ComponentsByCategories { get; set; }

        public class ComponentCategory
        {
            public string Name { get; set; }
            public IEnumerable<Component> Components { get; set; }
        }

        public class Component
        {
            public int ID { get; set; }
            public string TypeName { get; set; }
            public string Name { get; set; }
            public string SerialNo { get; set; }
        }
    }
}