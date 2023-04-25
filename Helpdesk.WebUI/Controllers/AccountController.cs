using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Entities.Users;
using Helpdesk.WebUI.Infrastructure;
using Helpdesk.WebUI.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IRequestsRepository _repository;

        public AccountController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.Name, details.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password.");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    ident.AddClaims(ClaimsRoles.CreateRolesFromClaims(ident, _repository));
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                    //if ((_repository.Consultants.Where(c => c.UserID == userID).Count() + _repository.Customers.Where(c => c.UserID == userID).Count()) != 1)
                    //{
                    //    return RedirectToAction("ChooseProfile");
                    //}

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Details");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Konto zostało utworzone pomyślnie. Możesz się zalogować.";
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            string currentID;
            if (string.IsNullOrEmpty(id))
            {
                currentID = User.Identity.GetUserId();
            }
            else
            {
                if (!User.IsInRole("Admin"))
                {
                    return View("Error", new string[] { "Access denied" });
                }
                currentID = id;
            }

            AppUser user = await UserManager.FindByIdAsync(currentID);
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

        //public ActionResult ChooseProfile()
        //{
        //    string userID = User.Identity.GetUserId();
        //    IEnumerable<Customer> customers = _repository.Customers.Where(c => c.UserID == userID);
        //    IEnumerable<Consultant> consultants = _repository.Consultants.Where(c => c.UserID == userID);
        //    ChooseProfileViewModel model = new ChooseProfileViewModel()
        //    {
        //        Customers = customers,
        //        Consultants = consultants
        //    };
        //    return View(model);
        //}

        private IAuthenticationManager AuthManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
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