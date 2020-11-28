using Blyatmir_Putin_Bot.Core.Models;
using System;
using System.IO;

namespace Blyatmir_Putin_Bot.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

	public class DirectoryRequiredAttribute : Attribute
	{
		public string DirectoryPath { get; set; }

		public DirectoryRequiredAttribute(string directoryPath)
		{
			this.DirectoryPath = directoryPath;

			EnsureCreated(this.DirectoryPath);
		}

		private static void EnsureCreated(string path)
		{
			if(!Directory.Exists(path))
			{
				Logger.Warning($"Directory [{path}] doesn't exist, creating now...");
				Directory.CreateDirectory(path);
			}
		}
	}
}
