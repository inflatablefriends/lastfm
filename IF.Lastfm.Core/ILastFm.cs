using IF.Lastfm.Core.Api;

namespace IF.Lastfm.Core
{
    public interface ILastFm
    {
        IAuth Auth { get; }
    }
}