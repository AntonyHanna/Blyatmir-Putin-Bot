using System;

namespace blyatmir_putin.Core.Models
{
	public static class Logger
	{
		private static object _messageLock = new object();

		public static void Debug(string message)
		{
			Print(message);
		}

		public static void Warning(string message)
		{
			Print(message, ConsoleColor.Yellow);
		}

		public static void Critical(string message)
		{
			Print(message, ConsoleColor.Red);
		}

		private static void Print(string message, ConsoleColor color)
		{
			lock(_messageLock)
			{
				Console.ForegroundColor = color;
				DateTime stamp = DateTime.Now;
				Console.Write($"[{stamp.ToShortDateString()} {stamp.ToLongTimeString()}]\t{message}\n");
				Console.ResetColor();
			}
		}

		private static void Print(string message)
		{
			lock(_messageLock)
			{
				DateTime stamp = DateTime.Now;
				Console.Write($"[{stamp.ToShortDateString()} {stamp.ToLongTimeString()}]\t{message}\n");
			}
		}
	}
}
