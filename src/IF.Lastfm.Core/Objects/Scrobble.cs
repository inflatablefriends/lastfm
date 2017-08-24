using System;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace IF.Lastfm.Core.Objects
{
    public class Scrobble : IEquatable<Scrobble>
    {
        /// <summary>
        /// Not part of the Last.fm API. This is a convenience property allowing Scrobbles to have a unique ID.
        /// IF.Lastfm.SQLite uses this field to store a primary key, if this Scrobble was cached.
        /// Not used in Equals or GetHashCode implementations.
        /// </summary>
        public int Id { get; set; }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as Scrobble);
        }

        public bool Equals(Scrobble other)
        {
            return other != null &&
                   IgnoredReason == other.IgnoredReason &&
                   Artist == other.Artist &&
                   AlbumArtist == other.AlbumArtist &&
                   Album == other.Album &&
                   Track == other.Track &&
                   TimePlayed.Equals(other.TimePlayed) &&
                   ChosenByUser == other.ChosenByUser &&
                   EqualityComparer<TimeSpan?>.Default.Equals(Duration, other.Duration);
        }

        public override int GetHashCode()
        {
            var hashCode = 417801827;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IgnoredReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Artist);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AlbumArtist);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Album);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Track);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTimeOffset>.Default.GetHashCode(TimePlayed);
            hashCode = hashCode * -1521134295 + ChosenByUser.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan?>.Default.GetHashCode(Duration);
            return hashCode;
        }
    }
}