namespace BlyatmirPutin.Common.Logging
{
	public class MessageLog
	{
		private static DateTime _timestamp;
		private static DateTime Timestamp
		{
			get
			{
				if (_timestamp == default)
				{
					_timestamp = DateTime.Now;
				}

				return _timestamp;
			}
		}

		public string Message { get; set; }
		public LogLevel Severity { get; set; }

		public MessageLog(string message, LogLevel severity, ConsoleColor color, string callerName = "")
		{
			this.Severity = severity;
			this.Message = FormatMessage(message, color, severity.ToString(), callerName);
		}

		private static string FormatMessage(string message, ConsoleColor color = default, string logLevel = "", string callerName = "")
		{
			Console.ForegroundColor = color;
			return string.Format($"{Timestamp,-10:dd MMM yyyy - hh:mm tt}  {logLevel,-8}  {callerName,-10}  {message}", message);
		}

		public enum LogLevel
		{
			Verbose,
			Info,
			Debug,
			Warning,
			Error,
			Critical
		}

		public override string ToString()
		{
			return Message;
		}
	}
}
