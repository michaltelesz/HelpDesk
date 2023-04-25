using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Helpdesk.Domain.Entities.Requests
{
    public class Request
    {
        public int ID { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(13)]
        public string ReadableID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual Computer Computer { get; set; }

        public DateTime ReceivedDate { get; set; }
        public virtual AppUser CreatedBy { get; set; }

        public virtual Consultant AssignedTo { get; set; }

        public DateTime? ResolvedDate { get; set; }
        public virtual Consultant ClosedBy { get; set; }

        public virtual Status Status { get; set; }

        public int? ComputerID { get; set; }
        public int StatusID { get; set; }
        public string CreatedByID { get; set; }
        public int? ClosedByID { get; set; }
        public int? AssignedToID { get; set; }

        public virtual ICollection<Call> Calls { get; set; }
    }
}