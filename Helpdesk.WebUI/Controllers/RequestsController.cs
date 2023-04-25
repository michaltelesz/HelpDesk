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
using Helpdesk.Domain.Entities;
using Helpdesk.WebUI.Models.Requests;
using Helpdesk.WebUI.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private IRequestsRepository _repository;
        private int _requestsPageSize = 5;

        public RequestsController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: Requests
        public ActionResult Index(string state, int page = 1)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            List<int> states = new List<int>();
            IEnumerable<Request> entityRequests;
            if (state != null)
            {
                string[] splits = state.Split(',');
                foreach (string split in splits)
                {
                    if (int.TryParse(split, out int parsed))
                    {
                        states.Add(parsed);
                    }
                }

                entityRequests = _repository.Requests.Where(r => states.Contains(r.StatusID));
            }
            else
            {
                entityRequests = _repository.Requests;
            }

            IEnumerable<RequestIndexViewModel.Request> requests = entityRequests.Select(er => new RequestIndexViewModel.Request()
            {
                ID = er.ID,
                ReadableID = er.ReadableID,
                ReceivedDate = er.ReceivedDate,
                CustomerName = er.Computer == null ? _repository.Customers.SingleOrDefault(c => c.UserID == er.CreatedByID)?.Name : er.Computer.Owner.Name,
                StatusLevel = er.Status.Level,
                StatusDescription = er.Status.Description
            });

            RequestIndexViewModel viewModel = new RequestIndexViewModel()
            {
                Requests = requests.OrderByDescending(c => c.ReceivedDate).Skip((page - 1) * _requestsPageSize).Take(_requestsPageSize),
                Statuses = _repository.Statuses,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _requestsPageSize,
                    TotalItems = requests.Count()
                },
                States = string.Join(",", states)
            };
            return View(viewModel);
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? requestID, int page = 1)
        {
            int callsPageSize = 5;
            if (requestID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = _repository.Requests.SingleOrDefault(r => r.ID == requestID);

            if (request == null)
            {
                return HttpNotFound();
            }

            Consultant creatorConsultant = _repository.Consultants.SingleOrDefault(c => c.UserID == request.CreatedByID);
            Customer creatorCustomer = request.CreatedByID == null ? null : _repository.Customers.SingleOrDefault(c => c.UserID == request.CreatedByID);

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && ((creatorCustomer != null && currentCustomer.ID == creatorCustomer.ID) || (request.Computer != null && request.Computer.OwnerID == currentCustomer.ID)))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            int[] openedStatusIDs = { 1, 2, 4 };
            int[] closedStatusIDs = { 3 };
            bool canAssign = false;
            if ((request.AssignedTo != null && request.AssignedTo.UserID != User.Identity.GetUserId()) || request.AssignedTo == null)
            {
                canAssign = true;
            }
            IEnumerable<RequestDetailsViewModel.Call> calls = request.Calls.Select(c => new RequestDetailsViewModel.Call
            {
                ID = c.ID,
                Date = c.Date,
                Description = c.Description,
                StatusLevel = c.Status.Level,
                StatusDescription = c.Status.Description,
                HasComponents = c.ComponentCalls.Count > 0
            });
            RequestDetailsViewModel requestViewModel = new RequestDetailsViewModel()
            {
                ID = request.ID,
                ReadableID = request.ReadableID,
                Description = request.Description,
                ComputerID = request.ComputerID,
                ComputerName = request.Computer?.Name,
                CustomerID = request.Computer?.OwnerID,
                CustomerName = request.Computer?.Owner.Name,
                ReceivedDate = request.ReceivedDate,
                ResolvedDate = request.ResolvedDate,
                CreatorConsultant = creatorConsultant,
                CreatorCustomer = creatorCustomer,
                AssignedTo = request.AssignedTo,
                ClosedBy = request.ClosedBy,
                Status = request.Status,
                IsOpened = openedStatusIDs.Contains(request.StatusID),
                IsClosed = closedStatusIDs.Contains(request.StatusID),
                CanAssign = canAssign,
                Calls = calls.OrderByDescending(c => c.Date).Skip((page - 1) * callsPageSize).Take(callsPageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = callsPageSize,
                    TotalItems = _repository.Calls.Where(c => c.RequestID == requestID).Count()
                }
            };
            return View(requestViewModel);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<Customer> customers = _repository.Customers;
            RequestChooseCustomerViewModel viewModel = new RequestChooseCustomerViewModel()
            {
                Customers = customers
            };
            return View("ChooseCustomer", viewModel);
        }

        // GET: Requests/Create/ChooseComputer/5
        [Route("Requests/Create/ChooseComputer/{customerID:int}")]
        public ActionResult ChooseComputer(int customerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Customer customer = _repository.Customers.SingleOrDefault(c => c.ID == customerID);
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ICollection<Computer> computers = customer.Computers.Where(c => c.Temporary != true).ToList();
            RequestChooseComputerViewModel viewModel = new RequestChooseComputerViewModel()
            {
                CustomerID = customer.ID,
                CustomerName = customer.Name,
                Computers = computers
            };
            return View("ChooseComputer", viewModel);
        }

        // GET: Requests/Create/5
        [Route("Requests/Create/{computerID:int}")]
        public ActionResult Create(int computerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Computer computerEntity = _repository.Computers.SingleOrDefault(c => c.ID == computerID);
            if (computerEntity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestCreateViewModel viewModel = new RequestCreateViewModel
            {
                ComputerID = computerEntity.ID,
                Computer = computerEntity
            };
            return View("Create", viewModel);
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Requests/Create/{computerID:int}")]
        public ActionResult Create([Bind(Include = "Description, ComputerID")] RequestCreateViewModel model, int computerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                int? consultantID = _repository.Consultants.SingleOrDefault(c => c.UserID == userID).ID;
                if (consultantID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                int statusID = 1;
                Request request = new Request()
                {
                    Description = model.Description,
                    ComputerID = model.ComputerID,
                    ReceivedDate = DateTime.Now,
                    CreatedByID = userID,
                    StatusID = statusID,
                    ReadableID = GetNextReadableID(userID, false)
                };
                _repository.SaveRequest(request);

                Call call = new Call()
                {
                    RequestID = request.ID,
                    Date = DateTime.Now,
                    UserID = userID,
                    Description = "--- UTWORZONO ZGŁOSZENIE ---",
                    StatusID = statusID
                };
                _repository.SaveCall(call);

                Assign(request.ID, consultantID);
                return RedirectToAction("Details", new { id = request.ID });
            }

            return View(model);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = _repository.Requests.Single(r => r.ID == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && ((currentUserID == request.CreatedByID) || (request.Computer != null && request.Computer.OwnerID == currentCustomer.ID)))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            RequestEditViewModel viewModel = new RequestEditViewModel()
            {
                ID = request.ID,
                Description = request.Description
            };
            return View(viewModel);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Description")] RequestEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Request request = _repository.Requests.SingleOrDefault(r => r.ID == model.ID);
                if (request == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string currentUserID = User.Identity.GetUserId();
                Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
                if (!(User.IsInRole("Consultant") || (currentCustomer != null && ((currentUserID == request.CreatedByID) || (request.Computer != null && request.Computer.OwnerID == currentCustomer.ID)))))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                request.Description = model.Description;
                _repository.SaveRequest(request);

                TempData["message"] = string.Format("Zapisano {0} ", request.ReadableID);
                return RedirectToAction("Details", new { id = request.ID });
            }
            return View(model);
        }

        // GET: Requests/Customer/5
        public ActionResult CustomerIndex(int? customerID, string state, int page = 1)
        {
            Customer customer;

            if (customerID == null)
            {
                customer = _repository.Customers.SingleOrDefault(c => c.UserID == User.Identity.GetUserId());
            }
            else
            {
                customer = _repository.Customers.SingleOrDefault(c => c.ID == customerID);
            }

            if (customer == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && (currentCustomer == customer))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            List<int> states = new List<int>();
            if (state != null)
            {
                string[] splits = state.Split(',');
                foreach (string split in splits)
                {
                    if (int.TryParse(split, out int parsed))
                    {
                        states.Add(parsed);
                    }
                }
            }

            IEnumerable<Request> entityRequests;
            if (state == null)
            {
                entityRequests = _repository.Requests.Where(r => (r.Computer?.Owner != null && r.Computer.OwnerID == customer.ID) || (r.CreatedByID == customer.UserID));
            }
            else
            {
                entityRequests = _repository.Requests.Where(r => (states.Contains(r.Status.ID)) && ((r.Computer?.Owner != null && r.Computer.OwnerID == customer.ID) || (r.CreatedByID == customer.UserID)));
            }

            IEnumerable<RequestCustomerIndexViewModel.Request> requests = entityRequests.Select(er => new RequestCustomerIndexViewModel.Request()
            {
                ID = er.ID,
                ReadableID = er.ReadableID,
                ReceivedDate = er.ReceivedDate,
                ComputerName = er.Computer?.Name,
                StatusLevel = er.Status.Level,
                StatusDescription = er.Status.Description
            });

            RequestCustomerIndexViewModel viewModel = new RequestCustomerIndexViewModel()
            {
                Requests = requests.OrderByDescending(r => r.ReceivedDate).Skip((page - 1) * _requestsPageSize).Take(_requestsPageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _requestsPageSize,
                    TotalItems = requests.Count()
                },
                CustomerID = customer.ID,
                CustomerName = customer.Name,
                States = string.Join(",", states)
            };
            return View("CustomerIndex", viewModel);
        }

        //GET: Requests/Customer/Create
        [Route("Requests/Customer/Create")]
        public ActionResult CustomerCreate()
        {
            Customer customer = _repository.Customers.SingleOrDefault(c => c.UserID == User.Identity.GetUserId());
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequestCustomerCreateViewModel viewModel = new RequestCustomerCreateViewModel();
            viewModel.CustomerName = customer.Name;

            List<RequestCustomerCreateViewModel.Computer> computers = new List<RequestCustomerCreateViewModel.Computer>();
            computers.Add(new RequestCustomerCreateViewModel.Computer()
            {
                Name = null,
                ID = null
            });

            IEnumerable<Computer> customersComputers = customer.Computers.Where(c => c.Temporary == false);
            foreach (Computer computer in customersComputers)
            {
                computers.Add(new RequestCustomerCreateViewModel.Computer
                {
                    ID = computer.ID,
                    Name = $"[{computer.SerialNo}] {computer.Name}"
                });
            }
            viewModel.Computers = computers;
            return View(viewModel);
        }

        //POST: Requests/Customer/Create
        [HttpPost]
        [Route("Requests/Customer/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerCreate([Bind(Include = "Description, ComputerID")] RequestCustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Request request = new Request();
                string userID = User.Identity.GetUserId();

                int statusID = 1;
                request.ReceivedDate = DateTime.Now;
                request.StatusID = statusID;
                request.ReadableID = GetNextReadableID(userID, true);
                request.CreatedByID = userID;
                request.Description = model.Description;
                request.ComputerID = model.ComputerID;
                _repository.SaveRequest(request);

                Call call = new Call()
                {
                    RequestID = request.ID,
                    Date = DateTime.Now,
                    UserID = userID,
                    Description = "--- ZGLOSZENIE UTWORZONE PRZEZ KLIENTA ---",
                    StatusID = statusID
                };
                _repository.SaveCall(call);
                return RedirectToAction("Details", new { id = request.ID });
            }

            return View(model);
        }

        // GET: Requests/Consultant/5
        public ActionResult ConsultantIndex(int? consultantID, string state, int page = 1)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Consultant consultant;

            if (consultantID == null)
            {
                consultant = _repository.Consultants.SingleOrDefault(c => c.UserID == User.Identity.GetUserId());
            }
            else
            {
                consultant = _repository.Consultants.SingleOrDefault(c => c.ID == consultantID);
            }

            if (consultant == null)
            {
                return HttpNotFound();
            }

            List<int> states = new List<int>();
            if (state != null)
            {
                string[] splits = state.Split(',');
                foreach (string split in splits)
                {
                    if (int.TryParse(split, out int parsed))
                    {
                        states.Add(parsed);
                    }
                }
            }

            IEnumerable<Request> entityRequests;

            if (state == null)
            {
                entityRequests = _repository.Requests.Where(r => r.AssignedToID == consultant.ID);
            }
            else
            {
                entityRequests = _repository.Requests.Where(r => r.AssignedToID == consultant.ID && states.Contains(r.StatusID));
            }

            IEnumerable<RequestConsultantIndexViewModel.Request> requests = entityRequests.Select(er => new RequestConsultantIndexViewModel.Request()
            {
                ID = er.ID,
                ReadableID = er.ReadableID,
                ReceivedDate = er.ReceivedDate,
                CustomerName = er.Computer == null ? _repository.Customers.SingleOrDefault(c => c.UserID == er.CreatedByID)?.Name : er.Computer.Owner.Name,
                StatusLevel = er.Status.Level,
                StatusDescription = er.Status.Description
            });

            RequestConsultantIndexViewModel viewModel = new RequestConsultantIndexViewModel()
            {
                Requests = requests.OrderByDescending(r => r.ReceivedDate).Skip((page - 1) * _requestsPageSize).Take(_requestsPageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _requestsPageSize,
                    TotalItems = requests.Count()
                },
                ConsultantID = consultantID,
                States = string.Join(",", states)
            };
            return View("ConsultantIndex", viewModel);
        }

        [Route("Requests/Unassigned")]
        public ActionResult IndexUnassigned(int page = 1)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<Request> entityRequests = _repository.Requests.Where(r => r.AssignedTo == null && r.StatusID != 3);

            IEnumerable<RequestIndexViewModel.Request> requests = entityRequests.Select(er => new RequestIndexViewModel.Request()
            {
                ID = er.ID,
                ReadableID = er.ReadableID,
                ReceivedDate = er.ReceivedDate,
                CustomerName = er.Computer == null ? _repository.Customers.SingleOrDefault(c => c.UserID == er.CreatedByID)?.Name : er.Computer.Owner.Name,
                StatusLevel = er.Status.Level,
                StatusDescription = er.Status.Description
            });

            RequestIndexViewModel viewModel = new RequestIndexViewModel()
            {
                Requests = requests.OrderByDescending(r => r.ReceivedDate).Skip((page - 1) * _requestsPageSize).Take(_requestsPageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _requestsPageSize,
                    TotalItems = requests.Count()
                }
            };

            return View("IndexUnassigned", viewModel);
        }

        // GET: Requests/Close/5
        public ActionResult Close(int id)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Request request = _repository.Requests.SingleOrDefault(r => r.ID == id);
            int? statusID = 3;

            if (request == null)
            {
                return HttpNotFound();
            }

            if (statusID == null || request.StatusID == statusID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userID = User.Identity.GetUserId();
            Consultant consultant = _repository.Consultants.SingleOrDefault(c => c.UserID == userID);
            Call newCall = new Call
            {
                Date = DateTime.Now,
                StatusID = (int)statusID,
                UserID = userID,
                Description = $"--- ZAMKNIĘTO ---",
                RequestID = id
            };
            _repository.SaveCall(newCall);
            request.ResolvedDate = DateTime.Now;
            request.ClosedByID = consultant.ID;
            request.StatusID = (int)statusID;
            _repository.SaveRequest(request);

            return RedirectToAction("Details", new { id = request.ID });
        }

        // GET: Requests/Reopen/5
        public ActionResult Reopen(int id)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Request request = _repository.Requests.SingleOrDefault(r => r.ID == id);
            int? statusID = 2;

            if (request == null)
            {
                return HttpNotFound();
            }

            if (statusID == null || request.StatusID == statusID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userID = User.Identity.GetUserId();
            Consultant consultant = _repository.Consultants.SingleOrDefault(c => c.UserID == userID);
            Call newCall = new Call
            {
                Date = DateTime.Now,
                StatusID = (int)statusID,
                UserID = userID,
                Description = $"--- OTWARTO PONOWNIE ---",
                RequestID = id
            };
            _repository.SaveCall(newCall);

            request.ResolvedDate = null;
            request.StatusID = (int)statusID;
            _repository.SaveRequest(request);

            return RedirectToAction("Details", new { id = request.ID });
        }

        public ActionResult Assign(int? requestID, int? consultantID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Request request = _repository.Requests.SingleOrDefault(r => r.ID == requestID);

            if (request == null)
            {
                return HttpNotFound();
            }

            Consultant consultant;

            if (consultantID == null)
            {
                consultant = _repository.Consultants.SingleOrDefault(c => c.UserID == User.Identity.GetUserId());
            }
            else
            {
                consultant = _repository.Consultants.SingleOrDefault(c => c.ID == consultantID);
            }

            if (consultant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Call newCall = new Call
            {
                Date = DateTime.Now,
                StatusID = request.StatusID,
                Description = $"--- Zgłoszenie przypisane do konsultanta: {consultant.Name} ---",
                RequestID = request.ID
            };
            _repository.SaveCall(newCall);

            request.AssignedToID = consultant.ID;
            _repository.SaveRequest(request);

            return RedirectToAction("Details", new { id = request.ID });
        }

        private string GetNextReadableID(string id, bool isCustomer)
        {
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            int cID;
            if (isCustomer)
            {
                cID = (int)_repository.Customers.SingleOrDefault(c => c.UserID == id)?.ID;
            }
            else
            {
                cID = (int)_repository.Consultants.SingleOrDefault(c => c.UserID == id)?.ID;
            }
            int count = _repository.Requests.Where(r => r.ReceivedDate.Year == now.Year && r.CreatedByID == id).Count() + 1;
            return $"{year}/{(isCustomer ? cID.ToString("D4") : "S" + cID.ToString("D3"))}/{count.ToString("D3")}";
        }
    }
}
