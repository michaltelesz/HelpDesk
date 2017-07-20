using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Domain.Concrete;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Abstract;
using System.Text.RegularExpressions;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.WebUI.Models;

namespace Helpdesk.WebUI.Controllers
{
    public class RequestsController : Controller
    {
        private IRequestsRepository repository;
        public int PageSize = 5;

        public RequestsController(IRequestsRepository requestsRepository)
        {
            repository = requestsRepository;
        }
        // GET: Requests
        public ActionResult Index()
        {
            return View(repository.Requests);
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id, int page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = repository.Requests.Single(r => r.ID == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            RequestDetailsViewModel requestViewModel = new RequestDetailsViewModel()
            {
                ID = request.ID,
                ReadableID = request.ReadableID,
                Description = request.Description,
                Computer = request.Computer,
                ReceivedDate = request.ReceivedDate,
                ResolvedDate = request.ResolvedDate,
                Status = request.Status,
                Calls = request.Calls.OrderByDescending(c => c.Date).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Calls.Where(c => c.RequestID == id).Count()
                }
            };
            return View(requestViewModel);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            IEnumerable<Customer> customers = repository.Customers;
            return View("ChooseCustomer", customers);
        }

        // GET: Requests/Create/ChooseComputer/5
        [Route("Requests/Create/ChooseComputer/{customer:int}")]
        public ActionResult ChooseComputer(int customer)
        {
            ICollection<Computer> computers = repository.Customers.Single(c => c.ID == customer).Computers;
            return View("ChooseComputer", computers);
        }

        // GET: Requests/Create/5
        [Route("Requests/Create/{computer:int}")]
        public ActionResult Create(int computer)
        {
            Computer computerEntity = repository.Computers.SingleOrDefault(c => c.ID == computer);
            if (computerEntity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request newRequest = new Request
            {
                ComputerID = computerEntity.ID
            };
            return View("Create", newRequest);
        }



        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Requests/Create/{computer:int}")]
        public ActionResult Create([Bind(Include = "Description, ComputerID")] Request request, int computer)
        {
            if (ModelState.IsValid)
            {
                //db.Requests.Add(request);
                //db.SaveChanges();
                var statusID = repository.Statuses.Single(s => s.Description == "Nowe").ID;
                request.ReceivedDate = DateTime.Now;
                request.StatusID = statusID;
                request.ReadableID = Guid.NewGuid().ToString().Substring(0, 12);
                repository.SaveRequest(request);

                Call call = new Call()
                {
                    RequestID = request.ID,
                    Date = DateTime.Now,
                    Description = "--- REQUEST CREATED ---",
                    StatusID = statusID
                };
                repository.SaveCall(call);
                return RedirectToAction("Details", new { id = request.ID });
            }

            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = repository.Requests.Single(r => r.ID == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Description")] Request request)
        {
            if (ModelState.IsValid)
            {
                repository.SaveRequest(request);
                TempData["message"] = string.Format("Zapisano {0} ", request.ReadableID);
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Close/5
        public ActionResult Close(int id)
        {
            int statusID = repository.Statuses.Single(s => s.Description == "Zakończone").ID;
            Call newCall = new Call
            {
                Date = DateTime.Now,
                StatusID = statusID,
                Description = "--- RESOLVED ---",
                RequestID = id
            };
            repository.SaveCall(newCall);

            Request request = repository.Requests.Single(r => r.ID == id);
            request.StatusID = statusID;
            repository.SaveRequest(request);

            return RedirectToAction("Details", new { id = request.ID });
        }

        // GET: Requests/Reopen/5
        public ActionResult Reopen(int id)
        {
            return View();
        }
    }
}
