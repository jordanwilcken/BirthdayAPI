using BirthdayAPI.Controllers;
using BirthdayAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace BirthdayAPI
{
	public static class DatabaseInterface
	{
		public static BirthData[] GetBirthData(string user)
		{
			List<BirthData> birthdata = new List<BirthData>();

			string connectionString = ConfigurationManager.ConnectionStrings["SQLiteDbConnection"].ConnectionString;
			SQLiteConnection connection = new SQLiteConnection(connectionString);
			DataTable table = new DataTable();
			string selectText = string.Format(@"SELECT * FROM birthdata WHERE owner = '{0}'", user);
			SQLiteCommand command = new SQLiteCommand(selectText, connection);
			SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
			try
			{
				connection.Open();
				adapter.Fill(table);
			}
			catch (Exception e)
			{
				birthdata.Add(new BirthData("There was an error retrieving birthdays on the server", 'X', "Error", "Sorry"));
			}
			finally
			{
				connection.Close();
			}

			foreach (DataRow row in table.Rows)
			{
				birthdata.Add(
					new BirthData((string)row["first_name"], ((string)row["last_initial"]).ToCharArray()[0], (string)row["birthday"], "MM-DD")
				);
			}

			return birthdata.ToArray();
		}

		public static IEnumerable<BirthData> GetBirthData(string user, Dictionary<string, object> searchObject)
		{
			List<BirthData> birthdata = new List<BirthData>();

			string connectionString = ConfigurationManager.ConnectionStrings["SQLiteDbConnection"].ConnectionString;
			SQLiteConnection connection = new SQLiteConnection(connectionString);
			DataTable table = new DataTable();
			string selectText = @"SELECT * FROM birthdata";
			SQLiteCommand command = new SQLiteCommand(selectText, connection);
			command = AddWhereConditions(command, searchObject);

			SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
			try
			{
				connection.Open();
				adapter.Fill(table);
			}
			catch (Exception e)
			{
				birthdata.Add(new BirthData("There was an error retrieving birthdays on the server", 'X', "Error", "Sorry"));
			}
			finally
			{
				connection.Close();
			}

			foreach (DataRow row in table.Rows)
			{
				birthdata.Add(
					new BirthData((string)row["first_name"], ((string)row["last_initial"]).ToCharArray()[0], (string)row["birthday"], "MM-DD")
				);
			}

			return birthdata;
		}


		public static string AddBirthData(string user, BirthData birthData)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["SQLiteDbConnection"].ConnectionString;
			SQLiteConnection connection = new SQLiteConnection(connectionString);
			DataTable table = new DataTable();
			string insertText = string.Empty
				+ @"INSERT INTO birthdata (owner, first_name, last_initial, birthday) VALUES "
				+ "(@owner, @first_name, @last_initial, @birth_day)";
			SQLiteCommand command = new SQLiteCommand(insertText, connection);
			command.Parameters.AddWithValue("@owner", user);
			command.Parameters.AddWithValue("@first_name", birthData.Name.FirstName);
			command.Parameters.AddWithValue("@last_initial", birthData.Name.LastInitial.ToString());
			command.Parameters.AddWithValue("@birth_day", birthData.Birthday);

			string returnMessage = "I dunno what happened.";
			try
			{
				connection.Open();
				if (command.ExecuteNonQuery() > 0)
				{
					returnMessage = "Success!";
				}
			}
			catch (Exception e)
			{
				returnMessage = string.Format("There was an error. This was the message:\r\n\r\n{0}", e.Message);
			}
			finally
			{
				connection.Close();
			}

			return returnMessage;
		}

		private static SQLiteCommand AddWhereConditions(SQLiteCommand command, Dictionary<string, object> searchObject)
		{
			var conditions = new List<string>();
			var conditionParameters = new Dictionary<string, string>();

			if (searchObject.ContainsKey("names"))
			{
				var names = searchObject["names"] as Name[];
				var nameConditions = new List<string>();
				string nameCondition;
				int parameterNumber = 1;
				foreach (Name name in names)
				{
					var currentParameters = new Dictionary<string, string> {
						{ "@first_name" + parameterNumber, null },
						{ "@last_initial" + parameterNumber, null }
					};
					nameCondition = GetNameCondition(name, currentParameters);
					if (!string.IsNullOrEmpty(nameCondition))
					{
						nameConditions.Add(nameCondition);
						foreach (var pair in currentParameters)
						{
							if (pair.Value != null)
							{
								conditionParameters.Add(pair.Key, pair.Value);
							}
						}
						++parameterNumber;
					}
				}
				if (nameConditions.Count > 0)
				{
					conditions.Add(string.Format("({0})", string.Join(" OR ", nameConditions)));
				}
			}

			if (searchObject.ContainsKey("month"))
			{
				int month = (int)searchObject["month"];
				conditions.Add(string.Format("instr(birthday, {0}) = 1", month.ToString("00")));
			}

			if (conditions.Count != 0)
			{
				command.CommandText += " WHERE " + string.Join(" AND ", conditions);
				foreach (var pair in conditionParameters)
				{
					command.Parameters.AddWithValue(pair.Key, pair.Value);
				}
			}

			return command;
		}

		/// <summary>
		///  Builds a condition from the given name and populates the value in each parameter pair.
		/// </summary>
		/// <param name="name"></param>
		/// /// <param name="command">The collection of parameters to populate. You supply the names, and we'll supply the values.</param>
		/// <returns>A parameterized name condition.</returns>
		private static string GetNameCondition(Name name, Dictionary<string, string> parameters)
		{
			if (parameters.Count < 2)
			{
				throw new ArgumentException("You must pass a list containing at least 2 parameters into GetNameCondition.");
			}

			foreach (var pair in parameters)
			{
				if (string.IsNullOrEmpty(pair.Key))
				{
					throw new ArgumentException("You must pass named parameters into GetNameCondition.");
				}
			}

			string condition = string.Empty;
			if (!string.IsNullOrEmpty(name.FirstName))
			{
				string parameterName = parameters.Keys.First((key) => key.Contains("first"));
				condition = string.Format(@"first_name = {0}", parameterName);
				parameters[parameterName] = name.FirstName;
				if (name.LastInitial != '\0')
				{
					parameterName = parameters.Keys.First((key) => key.Contains("last"));
					condition += string.Format(@" AND last_initial = {0}", parameterName);
					parameters[parameterName] = name.LastInitial.ToString();
				}
			}
			else if (name.LastInitial != '\0')
			{
				string parameterName = parameters.Keys.First((key) => key.Contains("last"));
				condition = string.Format(@"last_initial = {0}", parameterName);
				parameters[parameterName] = name.LastInitial.ToString();
			}
			return condition;
		}
	}
}