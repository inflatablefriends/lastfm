using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests.Scrobblers;

namespace IF.Lastfm.SQLite.Tests.Integration
{
    public class SQLiteScrobblerTests : ScrobblerTestsBase
    {
        protected override IScrobbler GetScrobbler()
        {
            return new SQLiteScrobbler(MockAuth.Object, "test.db");
        }
    }
}
