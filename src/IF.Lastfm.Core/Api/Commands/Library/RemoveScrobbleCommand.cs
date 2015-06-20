using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.Library {
    [ApiMethodName(LastMethodsNames.library_removeScrobble)]
    internal class RemoveScrobbleCommand : PostAsyncCommandBase<LastResponse> {
        public override string Method
        { get { return LastMethodsNames.library_removeScrobble; } }

        public string Artist { get; set; }

        public string Track { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public RemoveScrobbleCommand( ILastAuth auth, string artist, string track, DateTimeOffset timestamp ) : base( auth ) {
            Artist = artist;
            Track = track;
            Timestamp = timestamp;
        }


        public override void SetParameters() {
            Parameters.Add( "artist", Artist );
            Parameters.Add( "track", Track );
            Parameters.Add( "timestamp", Timestamp.AsUnixTime().ToString() );
        }

        public async override Task<LastResponse> HandleResponse( HttpResponseMessage response ) {
            return await LastResponse.HandleResponse( response );
        }
    }
}
