using System;

namespace IF.Lastfm.Core.Api
{
    public class Scrobble
    {
        #region Properties

        public string Artist { get; private set; }

        public string AlbumArtist { get; set; }

        public string Album { get; private set; }

        public string Track { get; private set; }

        public DateTime TimePlayed { get; private set; }

        public bool ChosenByUser { get; set; }

        public TimeSpan? Duration { get; set; }

        #endregion

        public Scrobble(string artist, string album, string track, DateTime timeplayed)
        {
            Artist = artist;
            Album = album;
            Track = track;
            TimePlayed = timeplayed;
        }
    }
}