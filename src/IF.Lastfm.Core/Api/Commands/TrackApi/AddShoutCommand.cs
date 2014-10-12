using IF.Lastfm.Core.Api.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Track { get; set; }

        public string Artist { get; set; }

        public string Message { get; set; }


        public AddShoutCommand(ILastAuth auth, string track, string artist, string message) : base(auth)
        {
            Method = "track.shout";

            Track = track;
            Artist = artist;
            Message = message;
        }

        public override void SetParameters()
        {
            Parameters.Add("track", Track);
            Parameters.Add("artist", Artist);
            Parameters.Add("message", Message);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
