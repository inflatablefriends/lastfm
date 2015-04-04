using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
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

        public override Task CacheAsync(Scrobble scrobble)
        {
            return Task.Run(() => Cache(scrobble));
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
