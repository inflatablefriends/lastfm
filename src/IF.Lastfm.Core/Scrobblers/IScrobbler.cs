﻿using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Scrobblers
{
    public interface IScrobbler
    {
        Task<ScrobbleResponse> ScrobbleAsync(Scrobble scrobble);
    }
}