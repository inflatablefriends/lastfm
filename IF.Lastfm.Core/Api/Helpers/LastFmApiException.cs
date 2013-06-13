using System;
using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class LastFmApiException : Exception
    {
        public LastFmApiError Error { get; set; }

        public LastFmApiException(LastFmApiError error)
        {
            Error = error;
        }
    }
}