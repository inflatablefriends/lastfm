using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Scrobblers;
using SQLite;

namespace IF.Lastfm.SQLite
{
    public class SQLiteScrobbler : ScrobblerBase
    {
        public string DatabasePath { get; private set; }

        public SQLiteScrobbler(ILastAuth auth, string databasePath) : base(auth)
        {
            DatabasePath = databasePath;

            CacheEnabled = true;
        }

        public override Task<IEnumerable<Scrobble>> GetCachedAsync()
        {
            using (var db = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadOnly))
            {
                var tableInfo = db.GetTableInfo(typeof (Scrobble).Name);
                if (!tableInfo.Any())
                {
                    return Task.FromResult(Enumerable.Empty<Scrobble>());
                }

                var cached = db.Query<Scrobble>("SELECT * FROM Scrobble");
                db.Close();

                return Task.FromResult(cached.AsEnumerable());
            }
        }

        protected override Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobbles, LastResponseStatus originalResponseStatus)
        {
            // TODO cache originalResponse - reason to cache
            return Task.Run(() =>
            {
                Cache(scrobbles);
                return LastResponseStatus.Cached;
            });
        }

        private void Cache(IEnumerable<Scrobble> scrobbles)
        {
            using (var db = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadWrite))
            {
                var tableInfo = db.GetTableInfo(typeof (Scrobble).Name);
                if (!tableInfo.Any())
                {
                    db.CreateTable<Scrobble>();
                }

                db.BeginTransaction();
                foreach (var scrobble in scrobbles)
                {
                    db.Insert(scrobble);
                }
                db.Commit();

                db.Close();
            }
        }
    }
}
