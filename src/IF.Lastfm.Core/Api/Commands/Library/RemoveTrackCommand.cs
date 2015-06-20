using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Library {
    [ApiMethodName(LastMethodsNames.library_removeTrack)]
    internal class RemoveTrackCommand : PostAsyncCommandBase<LastResponse> {
        public override string Method
        { get { return LastMethodsNames.library_removeTrack; } }

        public string Artist { get; set; }

        public string Track { get; set; }

        public RemoveTrackCommand( ILastAuth auth, string artist, string track) : base( auth ) {
            Artist = artist;
            Track = track;
        }


        public override void SetParameters() {
            Parameters.Add( "artist", Artist );
            Parameters.Add( "track", Track );
        }

        public async override Task<LastResponse> HandleResponse( HttpResponseMessage response ) {
            return await LastResponse.HandleResponse( response );
        }
    }
}