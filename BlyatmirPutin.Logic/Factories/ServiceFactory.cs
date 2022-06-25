using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlyatmirPutin.Logic.Factories
{
	public static class ServiceFactory
	{
		private static ServiceProvider? _serviceProvider;
		public static IServiceProvider? ServiceProvider
		{
			get
			{
				if(_serviceProvider == null)
				{
					_serviceProvider = new ServiceCollection().BuildServiceProvider();
				}

				return _serviceProvider;
			}
		}
	}
}
