using ElCheapo.Generics;
using System;

namespace blyatmir_putin.Core.Models
{
	public class LocalGame : Game
	{
		public int ID { get; set; }

		public bool Posted { get; set; }

		public LocalGame() : base() { }

		public LocalGame(Game game) 
			: base(game.StartDate, game.EndDate, game.Name, game.BannerUri, game.PosterUri, game.Description, game.Developer, game.Publisher, game.StorePageUrl)
		{
			this.Posted = false;
		}

		public LocalGame(DateTime startDate, DateTime endDate, string name, string bannerUri, string posterUri, string description, string developer, string publisher, string storePageUri) 
			: base(startDate, endDate, name, bannerUri, posterUri, description, developer, publisher, storePageUri)
		{
			this.Posted = false;
		}	
	}
}
