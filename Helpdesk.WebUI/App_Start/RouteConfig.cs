using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;

namespace Helpdesk.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(name: "Request_Create_Customers",
            //    url : "Requests/");
            //routes.MapRoute(null, "Requests/Customer/{customerId}/{page}", new { controller = "Requests", action = "CustomerIndex", customerId = UrlParameter.Optional });
            //routes.MapRoute(null, "Requests/Customer/#/{page}", new { controller = "Requests", action = "CustomerIndex"});
            routes.MapRoute("RequestsCustomerIndex", "Requests/Customer/{customerID}", new { controller = "Requests", action = "CustomerIndex", customerID = UrlParameter.Optional });
            routes.MapRoute("RequestsConsultantIndex", "Requests/Consultant/{consultantID}", new { controller = "Requests", action = "ConsultantIndex", consultantID = UrlParameter.Optional });

            routes.MapRoute(null, "Requests/Details/{requestID}/{page}", new { controller = "Requests", action = "Details"});
            routes.MapRoute(null, "Requests/Details/{requestID}", new { controller = "Requests", action = "Details" });

            //routes.MapRoute("UserAdminIndex", "UserAdmin/{page}", new { controller = "UserAdmin", action = "Index" });

            //routes.MapRoute("RequestDetails", "Requests/Details/{id}", new { controller = "Requests", action = "Details" }, new { id = new IntRouteConstraint() });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
