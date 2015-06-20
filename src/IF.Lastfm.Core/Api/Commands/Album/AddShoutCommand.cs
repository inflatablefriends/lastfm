using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Album
{
    [ApiMethodName(LastMethodsNames.album_shout)]
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public override string Method
        { get { return LastMethodsNames.album_shout; } }

        public string Album { get; set; }

        public string Artist { get; set; }

        public string Message { get; set; }

        public AddShoutCommand(ILastAuth auth, string album, string artist, string message)
            : base(auth)
        {
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
