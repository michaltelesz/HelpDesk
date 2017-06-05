using Helpdesk.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    public class StatusesController : Controller
    {
        private IRequestsRepository repository;

        public StatusesController(IRequestsRepository requestRepository)
        {
            this.repository = requestRepository;
        }

        // GET: Status
        public ActionResult Index()
        {
            return View(repository.Statuses);
        }
    }
}