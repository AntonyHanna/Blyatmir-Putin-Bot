using NetCord.Gateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlyatmirPutin.Logic.Events
{

	public class CustomVoiceStateUpdate
	{
		public static event EventHandler<CustomVoiceStateUpdateEventArgs> VoiceStateUpdateTriggered;

		public static void OnVoiceStateUpdateTriggered(CustomVoiceStateUpdateEventArgs e)
		{
			VoiceStateUpdateTriggered.Invoke(typeof(CustomVoiceStateUpdate), e);
		}

	}

	public class CustomVoiceStateUpdateEventArgs : EventArgs
	{
        public GatewayClient Client { get; set; }

        public VoiceState VoiceState { get; set; }
    }
}
