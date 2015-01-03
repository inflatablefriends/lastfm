using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class TrackScrobbleCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Artist { get; set; }

        public string Album { get; set; }

        public string Track { get; set; }

        public string AlbumArtist { get; set; }

        public DateTime TimePlayed { get; set; }

        public bool ChosenByUser { get; set; }
        
        public TrackScrobbleCommand(ILastAuth auth, string artist, string album, string track, string albumArtist, DateTime timeplayed)
            : base(auth)
        {
            Method = "track.scrobble";

            Artist = artist;
            Album = album;
            Track = track;
            AlbumArtist = albumArtist;
            TimePlayed = timeplayed;
        }

        public TrackScrobbleCommand(ILastAuth auth, Scrobble scrobble)
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
            Parameters.Add("timestamp", TimePlayed.ToUnixTimestamp().ToString());
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse>(error);
            }
        }
    }
}