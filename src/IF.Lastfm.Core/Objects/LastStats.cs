using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
	public class LastStats : ILastfmObject
	{
		#region Properties

		public int Listeners { get; set; }
		public int Plays { get; set; }
		public int? UserPlayCount { get; set; }

		#endregion

		internal static LastStats ParseJToken(JToken token)
		{
			var stats = new LastStats
			{
				Listeners = token.Value<int>("listeners"),
				Plays = token.Value<int>("playcount"),
				UserPlayCount = token.Value<int>("userplaycount")
			};

			return stats;
		}
	}
}
