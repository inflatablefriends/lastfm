using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Console
{
    internal class Program
    {
        private static string _apiKey;
        private static string _apiSecret;
        private static string _username;
        private static string _pass;

        private static void Main(string[] args)
        {
            try
            {
                Task.Run(async () => await Run()).Wait();
            }
            catch (AggregateException agg)
            {
                foreach (var ex in agg.InnerExceptions)
                {
                    System.Console.WriteLine("\n====================\n");
                    if (ex is LastFmApiException)
                    {
                        var lex = ex as LastFmApiException;
                        System.Console.WriteLine("LastFmApiException thrown:\n    {0}\n    {1}",
                                                 lex.Error.GetApiName(),
                                                 lex.StackTrace);
                    }
                    else
                    {
                        System.Console.WriteLine("Exception thrown:\n    {0}\n    {1}",
                                                 ex.Message,
                                                 ex.StackTrace);
                    }
                    System.Console.WriteLine("\n====================\n");
                }
            }

            System.Console.ReadLine();
        }

        public static async Task Run()
        {
            await LoadSession();

            var auth = new Auth(_apiKey, _apiSecret);
            await auth.GetSessionTokenAsync(_username, _pass);

            var albumApi = new AlbumApi(auth);

            var album = await albumApi.GetAlbumInfoAsync("Grimes", "Visions", false);
            
        }

        private async static Task LoadSession()
        {
            const string path = @"C:\lastfm-wp-config.json";

            string json;
            using (var reader = new StreamReader(path))
            {
                json = await reader.ReadToEndAsync();
            }

            var jo = JsonConvert.DeserializeObject<JToken>(json);

            _apiKey = jo.Value<string>("apikey");
            _apiSecret = jo.Value<string>("apisecret");
            _username = jo.Value<string>("username");
            _pass = jo.Value<string>("pass");
        }
    }
}