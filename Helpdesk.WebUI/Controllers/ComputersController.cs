using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Domain.Concrete;
using Helpdesk.Domain.Entities.Computers;
using Helpdesk.Domain.Abstract;
using Helpdesk.WebUI.Models.Computers;
using Helpdesk.Domain.Entities.Users;
using Microsoft.AspNet.Identity;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class ComputersController : Controller
    {
        private IRequestsRepository _repository;

        public ComputersController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: Requests
        public ActionResult Index()
        {
            return View(_repository.Computers);
        }

        // GET: Computers/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = _repository.Computers.SingleOrDefault(r => r.ID == id);
            if (computer == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            Customer currentCustomer = _repository.Customers.SingleOrDefault(c => c.UserID == currentUserID);
            if (!(User.IsInRole("Consultant") || (currentCustomer != null && (currentCustomer.ID == computer.OwnerID))))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            List<ComputerDetailsViewModel.ComponentCategory> componentsByCategories = new List<ComputerDetailsViewModel.ComponentCategory>();
            IEnumerable<ComponentTypeCategory> typeCategories = _repository.TypeCategories.OrderByDescending(tc => tc.Priority);
            foreach (ComponentTypeCategory category in typeCategories)
            {
                IEnumerable<Component> componentsInCategory = computer.Components.Where(c => c.Type.CategoryID == category.ID).OrderBy(c=>c.Type.Name);
                if (componentsInCategory.Count() > 0)
                {
                    componentsByCategories.Add(new ComputerDetailsViewModel.ComponentCategory()
                    {
                        Name = category.Name,
                        Components = componentsInCategory.Select(c => new ComputerDetailsViewModel.Component()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            SerialNo = c.SerialNo,
                            TypeName = c.Type.Name
                        })
                    });
                }
            }
            ComputerDetailsViewModel viewModel = new ComputerDetailsViewModel()
            {
                ID = computer.ID,
                Name = computer.Name,
                SerialNo = computer.SerialNo,
                OwnerID = computer.OwnerID,
                OwnerName = computer.Owner.Name,
                ComponentsByCategories = componentsByCategories
            };
            return View(viewModel);
        }

        // GET: Computers/Create/5
        [Route("Computers/Create/{customerID:int}")]
        public ActionResult Create(int customerID)
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
            ComputerCreateViewModel viewModel = new ComputerCreateViewModel()
            {
                CustomerName = customer.Name
            };
            return View(viewModel);
        }

        // POST: Computers/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Computers/Create/{customerID:int}")]
        public ActionResult Create([Bind(Include = "Name, SerialNo")] ComputerCreateViewModel model, int customerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                Computer newComputer = new Computer
                {
                    OwnerID = customerID,
                    Temporary = false,
                    Name = model.Name,
                    SerialNo = model.SerialNo
                };
                _repository.SaveComputer(newComputer);

                return RedirectToAction("Details", new { id = newComputer.ID });
            }
            Customer customer = _repository.Customers.SingleOrDefault(c => c.ID == customerID);
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.CustomerName = customer.Name;
            return View(model);
        }

        // GET: Computers/CreateTemp/5
        [Route("Computers/CreateTemp/{customerID:int}")]
        public ActionResult CreateTemp(int customerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            IEnumerable<ComputerCreateTempViewModel.ComponentType> componentTypes = _repository.ComponentTypes.Select(c => new ComputerCreateTempViewModel.ComponentType()
            {
                ID = c.ID,
                Name = c.Name,
                DataGroup = c.Category.Name
            });
            ComputerCreateTempViewModel viewModel = new ComputerCreateTempViewModel()
            {
                ComponentTypes = componentTypes
            };
            return View("CreateTemp", viewModel);
        }

        // POST: Computers/CreateTemp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Computers/CreateTemp/{customerID:int}")]
        public ActionResult CreateTemp([Bind(Include = "ComponentName, ComponentSerialNo, ComponentTypeID")] ComputerCreateTempViewModel model, int customerID)
        {
            if (!(User.IsInRole("Consultant")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                int customerTempComputersCount = _repository.Computers.Count(c => c.OwnerID == customerID && c.Temporary == true);
                Computer newComputer = new Computer
                {
                    OwnerID = customerID,
                    Temporary = true,
                    Name = $"K:{model.ComponentName}::{model.ComponentSerialNo}",
                    SerialNo = "#TEMP/" + customerID + "/" + (customerTempComputersCount + 1) + "#"
                };
                _repository.SaveComputer(newComputer);

                Component component = new Component()
                {
                    ComputerID = newComputer.ID,
                    Name = model.ComponentName,
                    SerialNo = model.ComponentSerialNo,
                    TypeID = model.ComponentTypeID
                };
                _repository.SaveComponent(component);

                return RedirectToAction("Create", "Requests", new { computerID = newComputer.ID });
            }
            IEnumerable<ComputerCreateTempViewModel.ComponentType> componentTypes = _repository.ComponentTypes.Select(c => new ComputerCreateTempViewModel.ComponentType()
            {
                ID = c.ID,
                Name = c.Name,
                DataGroup = c.Category.Name
            });
            model.ComponentTypes = componentTypes;
            return View(model);
        }
    }
}
