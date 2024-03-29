﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Domain.Entities.Requests
{
    public class Status
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
