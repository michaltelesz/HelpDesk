using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.WebUI.Models;
using Helpdesk.WebUI.Models.Calls;
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
    public class CallsController : Controller
    {
        IRequestsRepository _repository;

        public CallsController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: Calls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Call call = _repository.Calls.Single(r => r.ID == id);
            if (call == null)
            {
                return HttpNotFound();
            }

            Consultant creatorConsultant = _repository.Consultants.SingleOrDefault(c => c.UserID == call.UserID);
            Customer creatorCustomer = call.UserID == null ? null : _repository.Customers.SingleOrDefault(c => c.UserID == call.UserID);

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && ((creatorCustomer != null && currentCustomer.ID == creatorCustomer.ID) || (call.Request.Computer != null && call.Request.Computer.OwnerID == currentCustomer.ID)))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            CallDetailsViewModel viewModel = new CallDetailsViewModel()
            {
                ID = call.ID,
                RequestID = call.RequestID,
                RequestReadableID = call.Request.ReadableID,
                Status = call.Status,
                Date = call.Date,
                Description = call.Description,
                ComputerID = call.Request.ComputerID,
                ComputerName = call.Request.Computer?.Name,
                CreatorConsultant = creatorConsultant,
                CreatorCustomer = creatorCustomer,
                ComponentCalls = call.ComponentCalls.Select(cc => new CallDetailsViewModel.ComponentCall()
                {
                    ComponentID = cc.ComponentID,
                    ComponentName = cc.Component.Name,
                    ComponentSerialNo = cc.Component.SerialNo,
                    Description = cc.Description
                }),

            };
            return View(viewModel);
        }

        // GET: Calls/Create/1
        [Route("Calls/Create/{requestID:int}")]
        public ActionResult Create(int requestID)
        {
            Request requestEntity = _repository.Requests.SingleOrDefault(c => c.ID == requestID);
            if (requestEntity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && (currentUserID == requestEntity.CreatedByID))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<CallCreateViewModel.Component> components = new List<CallCreateViewModel.Component>();
            if (requestEntity.Computer != null && requestEntity.Computer.Components != null)
            {
                components = requestEntity.Computer.Components.Select(c => new CallCreateViewModel.Component
                {
                    ID = c.ID,
                    Name = c.Name,
                    DataGroup = c.Type.Name
                });
            }

            int[] allowedStatusesIDs = { 2, 4 };
            CallCreateViewModel newCall = new CallCreateViewModel()
            {
                RequestID = requestID,
                Statuses = _repository.Statuses.Where(s => allowedStatusesIDs.Contains(s.ID)),
                Components = components
            };
            return View(newCall);
        }

        // POST: Calls/Create/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Calls/Create/{requestID:int}")]
        public ActionResult Create(CallCreateViewModel model, int requestID)
        {
            if (ModelState.IsValid)
            {
                Request request = _repository.Requests.SingleOrDefault(r => r.ID == requestID);
                if (request == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string currentUserID = User.Identity.GetUserId();
                Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
                if (!(User.IsInRole("Consultant") || (currentCustomer != null && (currentUserID == request.CreatedByID))))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                Call call = new Call()
                {
                    RequestID = requestID,
                    Date = DateTime.Now,
                    UserID = currentUserID,
                    Description = model.Description,
                    StatusID = model.StatusID
                };
                _repository.SaveCall(call);

                request.StatusID = model.StatusID;
                _repository.SaveRequest(request);

                if (model.ComponentID != null)
                {
                    for (int i = 0; i < model.ComponentID.Length; i++)
                    {
                        ComponentCall componentCall = new ComponentCall()
                        {
                            CallID = call.ID,
                            ComponentID = model.ComponentID[i],
                            Description = model.ComponentDescription[i]
                        };
                        _repository.SaveComponentCall(componentCall);
                    }
                }

                return RedirectToAction("Details", "Requests", new { id = requestID });
            }
            return View(model);
        }
    }
}