using ElCheapo.Generics;
using System;

namespace Blyatmir_Putin_Bot.Model
{
	public class LocalGame : Game
	{
		public int ID { get; set; }

		public bool Posted { get; set; }

		public LocalGame() : base() { }

		public LocalGame(Game game) 
			: base(game.StartDate, game.EndDate, game.Name, game.BannerUri, game.PosterUri, game.Description, game.Developer, game.Publisher)
		{
			this.Posted = false;
		}

		public LocalGame(DateTime startDate, DateTime endDate, string name, string bannerUri, string posterUri, string description, string developer, string publisher) 
			: base(startDate, endDate, name, bannerUri, posterUri, description, developer, publisher)
		{
			this.Posted = false;
		}	
	}
}
