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

namespace Helpdesk.WebUI.Controllers
{
    public class RequestsController : Controller
    {
        private IRequestsRepository repository;

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
        public ActionResult Details(int? id)
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

        // GET: Requests/Create/1/1
        [Route("Requests/Create/{customer:int}/{computer:int}", Name ="Create_New")]
        public ActionResult Create(int customer, int computer)
        {
            Computer computerEntity = repository.Computers.SingleOrDefault(c => c.ID == computer && c.OwnerID == customer);
            if (computerEntity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View("Create", computerEntity);
        }

        // GET: Requests/Create/1
        [Route("Requests/Create/{customer:int}")]
        public ActionResult Create(int customer)
        {
            ICollection<Computer> computers = repository.Customers.Single(c => c.ID == customer).Computers;
            return View("Create_Computers", computers);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            IEnumerable<Customer> customers = repository.Customers;
            return View("Create_Customers", customers);
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description")] Request request)
        {
            if (ModelState.IsValid)
            {
                //db.Requests.Add(request);
                //db.SaveChanges();
                request.ReceivedDate = DateTime.Now;
                request.ComputerID = 1;
                request.StatusID = repository.Statuses.Single(s => s.Description == "Nowe").ID;
                request.ReadableID = Guid.NewGuid().ToString().Substring(0, 12);
                repository.SaveRequest(request);
                return RedirectToAction("Index");
            }

            return View(request);
        }
    }
}
