﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using BirthdayAPI.Controllers;
using BirthdayAPI.Models;
using System.Text.RegularExpressions;

namespace BirthdayAPI.UnitTests
{
	[TestFixture]
	public class DatabaseInterface_UnitTests
	{
		public const string USERNAME = "Chunk";

		[Test]
		public void GetBirthData_Test()
		{
			string firstName = "Sloth";
			char lastInitial = 'F';
			var theData = DatabaseInterface.GetBirthData(
				USERNAME,
				new Dictionary<string, object>
				{
					{ "names", new Name[] { new Name { FirstName = firstName, LastInitial = lastInitial } } }
				}
			);

			Assert.That(theData.Any(), "Didn't get any birthdata out of the database, but should have.");

			foreach(BirthData birthData in theData)
			{
				string failMessage = string.Format("Was trying to retrieve birthday of someone named '{0}', but somehow got birthday of someone named '{1}'.", firstName, birthData.Name.FirstName);
				Assert.That(Regex.IsMatch(birthData.Name.FirstName, "^" + firstName + "$"), failMessage);

				failMessage = string.Format("Was trying to retrieve birthday of someone with last initial '{0}', but somehow got birthday of someone with last initial '{1}'.", lastInitial, birthData.Name.LastInitial);
				Assert.That(Regex.IsMatch(birthData.Name.LastInitial.ToString(), "^" + lastInitial.ToString() + "$"), failMessage);
			}
		}
	}
}