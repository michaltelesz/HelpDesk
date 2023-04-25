using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Computers
{
    public class ComputerCreateTempViewModel
    {
        [Display(Name="Nazwa")]
        public string ComponentName { get; set; }
        [Display(Name = "Numer seryjny")]
        public string ComponentSerialNo { get; set; }
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
        [Display(Name = "Typ")]
        public int ComponentTypeID { get; set; }

        public class ComponentType
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string DataGroup { get; set; }
        }
    }
}