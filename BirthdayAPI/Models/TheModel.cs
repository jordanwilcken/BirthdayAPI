using BirthdayAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace BirthdayAPI.Models
{
	public static class TheModel
	{
		public static bool UseFakeData 
		{
			get { return bool.Parse(ConfigurationManager.AppSettings.Get("UseFakeData")); }
			set { ConfigurationManager.AppSettings.Set("UseFakeData", value.ToString()); }
		}

		public static IEnumerable<BirthData> GetBirthData(string user)
		{
			if (UseFakeData)
			{
				return FakeDataInterface.GetBirthData(user);
			}

			return DatabaseInterface.GetBirthData(user);
		}

		public static IEnumerable<BirthData> GetBirthData(string user, Dictionary<string, object> searchObject)
		{
			if (UseFakeData)
			{
				return FakeDataInterface.GetBirthData(user, searchObject);
			}

			return DatabaseInterface.GetBirthData(user, searchObject);
		}

        public static string TheBirthdayFormat
        {
            get { return ConfigurationManager.AppSettings.Get("TheBirthdayFormat"); }
            set { ConfigurationManager.AppSettings.Set("TheBirthdayFormat", value); }
        }

	   public static object Add(string user, BirthData birthData)
	   {
		   string theBirthdayFormat = TheBirthdayFormat;
		   Match match = Regex.Match(birthData.BirthdayFormat, theBirthdayFormat, RegexOptions.IgnoreCase);
		   if (!match.Success && !birthData.TryFormatBirthday(theBirthdayFormat))
		   {
			   return new { Message = "Sorry, I can't understand the birthday you supplied." };
		   }
		   
		   if (UseFakeData)
		   {
			   return FakeDataInterface.AddBirthData(user, birthData);
		   }

		   string returnMessage = DatabaseInterface.AddBirthData(user, birthData);
		   if (Regex.IsMatch(returnMessage, "success", RegexOptions.IgnoreCase))
		   {
			   return new { Message = returnMessage };
		   }
		   else
		   {
			   return new { Message = "There was a problem adding that birthday. Here's what we know:\n\n" + returnMessage };
		   }
	   }



	   internal static object Update(string user, BirthData birthData)
	   {
		   if (birthData == null)
		   {
			   return new { Message = "I'm sorry, the server could not find the birth data in the object you passed in." };
		   }

		   string theBirthdayFormat = TheBirthdayFormat;
		   string formatIn = birthData.BirthdayFormat;
		   if (formatIn == null)
		   {
			   formatIn = string.Empty;
		   }
		   Match match = Regex.Match(formatIn, theBirthdayFormat, RegexOptions.IgnoreCase);
		   if (!match.Success && !birthData.TryFormatBirthday(theBirthdayFormat))
		   {
			   return new { Message = "Sorry, I can't understand the birthday you supplied." };
		   }

		   if (UseFakeData)
		   {
			   return FakeDataInterface.UpdateBirthData(user, birthData);
		   }

		   string returnMessage = DatabaseInterface.UpdateBirthData(user, birthData);
		   if (Regex.IsMatch(returnMessage, "success", RegexOptions.IgnoreCase))
		   {
			   return new { Message = returnMessage };
		   }
		   else
		   {
			   return new { Message = "There was a problem adding that birthday. Here's what we know:\n\n" + returnMessage };
		   }
	   }

	   internal static void Delete(string user, Dictionary<string, object> searchObject)
		{
			if (UseFakeData)
			{
				FakeDataInterface.DeleteBirthData(user, searchObject);
			}

			DatabaseInterface.DeleteBirthData(user, searchObject);
		}
	}

	public class BirthData
	{
		public BirthData(string firstName, char lastInitial, string birthday, string birthdayFormat = "MM-DD")
		{
			if (string.IsNullOrEmpty(firstName))
			{
				throw new ArgumentException("The first name supplied was not valid.");
			}

			if (birthday == null)
			{
				throw new ArgumentException("You cannot have a null birthday.");
			}

			Name = new Name { FirstName = firstName, LastInitial = lastInitial };
			Birthday = birthday;
			if (!TryFormatBirthday(birthdayFormat))
			{
				BirthdayFormat = "unknown";
			}
		}

		public Name Name { get; set; }
		public string Birthday { get; set; }
		public string BirthdayFormat { get; private set; }
		public bool TryFormatBirthday(string format)
		{
			if (!Regex.IsMatch(format, "mm-dd", RegexOptions.IgnoreCase))
			{
				throw new NotImplementedException();
			}

			if (Regex.IsMatch(Birthday, @"\d{2}-\d{2}"))
			{
				BirthdayFormat = "MM-DD";
				return true;
			}

			DateTime dt;
			if (!DateTime.TryParse(Birthday, out dt))
			{
				return false;
			}

			Birthday = string.Format("{0}-{1}", dt.Month.ToString("00"), dt.Day.ToString("00"));
			BirthdayFormat = "MM-DD";
			return true;
		}

	}
}