using Helpdesk.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class StatusesController : Controller
    {
        private IRequestsRepository _repository;

        public StatusesController(IRequestsRepository requestRepository)
        {
            this._repository = requestRepository;
        }

        // GET: Status
        public ActionResult Index()
        {
            return View(_repository.Statuses);
        }
    }
}