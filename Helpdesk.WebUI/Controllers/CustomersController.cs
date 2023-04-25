using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Entities.Requests;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.WebUI.Infrastructure;
using Helpdesk.WebUI.Models.Customers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        IRequestsRepository _repository;
        private int _customersPageSize = 5;

        public CustomersController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: Customers
        public ActionResult Index(int page = 1)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<CustomerIndexViewModel.Customer> customers = _repository.Customers.Select(c => new CustomerIndexViewModel.Customer
            {
                ID = c.ID,
                Name = c.Name,
                ComputersCount = c.Computers.Count,
                RequestsCount = _repository.Requests.Where(r => (r.Computer?.Owner != null && r.Computer.OwnerID == c.ID) || (r.CreatedByID == c.UserID)).Count()
            });
            CustomerIndexViewModel viewModel = new CustomerIndexViewModel()
            {
                Customers = customers.OrderBy(c => c.Name).Skip((page - 1) * _customersPageSize).Take(_customersPageSize),
                PagingInfo = new Models.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _customersPageSize,
                    TotalItems = customers.Count()
                }
            };
            return View(viewModel);
        }

        // GET: Customers/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = _repository.Customers.Single(c => c.ID == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && currentCustomer.ID == customer.ID)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<Request> requests = _repository.Requests.Where(r => (r.Computer?.Owner != null && r.Computer.OwnerID == customer.ID) || (r.CreatedByID == customer.UserID));

            CustomerDetailsViewModel viewModel = new CustomerDetailsViewModel()
            {
                ID = customer.ID,
                Name = customer.Name,
                Address = customer.Address,
                PhoneNo = customer.PhoneNo,
                Computers = customer.Computers.Select(c => new CustomerDetailsViewModel.Computer()
                {
                    ID = c.ID,
                    Name = c.Name,
                    SerialNo = c.SerialNo,
                    Temporary = c.Temporary
                }),
                RequestsStatuses = _repository.Statuses.Select(s => new CustomerDetailsViewModel.RequestsStatus()
                {
                    Level = s.Level,
                    Description = s.Description,
                    Count = requests.Where(r => r.StatusID == s.ID).Count()
                })
            };
            return View(viewModel);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            CustomerCreateViewModel customer = new CustomerCreateViewModel();
            return View(customer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Address, PhoneNo")] CustomerCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string currentUserID = User.Identity.GetUserId();
                Customer customer = new Customer()
                {
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    PhoneNo = viewModel.PhoneNo
                };
                if (_repository.Consultants.SingleOrDefault(c => c.UserID == currentUserID) == null)
                {
                    customer.UserID = currentUserID;
                }
                _repository.SaveCustomer(customer);
                return RedirectToAction("Details", new { id = customer.ID });
            }

            return View(viewModel);
        }

        // GET: Customers/Edit/5 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _repository.Customers.Single(r => r.ID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && currentCustomer.ID == customer.ID)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            CustomerEditViewModel viewModel = new CustomerEditViewModel()
            {
                ID = customer.ID,
                Name = customer.Name,
                Address = customer.Address,
                PhoneNo = customer.PhoneNo
            };
            return View(viewModel);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Name, Address, PhoneNo")] CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _repository.Customers.SingleOrDefault(r => r.ID == model.ID);
                if (customer == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string currentUserID = User.Identity.GetUserId();
                Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
                if (!(User.IsInRole("Consultant") || (currentCustomer != null && currentCustomer.ID == customer.ID)))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                customer.Name = model.Name;
                customer.Address = model.Address;
                customer.PhoneNo = model.PhoneNo;
                _repository.SaveCustomer(customer);

                TempData["message"] = string.Format("Zapisano {0} ", customer.Name);
                return RedirectToAction("Details", new { id = customer.ID });
            }
            return View(model);
        }
    }
}