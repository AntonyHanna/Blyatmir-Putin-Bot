using System.Data.SqlClient;
using System.Reflection;

namespace BlyatmirPutin.DataAccess
{
	internal class DatabaseHelper
	{
		#region Properties
		private static SqlConnection? _databaseConnection;
		private static SqlConnection DatabaseConnection
		{
			get
			{
				if (_databaseConnection == null)
				{
					_databaseConnection = new SqlConnection(@"Data Source=(localdb)\ProjectModels;Initial Catalog=TESTDB;Integrated " +
			"Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent" +
			"=ReadWrite;MultiSubnetFailover=False");
					_databaseConnection.Open();
				}
				return _databaseConnection;
			}
		}
		#endregion

		#region Primary SQL Statements
		/// <summary>
		/// Gets all rows for the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static List<T> GetRows<T>() where T : class
		{
			List<T> objects = new List<T>();
			PropertyInfo[]? properties = typeof(T).GetProperties();

			string query = $"SELECT * FROM {typeof(T).Name}";
			SqlCommand command = new SqlCommand(query, DatabaseConnection);
			SqlDataReader? reader;

			try
			{
				reader = command.ExecuteReader();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return objects;
			}

			while (reader.Read())
			{
				T? obj = (T)Activator.CreateInstance(typeof(T));

				if (obj == null)
					return objects;

				for (int i = 0; i < properties?.Length; i++)
				{
					Type type = typeof(T);
					PropertyInfo? propertyInfo = type.GetProperty(properties[i].Name);

					propertyInfo?.SetValue(obj, Convert.ChangeType(reader[properties[i].Name], propertyInfo.PropertyType));
				}

				objects.Add(obj);
			}

			return objects;
		}

		/// <summary>
		/// Inserts the object into the Database
		/// </summary>
		/// <typeparam name="T">The object type to be inserted into the DB</typeparam>
		/// <param name="obj">The object to be inserted into the Database</param>
		/// <returns>Whether the query completed successfully</returns>
		public static bool Insert<T>(T obj) where T : class
		{
			int rowsAffected = 0;
			string query = GenerateInsertQuery(obj);
			SqlCommand command = new SqlCommand(query, DatabaseConnection);

			try
			{
				rowsAffected = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			command.Dispose();

			return rowsAffected == 1;
		}

		/// <summary>
		/// Updates the object with the new values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns>Whether the query completed successfully</returns>
		public static bool Update<T>(T obj) where T : class
		{
			int rowsAffected = 0;
			string query = GenerateUpdateQuery(obj);
			SqlCommand command = new SqlCommand(query, DatabaseConnection);

			try
			{
				rowsAffected = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			command.Dispose();

			return rowsAffected == 1;
		}

		/// <summary>
		/// Deletes the given object from the Database
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object to delete</param>
		/// <returns>Whether the query completed successfully</returns>
		public static bool Delete<T>(T obj) where T : class
		{
			int rowsAffected = 0;
			string query = GenerateDeleteQuery(obj);
			SqlCommand command = new SqlCommand(query, DatabaseConnection);

			try
			{
				rowsAffected = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			command.Dispose();

			return rowsAffected > 0;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Generates a query to insert the given object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		private static string GenerateInsertQuery<T>(T obj) where T : class
		{
			string query = $"INSERT INTO {typeof(T).Name} (";
			// get properties of the object
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// construct query containing all the properties of the object
			for (int i = 0; i < properties.Length; i++)
			{
				query += $"{properties[i].Name}";

				if (i != properties.Length - 1)
					query += ", ";
			}

			query += ") VALUES (";

			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];
				if (property.PropertyType == typeof(string) || property.PropertyType == typeof(char))
				{
					query += $"'{property.GetValue(obj)}'";
				}
				else
				{
					query += $"{property.GetValue(obj)}";
				}

				if (i != properties.Length - 1)
					query += ", ";
			}

			query += ");";

			return query;
		}

		/// <summary>
		/// Generates an query to update the given object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		private static string GenerateUpdateQuery<T>(T obj) where T : class
		{
			string query = $"UPDATE {typeof(T).Name} SET ";
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// construct the assignment portion of the query (SET)
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].PropertyType == typeof(string) || properties[i].PropertyType == typeof(char))
				{
					query += $"{properties[i].Name} = '{properties[i].GetValue(obj)}'";
				}
				else
				{
					query += $"{properties[i].Name} = {properties[i].GetValue(obj)}";
				}

				if (i != properties.Length - 1)
					query += ", ";
			}

			// construct the identifier portion of the query (WHERE)
			object? id = properties?.Where(p => p?.Name == "Id")?.First()?.GetValue(obj);

			if (id == null)
				throw new NullReferenceException("No property with name \"Id\" was found");

			query += $" WHERE Id = {id}";

			return query;
		}

		/// <summary>
		/// Generates a query to delete the given object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		private static string GenerateDeleteQuery<T>(T obj) where T : class
		{
			string query = $"DELETE FROM {typeof(T).Name} WHERE Id = ";
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			object? id = properties?.Where(p => p?.Name == "Id")?.First().GetValue(obj);

			if (id == null)
				throw new NullReferenceException("No property with name \"Id\" was found");

			query += $"{id};";

			return query;
		}
		#endregion
	}
}