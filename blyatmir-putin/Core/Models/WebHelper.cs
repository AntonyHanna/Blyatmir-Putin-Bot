using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace blyatmir_putin.Core.Models
{
	/*
	 * Made to contain things that help with web requests
	 */

	public static class WebHelper
	{
		public static string GetRequest(string url)
		{
			string json = string.Empty;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				json = reader.ReadToEnd();
			}

			return json;
		}
	}
}
