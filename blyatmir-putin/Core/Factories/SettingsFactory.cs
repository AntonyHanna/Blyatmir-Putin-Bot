using Blyatmir_Putin_Bot.Core.Interfaces;
using Blyatmir_Putin_Bot.Core.Models;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Core.Factories
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

		public static async Task<IAppSettings> CreateAsync()
		{
			IAppSettings settings = default;
			await Task.Run(() =>
			{
				if (SettingsFactory.IsDocker)
				{
					settings = new EnvironmentSettings();
				}
				else
				{
					LocalSettings localSettings = new LocalSettings();
					localSettings.LoadSettings();

					settings = localSettings;
				}
			});

			return settings;
		}
	}
}
