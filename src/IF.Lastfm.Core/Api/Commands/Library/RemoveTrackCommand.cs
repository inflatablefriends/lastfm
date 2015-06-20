using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.Library {
    [ApiMethodName("library.removeTrack")]
    internal class RemoveTrackCommand : PostAsyncCommandBase<LastResponse> {
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