using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IUserApi
    {
        IAuth Auth { get; }

        Task<PageResponse<Album>> GetTopAlbums(LastStatsTimeSpan span,
                                              int startIndex = 0,
                                              int endIndex = LastFm.DefaultPageLength);

        Task<PageResponse<Track>> GetRecentScrobbles(string username, DateTime since,
                                              int startIndex = 0,
                                              int endIndex = LastFm.DefaultPageLength);
    }

    /*
     * 
     * limit (Optional) : The number of results to fetch per page. Defaults to 50. Maximum is 200.
user (Required) : The last.fm username to fetch the recent tracks of.
page (Optional) : The page number to fetch. Defaults to first page.
from (Optional) : Beginning timestamp of a range - only display scrobbles after this time, in UNIX timestamp format (integer number of seconds since 00:00:00, January 1st 1970 UTC). This must be in the UTC time zone.
extended (0|1) (Optional) : Includes extended data in each artist, and whether or not the user has loved each track
to (Optional) : End timestamp of a range - only display scrobbles before this time, in UNIX timestamp format (integer number of seconds since 00:00:00, January 1st 1970 UTC). This must be in the UTC time zone.
api_key (Required) : A Last.fm API key.
     * 
     * */
}