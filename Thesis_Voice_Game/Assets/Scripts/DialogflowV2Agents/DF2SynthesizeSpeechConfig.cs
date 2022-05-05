using Newtonsoft.Json;

namespace Syrus.Plugins.DFV2Client
{
	[JsonObject]
	public class DF2SynthesizeSpeechConfig
	{
		////"speakingRate": 0.9,
		//[JsonProperty]
		//public string speakingRate { get; set; }

		////"pitch": -2,
		//[JsonProperty]
		//public string pitch { get; set; }

		[JsonProperty]
		public DF2VoiceSelectionParams Voice { get; set; }
	}
}
