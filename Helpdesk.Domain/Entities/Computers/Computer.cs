﻿using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Domain.Entities.Computers
{
    public class Computer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public bool Temporary { get; set; }
        public virtual Customer Owner { get; set; }
        public virtual ICollection<Component> Components { get; set; }

        public int OwnerID { get; set; }
    }
}