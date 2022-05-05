using Newtonsoft.Json;

namespace Syrus.Plugins.DFV2Client
{
	[JsonObject]
	public class DF2VoiceSelectionParams
	{
		[JsonProperty]
		public string ssmlGender { get; set; }
	}
}
