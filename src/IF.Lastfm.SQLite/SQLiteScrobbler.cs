using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Scrobblers;
using Newtonsoft.Json.Converters;
using SQLite;

namespace IF.Lastfm.SQLite
{
    public class SQLiteScrobbler : ScrobblerBase
    {
        public string DatabasePath { get; }

        public SQLiteScrobbler(ILastAuth auth, string databasePath, HttpClient client = null) : base(auth, client)
        {
            DatabasePath = databasePath;
        }

        public override async Task<IEnumerable<Scrobble>> GetCachedAsync()
        {
            var db = GetConnection();
            var tableInfo = db.Table<Scrobble>();
            var cached = await tableInfo.ToListAsync();
            return cached;
        }

        public override async Task RemoveFromCacheAsync(ICollection<Scrobble> scrobbles)
        {
            var db = GetConnection();
            await db.RunInTransactionAsync(connection =>
            {
                foreach (var scrobble in scrobbles)
                {
                    connection.Delete(scrobble);
                }
            });

            await Task.WhenAll(scrobbles.Select(s => db.DeleteAsync(s)).ToArray());
        }

        public override async Task<int> GetCachedCountAsync()
        {
            var db = GetConnection();
            var tableInfo = db.Table<Scrobble>();
            var count = await tableInfo.CountAsync();
            return count;
        }

        protected override async Task<LastResponseStatus> CacheAsync(IEnumerable<Scrobble> scrobbles, LastResponseStatus reason)
        {
            // TODO cache reason
            var db = GetConnection();
            await db.InsertAllAsync(scrobbles);
            return LastResponseStatus.Cached;
        }
        
        private SQLiteAsyncConnection GetConnection()
        {
            var db = new SQLiteAsyncConnection(DatabasePath, SQLiteOpenFlags.ReadWrite);
            db.GetConnection().CreateTable<Scrobble>(CreateFlags.AutoIncPK | CreateFlags.AllImplicit);
            return db;
        }
    }
}
