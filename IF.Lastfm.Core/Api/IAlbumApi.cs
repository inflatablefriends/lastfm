using System.Threading.Tasks;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface IAlbumApi
    {
        IAuth Auth { get; }

        Task<Album> GetAlbumInfoAsync(string artist, string album, bool autocorrect = false);
        Task<Album> GetAlbumInfoWithMbidAsync(string mbid, bool autocorrect = false);
    }
}