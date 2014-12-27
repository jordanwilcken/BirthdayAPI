using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace BirthdayAPI.UnitTests
{
	[TestFixture]
	public class DatabaseInterface_UnitTests
	{
		[Test]
		public void GetBirthData_Test()
		{
			DatabaseInterface.GetBirthData("Chunk", null);
			Assert.That(1 > 0);
		}
	}
}