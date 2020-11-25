using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blyatmir_Putin_Bot.Model
{
	public static class AttributeLoader
	{
		public static void LoadCustomAttributes()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (Type type in types)
			{
				IEnumerable<string> dir = type.GetCustomAttributes(typeof(DirectoryRequiredAttribute), true).Cast<DirectoryRequiredAttribute>().Select(x => x.DirectoryPath);
			}
		}
	}
}
