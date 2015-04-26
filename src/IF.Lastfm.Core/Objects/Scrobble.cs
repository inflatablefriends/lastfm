using System;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class Scrobble
    {
        public string IgnoredReason { get; private set; }

        public string Artist { get; private set; }

        public string AlbumArtist { get; set; }

        public string Album { get; private set; }

        public string Track { get; private set; }

        public DateTimeOffset TimePlayed { get; private set; }

        public bool ChosenByUser { get; set; }

        public TimeSpan? Duration { get; set; }

        public Scrobble()
        {
        }

        public Scrobble(string artist, string album, string track, DateTimeOffset timeplayed)
        {
            Artist = artist;
            Album = album;
            Track = track;
            TimePlayed = timeplayed;
        }

        internal static Scrobble ParseJToken(JToken token)
        {
            var album = token["album"]["#text"].Value<string>();
            var artist = token["artist"]["#text"].Value<string>();
            var track = token["track"]["#text"].Value<string>();
            var albumArtist = token["albumArtist"]["#text"].Value<string>();
            var timestamp = token["timestamp"].Value<double>();

            var ignoredMessage = String.Empty;
            var ignoredToken = token["ignoredMessage"];
            if (ignoredToken != null)
            {
                ignoredMessage = ignoredToken["#text"].Value<string>();
            }

            return new Scrobble(artist, album, track, timestamp.FromUnixTime())
            {
                AlbumArtist = albumArtist,
                IgnoredReason = ignoredMessage
            };
        }
    }
}