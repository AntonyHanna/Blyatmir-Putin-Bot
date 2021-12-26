using BlyatmirPutin.Common.Logging;
using Microsoft.Data.Sqlite;

namespace BlyatmirPutin.DataAccess.Database
{
	/// <summary>
	/// Manages the database connection
	/// </summary>
	public static class DatabaseManager
	{
		#region Properties
		private static SqliteConnection? _databaseConnection;

		/// <summary>
		/// Instance of the database connection
		/// </summary>
		internal static SqliteConnection? DatabaseConnection
		{
			get
			{
				if (_databaseConnection == null)
				{
					_databaseConnection = new SqliteConnection("Data Source=Guido.sqlite;");
					AppDomain.CurrentDomain.ProcessExit += DisposeDatabaseConnection;
				}

				return _databaseConnection;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Attempt to connect to the database
		/// </summary>
		/// <returns>Whether the operation complete successfully</returns>
		public static bool ConnectDatabase()
		{
			bool success = true;
			try
			{
				DatabaseConnection?.Open();
			}
			catch (Exception ex)
			{
				Logger.LogCritical(ex.Message);
				Environment.Exit(-1);
				success = false;
			}

			return success;
		}

		/// <summary>
		/// Attempt to disconnect from the database
		/// </summary>
		/// <returns>Whether the operation complete successfully</returns>
		public static bool DisconnectDatabase()
		{
			bool success = true;
			try
			{
				DatabaseConnection?.Close();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				success = false;
			}

			return success;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Dispose of the database connection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void DisposeDatabaseConnection(object? sender, EventArgs e)
		{
			Logger.LogDebug("Disposing of database connection...");
			DatabaseConnection?.Close();
			DatabaseConnection?.Dispose();
			_databaseConnection = null;
		}
		#endregion
	}
}
