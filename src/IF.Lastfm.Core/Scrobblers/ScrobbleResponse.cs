using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Scrobblers
{
    public class ScrobbleResponse : LastResponse
    {
        public int AcceptedCount { get; internal set; }

        public IEnumerable<Scrobble> Ignored { get; internal set; }
        
        public override bool Success
        {
            get
            {
                switch (Status)
                {
                    case LastResponseStatus.Successful:
                    case LastResponseStatus.Cached:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public Exception Exception { get; internal set; }

        public ScrobbleResponse(LastResponseStatus status)
        {
            Status = status;
        }

        public ScrobbleResponse()
        {
            Ignored = Enumerable.Empty<Scrobble>();
        }

        public static Task<ScrobbleResponse> CreateSuccessResponse(string json)
        {
            var root = JObject.Parse(json);
            var scrobblesToken = root["scrobbles"]["scrobble"];
            var allItems = PageResponse<Scrobble>.ParseItemsToken(scrobblesToken, Scrobble.ParseJToken).ToList();
            var ignored = allItems.Where(s => !String.IsNullOrEmpty(s.IgnoredReason)).ToList();

            var acceptedCount = allItems.Count - ignored.Count;

            var response = new ScrobbleResponse(LastResponseStatus.Successful)
            {
                AcceptedCount = acceptedCount,
                Ignored = ignored
            };

            return Task.FromResult(response);
        }
    }
}