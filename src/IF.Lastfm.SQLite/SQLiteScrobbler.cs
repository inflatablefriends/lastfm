using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
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
        }

        protected override Task<IEnumerable<Scrobble>> GetCachedAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<LastResponseStatus> CacheAsync(Scrobble scrobble, LastResponseStatus originalResponseStatus)
        {
            // TODO cache originalResponse - reason to cache
            return Task.Run(() =>
            {
                Cache(scrobble);
                return LastResponseStatus.Cached;
            });
        }

        private void Cache(Scrobble scrobble)
        {
            var connection = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadWrite);

            if (connection.TableMappings.All(table => table.TableName != typeof(Scrobble).Name))
            {
                connection.CreateTable<Scrobble>();
            }

            connection.Insert(scrobble);
        }
    }
}
