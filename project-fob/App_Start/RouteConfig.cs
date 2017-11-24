using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace project_fob {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Host",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Host", action = "MeetingPageHost"}

                );
            routes.MapRoute(
                name: "Attendee",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Attendee", action = "MeetingPageUser" }

                );
        }
    }
}
