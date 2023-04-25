using Helpdesk.Domain.Abstract;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Helpdesk.WebUI.Infrastructure
{
    public class ClaimsRoles
    {
        public static IEnumerable<Claim> CreateRolesFromClaims(ClaimsIdentity user, IRequestsRepository repository)
        {
            List<Claim> claims = new List<Claim>();
            if (repository.Customers.Any(c => c.UserID == user.GetUserId()))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }
            //if (repository.Consultants.Any(c => c.UserID == user.GetUserId()))
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, "Consultant"));
            //}
            return claims;
        }
    }
}