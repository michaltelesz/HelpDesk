using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.WebUI.Infrastructure;
using Helpdesk.WebUI.Models;
using Helpdesk.WebUI.Models.Account;
using Helpdesk.WebUI.Models.UserAdmin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {
        IRequestsRepository _repository;

        public UserAdminController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        // GET: UserAdmin/1
        [Route("UserAdmin/{page}")]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            UserIndexViewModel viewModel = new UserIndexViewModel();
            List<UserIndexViewModel.User> users = new List<UserIndexViewModel.User>();
            foreach (AppUser user in UserManager.Users.OrderBy(c => c.UserName).Skip((page - 1) * pageSize).Take(pageSize))
            {
                IEnumerable<string> roles = UserManager.GetRoles(user.Id);
                UserIndexViewModel.User viewModelUser = new UserIndexViewModel.User()
                {
                    ID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                };
                users.Add(viewModelUser);
            }
            UserIndexViewModel model = new UserIndexViewModel()
            {
                Users = users,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = UserManager.Users.Count()
                }
            };
            return View(model);
        }

        public async Task<ActionResult> Details(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error", new string[] { "User Not Found" });
            }
            UserDetailsViewModel model = new UserDetailsViewModel()
            {
                Name = user.UserName,
                Email = user.Email,
                Roles = await UserManager.GetRolesAsync(user.Id),
                Customer = _repository.Customers.FirstOrDefault(c => c.UserID == user.Id),
                Consultant = _repository.Consultants.FirstOrDefault(c => c.UserID == user.Id)
            };
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> EditRoles(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error", new string[] { "User Not Found" });
            }

            IEnumerable<string> currentRolesNames = await UserManager.GetRolesAsync(user.Id);
            IEnumerable<AppRole> currentRoles = currentRolesNames.Select(r => RoleManager.FindByNameAsync(r).Result).ToList();
            IEnumerable<AppRole> rolesToAdd = RoleManager.Roles.ToList().Except(currentRoles);
            UserRoleEditViewModel model = new UserRoleEditViewModel()
            {
                User = user,
                CurrentRoles = currentRoles,
                RolesToAdd = rolesToAdd
            };
            return View(model);
        }

        private AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        private AppRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>(); }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}