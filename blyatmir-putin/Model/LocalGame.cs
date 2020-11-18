using ElCheapo.Generics;
using System;

namespace Blyatmir_Putin_Bot.Model
{
	public class LocalGame : Game
	{
		public bool Posted { get; set; }

		public LocalGame() : base() { }

		public LocalGame(Game game) : base(game.EffectiveDate, game.Name, game.Description, game.Developer, game.Publisher)
		{
			this.Posted = false;
		}

		public LocalGame(DateTime effectiveDate, string name, string description, string developer, string publisher) : base(effectiveDate, name, description, developer, publisher)
		{
			this.Posted = false;
		}	
	}
}
