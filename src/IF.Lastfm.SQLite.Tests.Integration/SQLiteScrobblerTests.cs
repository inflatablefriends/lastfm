using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Helpers;
using IF.Lastfm.Core.Scrobblers;
using IF.Lastfm.Core.Tests;
using IF.Lastfm.Core.Tests.Resources;
using IF.Lastfm.Core.Tests.Scrobblers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLite;

namespace IF.Lastfm.SQLite.Tests.Integration
{
    [TestClass]
    public class SQLiteeScrobblerTests : ScrobblerTestsBase
    {
        protected override IScrobbler GetScrobbler()
        {
            return new SQLiteScrobbler(MockAuth.Object, "test.db");
        }
    }
}
