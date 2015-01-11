using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class TrackUpdateNowPlayingCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Artist { get; set; }

        public string Album { get; set; }

        public string Track { get; set; }

        public string AlbumArtist { get; set; }

        public bool ChosenByUser { get; set; }

        public TimeSpan? Duration { get; set; }

        public TrackUpdateNowPlayingCommand(ILastAuth auth, string artist, string album, string track)
            : base(auth)
        {
            Method = "track.updateNowPlaying";

            Artist = artist;
            Album = album;
            Track = track;
        }

        public TrackUpdateNowPlayingCommand(ILastAuth auth, Scrobble scrobble)
            : this(auth, scrobble.Artist, scrobble.Album, scrobble.Track)
        {
            ChosenByUser = scrobble.ChosenByUser;
            Duration = scrobble.Duration;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", Artist);
            Parameters.Add("album", Album);
            Parameters.Add("track", Track);
            Parameters.Add("albumArtist", AlbumArtist);
            Parameters.Add("chosenByUser", Convert.ToInt32(ChosenByUser).ToString());

            if (Duration.HasValue)
            {
                Parameters.Add("duration",Math.Round(Duration.Value.TotalSeconds).ToString());
            }
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