using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Artist
{
    [ApiMethodName(LastMethodsNames.artist_shout)]
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public override string Method
        { get { return LastMethodsNames.artist_shout; } }

        public string Artist { get; set; }

        public string Message { get; set; }

        public AddShoutCommand(ILastAuth auth, string artist, string message) : base(auth)
        {
            Artist = artist;
            Message = message;
        }

        public override void SetParameters()
        {
            Parameters.Add("artist", Artist);
            Parameters.Add("message", Message);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
