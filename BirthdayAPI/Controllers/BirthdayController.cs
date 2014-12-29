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
		/**
		 * @apiGroup Birthday
		 * @api {Get} /Birthday Get all birthdays for current user
		 * @apiSuccess {object[]} return				The object returned by the API
		 * @apiSuccess {object} return.name			Name Info
		 * @apiSuccess {string} return.name.firstname	The person's first name
		 * @apiSuccess {string} return.name.lastinitial	The person's last initial
		 * @apiSuccess {string} return.birthday			The person's birthday
		 * @apiSuccess {string} return.birthdayformat	The format of the birthday
		 */
		public IEnumerable<BirthData> Get()
		{
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name);
		}

		/**
		 * @apiGroup Birthday
		 * @api {Get} /Birthday/GetThisMonth Get birthdays for this month belonging to current user 
		 * @apiSuccess {object[]} return				The object returned by the API
		 * @apiSuccess {object} return.name			Name Info
		 * @apiSuccess {string} return.name.firstname	The person's first name
		 * @apiSuccess {string} return.name.lastinitial	The person's last initial
		 * @apiSuccess {string} return.birthday			The person's birthday
		 * @apiSuccess {string} return.birthdayformat	The format of the birthday
		 */
		public IEnumerable<BirthData> GetThisMonth()
		{
			var searchObject = new Dictionary<string, object> { { "month", DateTime.Now.Month } };
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name, searchObject);
		}

		/**
		 * @apiGroup Birthday
		 * @api {Post} /Birthday/GetForPeople Gets birthdays for people with matching names belonging to current user
		 * 
		 * @apiParam {object[]} param				The parameter object expected by the API
		 * @apiParam {string} param.firstname		Optional first name of the person
		 * @apiParam {string} param.lastinitial		Optional last initial of the person
		 * 
		 * @apiSuccess {object[]} return				The object returned by the API
		 * @apiSuccess {object} return.name			Name Info
		 * @apiSuccess {string} return.name.firstname	The person's first name
		 * @apiSuccess {string} return.name.lastinitial	The person's last initial
		 * @apiSuccess {string} return.birthday			The person's birthday
		 * @apiSuccess {string} return.birthdayformat	The format of the birthday
		 */
		[HttpPost]
		public IEnumerable<BirthData> GetForPeople(Name[] names)
		{
			var searchObject = new Dictionary<string, object> { { "names", names } };
			return TheModel.GetBirthData(HttpContext.Current.User.Identity.Name, searchObject);
		}

		/**
		 * @apiGroup Birthday
		 * @api {Post} /Birthday/Add Store a birthday for current user
		 * 
		 * @apiParam {object} name				Name Info
		 * @apiParam {string} name.firstname		The person's first name
		 * @apiParam {string} name.lastinitial		The person's last initial
		 * @apiParam {string} birthday			The person's birthday
		 * @apiParam {string} birthdayformat		The optional format of the birthday (i.e. MM-DD)
		 * 
		 * @apiSuccess {string} message			A description of what happened during the attempt to add the birthday
		 */
		public object Add(BirthData birthData)
		{
			return TheModel.Add(HttpContext.Current.User.Identity.Name, birthData);
		}

		/**
		 * @apiGroup Birthday
		 * @api {Post} /Birthday/Update Update a birthday for current user
		 * 
		 * @apiParam {object} name				Name Info
		 * @apiParam {string} name.firstname		The person's first name
		 * @apiParam {string} name.lastinitial		The person's last initial
		 * @apiParam {string} birthday			The person's birthday
		 * @apiParam {string} birthdayformat		The optional format of the birthday (i.e. MM-DD)
		 * 
		 * @apiSuccess {string} message			A description of what happened during the attempt to update the birthday
		 */
		public object Update(BirthData birthData)
		{
			return TheModel.Update(HttpContext.Current.User.Identity.Name, birthData);
		}

		/**
		 * @apiGroup Birthday
		 * @api {Post} /Birthday/Delete Delete a birthday for current user
		 * 
		 * @apiParam {object[]} param				The parameter object expected by the API
		 * @apiParam {string} param.firstname		The first name of the person to delete from birthdays
		 * @apiParam {string} param.lastinitial		The last initial of the person to delete from birthdays
		 */
		[HttpPost]
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