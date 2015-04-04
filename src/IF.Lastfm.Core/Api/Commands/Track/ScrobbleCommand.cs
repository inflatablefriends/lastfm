using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.Track
{
    internal class ScrobbleCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Artist { get; set; }

        public string Album { get; set; }

        public string Track { get; set; }

        public string AlbumArtist { get; set; }

        public DateTimeOffset? TimePlayed { get; set; }

        public bool ChosenByUser { get; set; }

        public ScrobbleCommand(ILastAuth auth, string artist, string album, string track, string albumArtist, DateTimeOffset? timeplayed)
            : base(auth)
        {
            Method = "track.scrobble";

            Artist = artist;
            Album = album;
            Track = track;
            AlbumArtist = albumArtist;
            TimePlayed = timeplayed;
        }

        public ScrobbleCommand(ILastAuth auth, Scrobble scrobble)
            : this(auth, scrobble.Artist, scrobble.Album, scrobble.Track, scrobble.AlbumArtist, scrobble.TimePlayed)
        {
            ChosenByUser = scrobble.ChosenByUser;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", Artist);
            Parameters.Add("album", Album);
            Parameters.Add("track", Track);
            Parameters.Add("albumArtist", AlbumArtist);
            Parameters.Add("chosenByUser", Convert.ToInt32(ChosenByUser).ToString());
            
            if (TimePlayed.HasValue)
            {
                Parameters.Add("timestamp", TimePlayed.Value.AsUnixTime().ToString());
            }
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse>(status);
            }
        }
    }
}