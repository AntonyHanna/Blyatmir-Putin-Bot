namespace blyatmir_putin.Core.Interfaces
{
	public interface IAppSettings
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
