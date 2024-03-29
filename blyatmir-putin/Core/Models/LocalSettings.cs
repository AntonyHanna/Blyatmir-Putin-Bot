﻿using System.IO;
using System.Xml;
using System.Xml.Serialization;
using blyatmir_putin.Core.Interfaces;

namespace blyatmir_putin.Core.Models
{
	public class LocalSettings : IAppSettings
	{
		public string Token { get; set; }

		public string Prefix { get; set; }

		public string RootDirectory { get; set; } = "config/";

		public string Activity { get; set; }

		public LocalSettings()
		{
			string path = $"{this.RootDirectory}Settings.xml";
			bool initialized = File.Exists(path);

			if(!initialized)
			{
				LocalSettings initialisationList = this;

				using (StreamWriter sr = new StreamWriter(path))
				using (XmlWriter writer = XmlWriter.Create(sr, XmlSettings()))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(LocalSettings));

					this.Token = " ";
					this.Prefix = " ";
					this.Activity = " ";

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

				return true;
			}

			return false;
		}

		/// <summary>
		/// The settings that should be used by all XmlWriters
		/// </summary>
		/// <returns></returns>
		public static XmlWriterSettings XmlSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();

			settings.Indent = true;
			settings.IndentChars = "    ";
			settings.CloseOutput = true;

			return settings;
		}
	}
}
