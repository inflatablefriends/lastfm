using System;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Scrobblers
{
    public class ScrobbleResponse : ILastResponse
    {
        public LastResponseStatus Status { get; internal set; }

        public bool Cached
        {
            get { return Status == LastResponseStatus.Cached; }
        }

        public Exception Exception { get; internal set; }

        public ScrobbleResponse(LastResponseStatus status)
        {
            Status = status;
        }
    }
}