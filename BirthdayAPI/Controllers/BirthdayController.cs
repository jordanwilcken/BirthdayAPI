using BirthdayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BirthdayAPI.Controllers
{
	[Authorize]
	public class BirthdayController : ApiController
	{
		// GET api/<controller>
		public IEnumerable<BirthData> Get()
		{
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name);
		}

		public IEnumerable<BirthData> GetThisMonth()
		{
			var searchObject = new Dictionary<string, object> { { "month", DateTime.Now.Month } };
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name, searchObject);
		}

		// POST api/<controller>
		public IEnumerable<BirthData> GetForPeople(Name[] names)
		{
			var searchObject = new Dictionary<string, object> { { "names", names } };
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name, searchObject);
		}

		public bool Add(BirthData birthData)
		{
			return TheModel.Add(HttpContext.Current.User.Identity.Name, birthData);
		}

		public bool Update(BirthData birthData)
		{
			return TheModel.Update(HttpContext.Current.User.Identity.Name, birthData);
		}

		public void Delete(Name name)
		{
			var searchObject = new Dictionary<string, object> { { "names", new Name[] { name } } };
			TheModel.Delete(HttpContext.Current.User.Identity.Name, searchObject);
		}
	}

	public class Name
	{
		public string FirstName { get; set; }
		public char LastInitial { get; set; }
	}
}