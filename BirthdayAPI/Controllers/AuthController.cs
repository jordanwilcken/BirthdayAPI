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
		public bool Login(Credentials credentials)
		{
			if (!Membership.ValidateUser(credentials.username, credentials.password))
			{
				return false;
			}
			else
			{
				FormsAuthentication.SetAuthCookie(credentials.username, false);
				return true;
			}
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
		}

		public object AddUser(Credentials credentials)
		{
			if (credentials.username == null)
			{
				return new { Message = "I'm sorry, I couldn't find the username in the data you supplied." };
			}
			if (credentials.password == null)
			{
				return new { Message = "I'm sorry, I couldn't find the password in the data you supplied." };
			}

			try
			{
				Membership.CreateUser(credentials.username, credentials.password);
			}
			catch (MembershipCreateUserException e)
			{
				return new { Message = "User not created.  Here's what we know:\n\n" + e.Message };
			}

			return new { Message = "User created." };
		}
	}

	public class Credentials
	{
		public string username { get; set; }
		public string password { get; set; }
	}
}