using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class UnloveTrackCommand : PostAsyncCommandBase<LastResponse>
    {
        public string TrackName { get; protected set; }
        public string ArtistName { get; protected set; }

        public UnloveTrackCommand(IAuth auth, string trackname, string artistname)
            : base(auth)
        {
            Method = "track.unlove";
            TrackName = trackname;
            ArtistName = artistname;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", TrackName);
            Parameters.Add("artist", ArtistName);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}