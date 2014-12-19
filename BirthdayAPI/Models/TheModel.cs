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

		public static object Add(string user, BirthData birthData)
		{
			Match match = Regex.Match(birthData.BirthdayFormat, "mm-dd", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				if (birthData.Birthday.Length > 5)
				{
					birthData.Birthday = birthData.Birthday.Substring(match.Index, 5);
					if (!BirthdayMatchesFormat(birthData.Birthday, "mm-dd"))
					{
						if (!BirthData.TryFormatBirthday(birthData.Birthday, "mm-dd"))
						{
							//not going to continue because I dunno what you give me for a birthday.
						}
					}
					//birthData.BirthdayFormat = "MM-DD";
				}
			}
			else 
			{
				DateTime dt;
				if (!DateTime.TryParse(birthData.Birthday, out dt))
				{
					return new { Message = "Sorry, I couldn't validate the birthday you passed in. Try entering the birthday in the format 'MM-DD'" };
				}
				birthData.Birthday = string.Format("{0}-{1}", dt.Month.ToString("00"), dt.Day.ToString("00"));
				birthData.BirthdayFormat = "MM-DD";
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

		private static bool BirthdayMatchesFormat(string birthday, string format)
		{
			if (!Regex.IsMatch(format, "mm-dd", RegexOptions.IgnoreCase))
			{
				throw new NotImplementedException();
			}


			return false;
		}

		internal static bool Update(string User, BirthData birthData)
		{
			throw new NotImplementedException();
		}

		internal static void Delete(string User, Dictionary<string, object> searchObject)
		{
			throw new NotImplementedException();
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
			BirthdayFormat = birthdayFormat;
		}

		public Name Name {get;set;}
		public string Birthday {get; set;}
		public string BirthdayFormat { get; set; }

		internal static bool TryFormatBirthday(string p1, string p2)
		{
			throw new NotImplementedException();
		}
	}
}