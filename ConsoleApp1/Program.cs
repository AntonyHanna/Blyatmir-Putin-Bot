﻿using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.Logic.Discord;
using BlyatmirPutin.Models.Factories;
using BlyatmirPutin.Models.Interfaces;

public class Program
{
	public DiscordManager? DiscordManager;

	public static Task Main(string[] args)
		=> new Program().MainAsync();

	public async Task MainAsync()
	{
		// handle application exit signal
		AppDomain.CurrentDomain.ProcessExit += (obj, e) => { 
			Cleanup(obj, e);
		};

		// handle Ctrl + c
		Console.CancelKeyPress += (obj, e) =>
		{
			// send application exit signal
			Environment.Exit(0);
		};

		DiscordManager = new DiscordManager();
		IConfiguration? config = ConfigurationFactory.Create();

		await DiscordManager.ConnectAsync(config);

		await Task.Delay(-1);
	}

	private async void Cleanup(object? sender, EventArgs e)
	{
		Logger.LogInfo("Preparing to exit application...");

		if (DiscordManager != null)
		{
			Logger.LogInfo("Disconnecting from discord...");
			await DiscordManager.DisconnectAsync();

			Logger.LogDebug("Disposing discord manager...");
			DiscordManager.Dispose();
		}

		Logger.LogDebug("Disposing of database connection...");
		// need to disconnect the database here

		Logger.LogDebug("Resetting console color...");
		Console.ResetColor();

		Logger.LogInfo("Exiting application now...");
	}
}