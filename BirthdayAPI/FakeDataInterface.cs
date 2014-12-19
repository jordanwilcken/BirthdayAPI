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
			throw new NotImplementedException();
		}

		internal static bool AddBirthData(string user, Models.BirthData birthData)
		{
			throw new NotImplementedException();
		}
	}
}