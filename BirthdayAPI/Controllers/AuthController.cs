using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace BirthdayAPI.Controllers
{
	public class AuthController : ApiController
	{
		public bool Login(Thing id)
		{
			if (!Membership.ValidateUser(id.prop1, id.prop2))
			{
				return false;
			}
			else
			{
				FormsAuthentication.SetAuthCookie(id.prop1, false);
				HttpContext.Current.Session["UserName"] = id.prop1;
				return true;
			}
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
		}

		//below here is boiler plate

		// GET api/<controller>
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody]string value)
		{
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}

	public class Thing
	{
		public string prop1 { get; set; }
		public string prop2 { get; set; }
	}
}