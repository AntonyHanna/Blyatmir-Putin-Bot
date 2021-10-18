namespace blyatmir_putin.Core.Interfaces
{
	public interface IAppSettings
	{
		public string Token { get; set; }
		public string Prefix { get; set; }
		public string RootDirectory { get; set; }
		public string Activity { get; set; }
	}
}
