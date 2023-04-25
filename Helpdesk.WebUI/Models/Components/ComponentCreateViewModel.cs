using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Components
{
    public class ComponentCreateViewModel
    {
        public string ComputerName { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Numer seryjny")]
        public string SerialNo { get; set; }
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
        [Display(Name = "Typ")]
        public int TypeID { get; set; }

        public class ComponentType
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string DataGroup { get; set; }
        }
    }
}