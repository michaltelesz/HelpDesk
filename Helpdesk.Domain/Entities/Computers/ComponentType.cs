﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Computers
{
    public class ComponentType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ComponentTypeCategory Category { get; set; }
    }
}
