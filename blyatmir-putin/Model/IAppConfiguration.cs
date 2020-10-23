using System;
using System.Collections.Generic;
using System.Text;

namespace Blyatmir_Putin_Bot.Model
{
	public interface IAppConfiguration
	{
		public string Token { get; set; }
		public string Prefix { get; set; }
		public string RootDirectory { get; set; }
		public string Activity { get; set; }
		public string DockerIP { get; set; }
		public string ServerUser { get; set; }
		public string ServerPassword { get; set; }
	}
}
