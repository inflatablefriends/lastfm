using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Track { get; set; }
        public string Artist { get; set; }
        public string Message { get; set; }

        public AddShoutCommand(IAuth auth, string track, string artist, string message) : base(auth)
        {
            Method = "track.shout";
            Track = track;
            Artist = artist;
            Message = message;
        }

        public async override Task<LastResponse> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"track", Track},
                                 {"artist", Artist},
                                 {"message", Message}
                             };

            return await ExecuteInternal(parameters);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
