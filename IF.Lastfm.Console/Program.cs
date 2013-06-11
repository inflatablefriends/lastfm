using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;

namespace IF.Lastfm.Console
{
    class Program
    {
        private const string ApiKey = "xxx";
        private const string ApiSecret = "xxx";

        static void Main(string[] args)
        {
            Run().Wait();
        }

        public static async Task Run()
        {
            var auth = new Auth(ApiKey, ApiSecret);
            await auth.GetSessionTokenAsync("xxx", "xxx");

            var albumApi = new AlbumApi(auth);

            var album = await albumApi.GetAlbumInfoAsync("Grimes", "Visions", false);

            var req = WebRequest.Create("");
            var w = await req.GetRequestStreamAsync();

            System.Console.ReadLine();
        }
    }
}
