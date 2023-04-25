using Helpdesk.Domain.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.WebUI.Models.Requests
{
    public class RequestConsultantIndexViewModel
    {
        public IEnumerable<Request> Requests { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int? ConsultantID { get; set; }
        public string States { get; set; }

        public class Request
        {
            public int ID { get; set; }
            public string ReadableID { get; set; }
            public DateTime ReceivedDate { get; set; }
            public string CustomerName { get; set; }
            public string StatusLevel { get; set; }
            public string StatusDescription { get; set; }
        }
    }
}