using System;

namespace IF.Lastfm.Core.Api
{
    public class Scrobble
    {
        #region Properties

        public string Artist { get; private set; }
        public string AlbumArtist { get; private set; }
        public string Album { get; private set; }
        public string Track { get; private set; }
        public DateTime TimePlayed { get; private set; }
        public bool ChosenByUser { get; private set; }
        public TimeSpan Duration { get; private set; }

        #endregion

        public Scrobble(string artist, string album, string track, DateTime timeplayed,
            string albumartist = "", bool chosenByUser = true)
        {
            Artist = artist;
            Album = album;
            Track = track;
            TimePlayed = timeplayed;
            AlbumArtist = string.IsNullOrWhiteSpace(albumartist) ? artist : albumartist;
            ChosenByUser = chosenByUser;
        }

        public Scrobble(string artist, string album, string track, DateTime timeplayed, TimeSpan duration,
            string albumartist = "", bool chosenByUser = true) : this(artist, album, track, timeplayed, albumartist, chosenByUser)
        {
            Duration = duration;
        }
    }
}