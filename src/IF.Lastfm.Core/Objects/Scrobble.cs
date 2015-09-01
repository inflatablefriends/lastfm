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
            var album = token.SelectToken("album.#text")?.Value<string>();
            var artist = token.SelectToken("artist.#text")?.Value<string>();
            var track = token.SelectToken("track.#text")?.Value<string>();
            var albumArtist = token.SelectToken("albumArtist.#text")?.Value<string>();
            var timestamp = token.SelectToken("timestamp").Value<double>();
            var ignoredMessage = token.SelectToken("ignoredMessage.#text")?.Value<string>();

            return new Scrobble(artist, album, track, timestamp.FromUnixTime())
            {
                AlbumArtist = albumArtist,
                IgnoredReason = ignoredMessage
            };
        }
    }
}