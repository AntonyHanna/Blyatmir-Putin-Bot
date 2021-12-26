﻿using BlyatmirPutin.DataAccess.Xml;
using BlyatmirPutin.Models.Common.Configuration;
using BlyatmirPutin.Models.Interfaces;

namespace BlyatmirPutin.Models.Factories
{
	public static class ConfigurationFactory
	{
		public static IConfiguration? Create()
		{
			const string path = ".\\Config.xml";
			IConfiguration? config = default;

			if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
			{
				Console.WriteLine("Loading docker config...");
				config = DockerConfiguration.Load();	
			}
			else
			{
				Console.WriteLine("Loading desktop config...");
				DesktopConfiguration? desktopConfig = new DesktopConfiguration();
				// ensures the config has been generated
				// and depending on the status performs the
				// appropriate action
				switch (XmlManager.EnsureCreated<DesktopConfiguration>(path))
				{
					case XmlManager.FileStatus.AlreadyExists:
						if (XmlManager.Read(ref desktopConfig, path))
						{
							Console.WriteLine("Successfully loaded the config.");
							config = desktopConfig;
						}
						break;

					case XmlManager.FileStatus.Created:
					case XmlManager.FileStatus.Error:
						Console.WriteLine("There was either an error loading " +
							"the file or the file could not be found.\n\n" +
							"Check to make sure that Config.xml has been" +
							" filled out appropriately...");
						Environment.Exit(-1);
						break;
				}
			}

			return config;
		}
	}
}