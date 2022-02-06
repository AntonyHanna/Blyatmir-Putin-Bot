using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.Models.Common.Configuration;
using Microsoft.Data.Sqlite;
using System.Reflection;

namespace BlyatmirPutin.DataAccess.Database
{
	public class DatabaseHelper
	{
		#region Properties

		#endregion

		#region Delegates
		delegate string DatabaseObjectOperation<T>(T obj) where T : class;
		#endregion

		#region Public Methods
			#region Generic Methods
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
			SqliteCommand command = new SqliteCommand(query, DatabaseManager.DatabaseConnection);
			SqliteDataReader? reader;

			try
			{
				reader = command.ExecuteReader();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				return objects;
			}

			while (reader.Read())
			{
				T? obj = (T?)Activator.CreateInstance(typeof(T));

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
			int affectedRows = PerformDatabaseOperation(obj, GenerateInsertQuery);
			return affectedRows == 1;
		}

		/// <summary>
		/// Updates the object with the new values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns>Whether the query completed successfully</returns>
		public static bool Update<T>(T obj) where T : class
		{
			int affectedRows = PerformDatabaseOperation(obj, GenerateUpdateQuery);
			return affectedRows == 1;
		}

		/// <summary>
		/// Deletes the given object from the Database
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object to delete</param>
		/// <returns>Whether the query completed successfully</returns>
		public static bool Delete<T>(T obj) where T : class
		{
			int affectedRows = PerformDatabaseOperation(obj, GenerateDeleteQuery);
			return affectedRows > 1;
		}
			#endregion

			#region Non-Generic Methods
		public static bool ExecuteRawSql(string sql)
		{
			SqliteCommand createCommand = new SqliteCommand(sql, DatabaseManager.DatabaseConnection);

			try
			{
				createCommand.ExecuteReader();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				return false;
			}

			createCommand.Dispose();

			return true;
		}

		public static int ExecuteRawSqlNonQuery(string sql)
		{
			int count = 0;
			SqliteCommand createCommand = new SqliteCommand(sql, DatabaseManager.DatabaseConnection);

			try
			{
				count = createCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
			}

			createCommand.Dispose();

			return count;
		}

		/// <summary>
		/// Checks the Table Definitions for the specified type
		/// and creates the table with the resulting SQL
		/// </summary>
		/// <param name="type">The type to provide to the <see cref="TableDefinitions.Mappings"/> dictionary</param>
		/// <returns>Whether the operation completed successfully</returns>
		public static bool CreateTable(Type type)
		{
			string tableCreationQuery;
			try
			{
				tableCreationQuery = TableDefinitions.Mappings[type];
				Logger.LogDebug($"Created table for key {type.Name}");
			}
			catch
			{
				Logger.LogWarning($"There's no type {type.Name} in the dictionary, skipping...");
				return false;
			}

			return ExecuteRawSql(tableCreationQuery);
		}

		/// <summary>
		/// Creates the tables defined in <see cref="TableDefinitions.Mappings"/>
		/// </summary>
		public static void EnsureTablesCreated()
		{
			for (int i = 0; i < TableDefinitions.Mappings.Count; i++)
			{
				CreateTable(TableDefinitions.Mappings.ElementAt(i).Key);
			}
		}
			#endregion

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
			{
				Logger.LogCritical("No property with name \"Id\" was found");
				throw new NullReferenceException("No property with name \"Id\" was found");
			}

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
			{
				Logger.LogCritical("No property with name \"Id\" was found");
				throw new NullReferenceException("No property with name \"Id\" was found");
			}

			query += $"{id};";

			return query;
		}

		/// <summary>
		/// Performs the query returned by the <paramref name="operation"/> delegate
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object to use for the operation</param>
		/// <param name="operation">The SQL operation to perform</param>
		/// <returns>The number of rows affected by the operation</returns>
		private static int PerformDatabaseOperation<T>(T obj, DatabaseObjectOperation<T> operation) where T : class
		{
			string query = operation(obj);
			return ExecuteRawSqlNonQuery(query);
		}
		#endregion
	}
}