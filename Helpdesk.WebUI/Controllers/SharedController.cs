using Helpdesk.Domain.Abstract;
using Helpdesk.WebUI.Models.Shared;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.WebUI.Controllers
{
    public class SharedController : Controller
    {
        IRequestsRepository _repository;

        public SharedController(IRequestsRepository requestsRepository)
        {
            _repository = requestsRepository;
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            IIdentity user = User.Identity;
            bool isAuthenticated = user.IsAuthenticated;

            MenuViewModel viewModel = new MenuViewModel();
            viewModel.IsAuthenticated = isAuthenticated;

            if(isAuthenticated)
            {
                viewModel.IsCustomer = (user as ClaimsIdentity).HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Customer");
                viewModel.CustomerId = _repository.Customers.SingleOrDefault(c => c.UserID == user.GetUserId())?.ID;

                viewModel.IsConsultant = (user as ClaimsIdentity).HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Consultant");
            }
            
            return PartialView("~/Views/Shared/_TopMenu.cshtml", viewModel);
        }

        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            IIdentity user = User.Identity;
            bool isAuthenticated = user.IsAuthenticated;

            MenuViewModel viewModel = new MenuViewModel();
            viewModel.IsAuthenticated = isAuthenticated;

            if (isAuthenticated)
            {
                viewModel.IsCustomer = (user as ClaimsIdentity).HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Customer");
                viewModel.CustomerId = _repository.Customers.SingleOrDefault(c => c.UserID == user.GetUserId())?.ID;

                viewModel.IsConsultant = (user as ClaimsIdentity).HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Consultant");
            }

            return PartialView("~/Views/Shared/_LeftMenu.cshtml", viewModel);
        }
    }
}