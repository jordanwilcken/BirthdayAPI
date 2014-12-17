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
			return TheModel.GetBirthData(User);
		}

		public IEnumerable<BirthData> GetThisMonth()
		{
			var searchObject = new Dictionary<string, object> { { "month", DateTime.Now.Month } };
			return TheModel.GetBirthData(User, searchObject);
		}

		// POST api/<controller>
		public IEnumerable<BirthData> GetForPeople(Name[] names)
		{
			var searchObject = new Dictionary<string, object> { { "names", names } };
			return TheModel.GetBirthData(User, searchObject);
		}

		public bool Add(BirthData birthData)
		{
			return TheModel.Add(User, birthData);
		}

		public bool Update(BirthData birthData)
		{
			return TheModel.Update(User, birthData);
		}

		public void Delete(Name name)
		{
			var searchObject = new Dictionary<string, object> { { "names", new Name[] { name } } };
			TheModel.Delete(User, searchObject);
		}

		private string User
		{
			get
			{
				string user = HttpContext.Current.Session["UserName"] as string;
				if (user == null)
				{
					throw new Exception("You forgot to set the UserName on this session.");
				}
				return user;
			}
		}
	}

	public class Name
	{
		public string FirstName { get; set; }
		public char LastInitial { get; set; }
	}
}