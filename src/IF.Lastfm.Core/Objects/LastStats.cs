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
            // Check for user play count.
            var userPlayCountStr = token.Value<string>("userplaycount");
            int userPlayCount;
            bool hasUserPlayCount = int.TryParse(userPlayCountStr, out userPlayCount);

            var stats = new LastStats
            {
                Listeners = token.Value<int>("listeners"),
                Plays = token.Value<int>("plays"),
                UserPlayCount = hasUserPlayCount ? userPlayCount : default(int?)
            };

            return stats;
        }
    }
}
