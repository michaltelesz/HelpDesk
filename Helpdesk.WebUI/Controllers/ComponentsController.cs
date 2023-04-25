using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.WebUI.Models.Components;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class ComponentsController : Controller
    {
        private IRequestsRepository _repository;

        public ComponentsController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: Components/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Component component = _repository.Components.Single(c => c.ID == id);

            if (component == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && (component.Computer != null && currentCustomer.ID == component.Computer.OwnerID))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<Request> entityRequests = component.ComponentCalls.Select(cc => cc.Call.Request).Distinct();
            IEnumerable<ComponentDetailsViewModel.Request> requests = entityRequests.Select(er => new ComponentDetailsViewModel.Request()
            {
                ID = er.ID,
                ReadableID = er.ReadableID,
                ReceivedDate = er.ReceivedDate,
                StatusLevel = er.Status.Level,
                StatusDescription = er.Status.Description
            });
            ComponentDetailsViewModel viewModel = new ComponentDetailsViewModel()
            {
                ID = component.ID,
                Name = component.Name,
                SerialNo = component.SerialNo,
                ComputerName = component.Computer.Name,
                ComputerID = component.ComputerID,
                TypeName = component.Type.Name,
                Requests = requests
            };
            return View(viewModel);
        }

        // GET: Components/Create/5
        [Route("Components/Create/{computerID:int}")]
        public ActionResult Create(int? computerID)
        {
            if (computerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Computer computer = _repository.Computers.SingleOrDefault(c => c.ID == computerID);
            if (computer == null)
            {
                return HttpNotFound();
            }

            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<ComponentCreateViewModel.ComponentType> componentTypes = _repository.ComponentTypes.Select(c => new ComponentCreateViewModel.ComponentType()
            {
                ID = c.ID,
                Name = c.Name,
                DataGroup = c.Category.Name
            });
            ComponentCreateViewModel viewModel = new ComponentCreateViewModel()
            {
                ComputerName = computer.Name,
                ComponentTypes = componentTypes
            };
            return View("Create", viewModel);
        }

        // POST: Components/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Components/Create/{computerID:int}")]
        public ActionResult Create([Bind(Include = "Name, SerialNo, TypeID")] ComponentCreateViewModel model, int computerID)
        {
            Computer computer = _repository.Computers.SingleOrDefault(c => c.ID == computerID);
            if (ModelState.IsValid)
            {
                if (computer == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!(User.IsInRole("Consultant")))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                Component component = new Component()
                {
                    ComputerID = computer.ID,
                    Name = model.Name,
                    SerialNo = model.SerialNo,
                    TypeID = model.TypeID
                };
                _repository.SaveComponent(component);

                return RedirectToAction("Details", "Components", new { id = component.ID });
            }
            IEnumerable<ComponentCreateViewModel.ComponentType> componentTypes = _repository.ComponentTypes.Select(c => new ComponentCreateViewModel.ComponentType()
            {
                ID = c.ID,
                Name = c.Name,
                DataGroup = c.Category.Name
            });
            model.ComputerName = computer.Name;
            model.ComponentTypes = componentTypes;
            return View(model);
        }
    }
}