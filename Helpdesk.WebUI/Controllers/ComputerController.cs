using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    public class ComputerController : Controller
    {
        private IRequestRepository repository;

        public ComputerController(IRequestRepository requestRepository)
        {
            this.repository = requestRepository;
        }

        public ViewResult List()
        {
            return View(repository.Computers);
        }
    }
}