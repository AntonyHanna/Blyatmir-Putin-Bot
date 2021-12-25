using Microsoft.Data.Sqlite;

namespace BlyatmirPutin.DataAccess
{
	/// <summary>
	/// Manages the database connection
	/// </summary>
	public static class DatabaseManager
	{
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
					_databaseConnection?.Open();
					AppDomain.CurrentDomain.ProcessExit += DisposeDatabaseConnection;
				}

				return _databaseConnection;
			}
		}

		/// <summary>
		/// Dispose of the database connection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void DisposeDatabaseConnection(object? sender, EventArgs e)
		{
			Console.WriteLine("Disposing of database connection...");
			_databaseConnection?.Close();
			_databaseConnection?.Dispose();
			_databaseConnection = null;
		}
	}
}
