using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.TrackApi
{
    internal class ScrobbleTracksCommand : PostAsyncCommandBase<LastResponse>
    {
        public ScrobbleTracksCommand(IAuth auth, Scrobble scrobble) : base(auth)
        {
            ConstructInternal(auth, new[] { scrobble });
        }

        public ScrobbleTracksCommand(IAuth auth, IEnumerable<Scrobble> scrobbles) : base(auth)
        {
            ConstructInternal(auth, scrobbles);
        }

        private void ConstructInternal(IAuth auth, IEnumerable<Scrobble> scrobbles)
        {
            Method = "track.scrobble";
        }

        public override Task<LastResponse> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"artist", scrobble.Artist},
                                 {"album", scrobble.Album},
                                 {"track", scrobble.Track},
                                 {"albumArtist", scrobble.AlbumArtist},
                                 {"chosenByUser", scrobble.ChosenByUser.ToInt().ToString()},
                                 {"timestamp", scrobble.TimePlayed.ToUnixTimestamp().ToString()},
                                 {"sk", Auth.User.Token}
                             };

            HttpContent post = new StringContent();

        }

        public override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}
