using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class LoveTrackCommand : PostAsyncCommandBase<LastResponse>
    {
        public string TrackName { get; protected set; }
        public string ArtistName { get; protected set; }

        public LoveTrackCommand(IAuth auth, string trackname, string artistname)
            : base(auth)
        {
            Method = "track.love";
            TrackName = trackname;
            ArtistName = artistname;
        }

        public async override Task<LastResponse> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"track", TrackName},
                                 {"artist", ArtistName}
                             };

            return await ExecuteInternal(parameters);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse(error);
            }
        }
    }
}