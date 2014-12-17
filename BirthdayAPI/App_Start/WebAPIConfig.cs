using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BirthdayAPI
{
	public static class WebAPIConfig
	{
		public static void Register(HttpConfiguration config)
		{
			#region Routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "ControllerApi",
				routeTemplate: "{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }

			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
			#endregion WebAPI Routes
		}
	}
}