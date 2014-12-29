using BirthdayAPI.Controllers;
using BirthdayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayAPI
{
	public static class FakeDataInterface
	{
		internal static IEnumerable<Models.BirthData> GetBirthData(string user)
		{
			throw new NotImplementedException();
		}

		internal static IEnumerable<Models.BirthData> GetBirthData(string user, Dictionary<string, object> searchObject)
		{
			var theBirthData = new List<BirthData>();

			if (searchObject.ContainsKey("names"))
			{
				var names = searchObject["names"] as Name[];
				foreach (Name name in names)
				{
					var birthData = new BirthData(name.FirstName, name.LastInitial, "03-22", "MM-DD");
					theBirthData.Add(birthData);
				}
			}

			return theBirthData;
		}

		internal static bool AddBirthData(string user, Models.BirthData birthData)
		{
			throw new NotImplementedException();
		}

		internal static object UpdateBirthData(string user, Models.BirthData birthData)
		{
			throw new NotImplementedException();
		}

		internal static object DeleteBirthData(string user, Dictionary<string, object> searchObject)
		{
			throw new NotImplementedException();
		}
	}
}