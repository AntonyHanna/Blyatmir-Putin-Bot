using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Model
{
	public class Container : PersistantStorage<Container>
	{
		public static readonly List<Container> ContainerList = new List<Container>(Read());

		public enum ContainerPermissions
		{
			root = 0,   // full access to containers
			user = 1,   // limited access to containers
			jack = 2    // no access to containers
		}
		public string Id { get; set; }
		public string Name { get; set; }
		public string Tag { get; set; }
		public string Image { get; set; }
		public ContainerPermissions PermisssionLevel { get; set; } = ContainerPermissions.root;

		public Container()
		{
			this.Id = default;
			this.Name = default;
			this.Image = default;
			this.Tag = default;
		}

		public Container(string containerId)
		{
			this.Id = containerId;
			this.Name = GetContainerName(this.Id);
			this.Image = GetContainerImage(this.Id);
			this.Tag = GetContainerTag(this.Id);

			ContainerList.Add(this);
			Write(ContainerList);
		}

		/// <summary>
		/// Generate any containers that are missing from file
		/// </summary>
		/// <returns></returns>
		public static Task GenerateMissingContiners()
		{
			Logger.Debug("Attempting to generate missing containers");

			if (!SshController.IsSshEnabled)
			{
				Logger.Warning("Aborting... SSH is NOT enabled");
				return Task.CompletedTask;
			}
				

			if (!Directory.Exists(Startup.AppConfig.RootDirectory))
			{
				Logger.Warning("Root directory not found. Creating root directory...");
				Directory.CreateDirectory(Startup.AppConfig.RootDirectory);
			}

			string[] containerIds = GetAllContainerIds();

			//loop through all the guilds
			for (int j = 0; j < containerIds.Count(); j++)
			{
				
				// check for container update
				// trigger an event if updated with the container info
				// 

				bool isPresent = false;

				InitializeStorage();


				if (ContainerList.Count() > 0)
					foreach (var cont in ContainerList)
						if (containerIds[j] == cont.Id)
							isPresent = true;

				// for the ones not present add them to data
				if (!isPresent)
				{
					new Container(containerIds[j]);
					Write(ContainerList);
				}
			}
			Logger.Debug("Finished generating container data");
			return Task.CompletedTask; 
		}

		public static string[] GetAllContainerIds()
		{
			// returns a single string with all container ids
			// then splits the string into multiple strings
			// does not return any strings that are empty
			return SshController.SshClient.RunCommand("docker ps -a --format {{.ID}}").Result.Split("\n", StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Get all containers on the server
		/// </summary>
		/// <returns></returns>
		public static Container[] GetAllContainers()
		{
			return ContainerList.ToArray();
		}

		/// <summary>
		/// Returns a container with a matching containerId
		/// </summary>
		/// <param name="containerId">The ID of the container you're looking for</param>
		/// <returns></returns>
		public static Container GetContainerById(string containerId)
		{
			var result = from container in ContainerList
						 where container.Id == containerId
						 select container;

			return result.First();
		}

		public static Container GetContainerByName(string containerName)
		{
			// linq query to get container from Containers list
			var result = from container in ContainerList
						 where container.Name == containerName
						 select container;

			return result.First();
		}

		/// <summary>
		/// Get all info on a single container
		/// </summary>
		/// <param name="containerId">The ID of the container you're trying to access</param>
		/// <returns></returns>
		public static string GetContainerInfo(string containerId)
		{
			return SshController.SshClient.RunCommand($"docker ps -a --filter \"id={containerId}\"").Result;
		}

		public static string[] GetContainerPorts(string containerId)
		{
			// returns a single string with all container ports
			// then splits the string into multiple strings
			// does not return any strings that are empty
			string[] ports = SshController.SshClient.RunCommand(string.Format("docker ps -a --filter \"id = {0}\" --format {{.Ports}}", containerId)).Result.Split("\n", StringSplitOptions.RemoveEmptyEntries);

			// sanatise the ports to only show the port people should use
			for (int i = 0; i < ports.Length; i++)
			{
				// gets the portion of the string that matches the regex then removes the trailing -
				ports[i] = Regex.Match(ports[0], "[0-9]{0,}-").Value.Replace("-", "").Trim();
			}

			return ports;
		}

		public static string GetContainerCreation(string containerId)
		{
			return SshController.SshClient.RunCommand(string.Format("docker ps -a --filter \"id = {0}\" --format {{.RunningFor}}", containerId)).Result.Trim();
		}

		public static string GetContainerCurrentRunState(string containerId)
		{
			return SshController.SshClient.RunCommand($"docker ps -a --filter \"id = {containerId}\" --format {{{{.Status}}}}").Result.Trim();
		}

		/// <summary>
		/// Gets the container Name
		/// </summary>
		/// <param name="containerId">The ID of the container you're trying to access</param>
		/// <returns></returns>
		public static string GetContainerName(string containerId)
		{
			return SshController.SshClient.RunCommand($"docker ps -a --filter \"id = {containerId}\" --format {{{{.Names}}}}").Result.Trim('\n');
		}

		public static string GetContainerImage(string containerId)
		{
			string unsanitisedString = SshController.SshClient.RunCommand($"docker ps -a --filter \"id = {containerId}\" --format {{{{.Image}}}}").Result.Trim('\n');
			return Regex.Replace(unsanitisedString, ":[A-z]{0,}", "");
		}

		public static string GetContainerTag(string containerId)
		{
			string unsanitisedString = SshController.SshClient.RunCommand($"docker ps -a --filter \"id = {containerId}\" --format {{{{.Image}}}}").Result.Trim('\n');

			// some containers don't have a branch specified and default to :latest
			if (!unsanitisedString.Contains(":"))
				unsanitisedString += ":latest";

			return Regex.Replace(unsanitisedString, ".{0,}/.{0,}:", "");
		}

		public static void SetContainerPermissionLevel(string containerId, ContainerPermissions newContainerPermissions)
		{
			GetContainerById(containerId).PermisssionLevel = newContainerPermissions;

			Write(ContainerList);
		}

		public static bool IsValidContainer(string containerName)
		{
			for (int i = 0; i < Container.ContainerList.Count; i++)
				if (Container.ContainerList[i].Name == containerName)
					return true;
			return false;
		}

		public static bool UserHasSufficientPriveleges(Container container, User user)
		{
			if (container.PermisssionLevel >= user.ContainerAccessLevel)
				return true;
			return false;
		}
	}
}
