using IF.Lastfm.Core.Api.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api.Commands.AlbumApi
{
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Album { get; set; }

        public string Artist { get; set; }

        public string Message { get; set; }

        public AddShoutCommand(ILastAuth auth, string album, string artist, string message)
            : base(auth)
        {
            Method = "album.shout";

            Album = album;
            Artist = artist;
            Message = message;
        }

        public override void SetParameters()
        {
            Parameters.Add("album", Album);
            Parameters.Add("artist", Artist);
            Parameters.Add("message", Message);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
