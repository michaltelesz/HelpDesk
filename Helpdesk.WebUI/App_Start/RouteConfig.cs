using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            routes.MapRoute("RequestDetails_CallsPages", "Requests/{id}/Page/{page}", new { controller = "Requests", action = "Details" });
            routes.MapRoute("RequestDetails", "Requests/{id}", new { controller = "Requests", action = "Details" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Requests", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
