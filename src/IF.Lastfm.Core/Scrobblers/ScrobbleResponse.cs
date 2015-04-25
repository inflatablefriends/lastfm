using System;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Scrobblers
{
    public class ScrobbleResponse : ILastResponse
    {
        public LastResponseStatus Status { get; internal set; }

        public bool Success
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
    }
}