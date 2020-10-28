using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blyatmir_Putin_Bot.Model
{
	public static class SettingsFactory
	{
		public static bool IsDocker
		{
			get
			{
				if (Environment.GetEnvironmentVariable("ISDOCKER") != null)
				{
					return true;
				}

				return false;
			}
		}

		public static IAppSettings Create()
		{
			if(SettingsFactory.IsDocker)
			{
				return new EnvironmentSettings();
			}
			else
			{
				LocalSettings settings = new LocalSettings();
				settings.LoadSettings();

				return settings;
			}
		}
	}
}
