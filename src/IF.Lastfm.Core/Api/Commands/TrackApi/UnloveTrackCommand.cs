using IF.Lastfm.Core.Api.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class UnloveTrackCommand : PostAsyncCommandBase<LastResponse>
    {
        public string TrackName { get; set; }

        public string ArtistName { get; set; }

        public UnloveTrackCommand(ILastAuth auth, string trackname, string artistname)
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