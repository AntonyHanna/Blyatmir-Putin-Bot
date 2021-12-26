using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BlyatmirPutin.Common.Logging
{
	public static class Logger
	{
		private static bool IsConsoleAvailable
		{
			get
			{
				return Console.LargestWindowWidth != 0;
			}
		}

		public static void LogInfo(string message, [CallerMemberName] string callerName = "")
		{
			if(IsConsoleAvailable)
				Console.WriteLine(new MessageLog(message, MessageLog.LogLevel.Info, DetermineForegroundColor(), callerName));
		}

		public static void LogDebug(string message, [CallerMemberName] string callerName = "")
		{
			if (IsConsoleAvailable)
				Console.WriteLine(new MessageLog(message, MessageLog.LogLevel.Debug, ConsoleColor.Green, callerName));
		}

		public static void LogWarning(string message, [CallerMemberName] string callerName = "")
		{
			if (IsConsoleAvailable)
				Console.WriteLine(new MessageLog(message, MessageLog.LogLevel.Warning, ConsoleColor.Yellow, callerName));
		}

		public static void LogError(string message, [CallerMemberName] string callerName = "")
		{
			if (IsConsoleAvailable)
				Console.Error.WriteLine(new MessageLog(message, MessageLog.LogLevel.Error, ConsoleColor.Red, callerName));
		}

		public static void LogCritical(string message, [CallerMemberName] string callerName = "")
		{
			if (IsConsoleAvailable)
				Console.Error.WriteLine(new MessageLog(message, MessageLog.LogLevel.Critical, ConsoleColor.Magenta, callerName));
		}
		
		private static ConsoleColor DetermineForegroundColor()
		{
			if(Console.BackgroundColor == ConsoleColor.Black)
			{
				return ConsoleColor.White;
			}

			return ConsoleColor.Black;
		}
	}
}
