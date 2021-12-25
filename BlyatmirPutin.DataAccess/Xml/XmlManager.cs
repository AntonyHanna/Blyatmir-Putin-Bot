using System.Xml;
using System.Xml.Serialization;

namespace BlyatmirPutin.DataAccess.Xml
{
	public class XmlManager
	{
		/// <summary>
		/// Ensures that the file has been created. Does nothing if it exists
		/// </summary>
		/// <typeparam name="T">The type of object to create the file for</typeparam>
		/// <param name="path">Where to store the resulting XML file</param>
		/// <param name="template">The object to use to prefill the file with</param>
		/// <returns>Returns false if no file was created, true otherwise</returns>
		public static FileStatus EnsureCreated<T>(string path, T? template = null) where T : class
		{
			if(File.Exists(path))
			{
				return FileStatus.AlreadyExists;
			}

			try
			{
				T? obj = template ?? (T?)Activator.CreateInstance(typeof(T));

				using (StreamWriter sr = new StreamWriter(path))
				using (XmlWriter writer = XmlWriter.Create(sr, WriterSettings()))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(T));

					serializer.Serialize(writer, obj);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return FileStatus.Error;
			}


			return FileStatus.Created;
		}

		/// <summary>
		/// Reads the file and deserializes it into <typeparamref name="obj"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object to be filled with data upon deserialization</param>
		/// <param name="path">The path of where the file can be found</param>
		/// <returns></returns>
		public static bool Read<T>(ref T? obj, string path) where T : class
		{
			if(!File.Exists(path))
			{
				Console.WriteLine($"The file at path [{path}] could not be found...");
				return false;
			}

			try
			{
				using (XmlReader reader = XmlReader.Create(path))
				{
					XmlSerializer serialize = new XmlSerializer(typeof(T));

					T? deserializedObj = (T?)serialize.Deserialize(reader);

					if(deserializedObj == null)
					{
						Console.WriteLine("Deserialized object was null...");
						return false;
					}
					else
					{
						obj = deserializedObj;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// The settings to be used for all Xml writing
		/// </summary>
		/// <returns></returns>
		internal static XmlWriterSettings WriterSettings() => new XmlWriterSettings
		{
			Indent = true,
			IndentChars = "    ",
			CloseOutput = true,
		};

		public enum FileStatus
		{
			Created,
			AlreadyExists,
			Error
		};

	}
}
