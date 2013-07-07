using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class Shout
    {
        #region Properties

        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime TimePosted { get; set; }

        #endregion

        public static Shout ParseJToken(JToken token)
        {
            var s = new Shout();

            s.Body = token.Value<string>("body");
            s.Author = token.Value<string>("author");


            var provider = CultureInfo.InvariantCulture;

            var date = token.Value<string>("date");
            DateTime time;
            // Tue, 18 Jun 2013 17:39:50
            var success = DateTime.TryParseExact(date, "ddd, dd MMM yyyy HH:mm:ss", provider, DateTimeStyles.AssumeUniversal, out time);

            if (success)
            {
                s.TimePosted = time;
            }

            return s;
        }
    }
}