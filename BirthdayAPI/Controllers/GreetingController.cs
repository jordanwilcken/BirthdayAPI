using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace BirthdayAPI
{
	[Authorize]
	public class GreetingController : ApiController
	{
		[HttpGet, AllowAnonymous]
		public string Get()
		{
			var thatUser = Membership.GetUser(@"Phillip");
			string theGreeting = "You have been grunted!";
			if (thatUser != null)
			{
				theGreeting = string.Format("Well, hello, {0}!", thatUser.UserName);
			}
			return theGreeting;
		}

		[HttpGet, AllowAnonymous]
		public string Smitty(int id)
		{
			return id.ToString();
		}

		[HttpGet]
		public string Protected()
		{
			return "I think you are not anonymous.";
		}
	}
}