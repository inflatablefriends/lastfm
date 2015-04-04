using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.Track
{
    internal class LoveCommand : PostAsyncCommandBase<LastResponse>
    {
        public string TrackName { get; set; }

        public string ArtistName { get; set; }

        public LoveCommand(ILastAuth auth, string trackname, string artistname)
            : base(auth)
        {
            Method = "track.love";

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