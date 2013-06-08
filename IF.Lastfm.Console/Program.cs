using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;

namespace IF.Lastfm.Console
{
    class Program
    {
        private const string ApiKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        private const string ApiSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx";

        static void Main(string[] args)
        {
            var lastfm = new Auth(ApiKey, ApiSecret);

            lastfm.GetSessionTokenAsync("xxxxxxxxxxxx", "xxxxxxxxxxxx").Wait();
        }
    }
}
