using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace Blyatmir_Putin_Bot.Model
{
	public class LocalSettings : IAppSettings
	{
		public string Token { get; set; }

		public string Prefix { get; set; }

		public string RootDirectory { get; set; } = "config/";

		public string Activity { get; set; }

		public string DockerIP { get; set; }

		public string ServerUser { get; set; }

		public string ServerPassword { get; set; }

		public LocalSettings()
		{
			string path = $"{this.RootDirectory}Settings.xml";
			bool initialized = File.Exists(path);

			if(!initialized)
			{
				if(!Directory.Exists(this.RootDirectory))
				{
					Logger.Warning("No Settings file found. Attempting to create the file.");
					Directory.CreateDirectory(this.RootDirectory);
					Logger.Warning("Settings file has been created.");
				}
		
				LocalSettings initialisationList = this;

				using (StreamWriter sr = new StreamWriter(path))
				using (XmlWriter writer = XmlWriter.Create(sr, PersistantStorage<LocalSettings>.XmlSettings()))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(LocalSettings));

					this.Token = " ";
					this.Prefix = " ";
					this.Activity = " ";
					this.DockerIP = " ";
					this.ServerUser = " ";
					this.ServerPassword = " ";


					serializer.Serialize(writer, initialisationList);
				}
			}
		}

		public bool LoadSettings()
		{
			string path = $"{this.RootDirectory}Settings.xml";
			LocalSettings settings;

			using (XmlReader reader = XmlReader.Create(path))
			{
				XmlSerializer serialize = new XmlSerializer(typeof(LocalSettings));
				settings = serialize.Deserialize(reader) as LocalSettings;
			}

			if (settings != null)
			{
				this.Token = settings.Token;
				this.Prefix = settings.Prefix;
				this.Activity = settings.Activity;
				this.DockerIP = settings.DockerIP;
				this.ServerUser = settings.ServerUser;
				this.ServerPassword = settings.ServerPassword;

				return true;
			}

			return false;
		}
	}
}
