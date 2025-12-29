using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Logic.Discord;
using BlyatmirPutin.Models.Factories;
using BlyatmirPutin.Models.Interfaces;
using NetCord;
using NetCord.Gateway;
using System;
using System.IO;
using System.Threading.Tasks;

public class Program
{
	public DiscordManager? DiscordManager;

	public static Task Main(string[] args)
		=> new Program().MainAsync();

	public async Task MainAsync()
	{
		// handle application exit signal
		AppDomain.CurrentDomain.ProcessExit += (obj, e) =>
		{
			Cleanup(obj, e);
		};

		// handle Ctrl + c
		Console.CancelKeyPress += (obj, e) =>
		{
			// send application exit signal
			Environment.Exit(0);
		};

		Directory.CreateDirectory("./data/user-intros");

		DiscordManager = new DiscordManager();
		IConfiguration? config = ConfigurationFactory.Create();

		if (config == null)
		{
			Logger.LogCritical("Failed to find to generate a config...");
			Environment.Exit(-1);
		}

		if (string.IsNullOrWhiteSpace(config.Token) || string.IsNullOrEmpty(config.Token))
		{
			Logger.LogCritical("Bot token was left empty, now exiting application...");
			Environment.Exit(-1);
		}

		if (!DatabaseManager.ConnectDatabase())
		{
			Logger.LogCritical("Failed to connect to database, now exiting application...");
			Environment.Exit(-1);
		}

		if (DatabaseManager.IsInitialised)
		{
			Logger.LogWarning($"No database found, creating tables now...");
			DatabaseHelper.EnsureTablesCreated();
		}
		
		Logger.LogInfo("Connected to database successfully");

		await DiscordManager.Setup(config);
		await DiscordManager.Start();
		await DiscordManager.UpdatePresenceAsync(new PresenceProperties(UserStatusType.Online)
		{
			Activities = [
				new UserActivityProperties(config.Activity, UserActivityType.Streaming)
				{
					Url = @"https://www.youtube.com/watch?v=WY0F3JhLE5o",
					State = "Do you come from a land down under"
				}
			]
		});
		
		await Task.Delay(-1);
	}

	private async void Cleanup(object? sender, EventArgs e)
	{
		Logger.LogInfo("Preparing to exit application...");

		if (DiscordManager != null)
		{
			Logger.LogInfo("Disconnecting from discord...");
			// TODO
			//await DiscordManager.DisconnectAsync();

			Logger.LogDebug("Disposing discord manager...");
			DiscordManager.Dispose();
		}

		Logger.LogDebug("Disconnecting and disposing of database connection...");
		DatabaseManager.DisconnectDatabase();
		DatabaseManager.Dispose();

		Logger.LogDebug("Resetting console color...");
		Console.ResetColor();

		Logger.LogInfo("Exiting application now...");
	}
}