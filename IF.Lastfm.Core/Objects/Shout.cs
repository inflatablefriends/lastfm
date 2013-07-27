using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IF.Lastfm.Core.Api.Helpers;
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

        public static PageResponse<Shout> ParsePageJToken(JToken token)
        {
            var shoutsToken = token.SelectToken("shout");

            var pageresponse = PageResponse<Shout>.CreateSuccessResponse();
            pageresponse.AddPageInfoFromJToken(token.SelectToken("@attr"));

            var shouts = new List<Shout>();
            if (shoutsToken != null && pageresponse.TotalItems > 0)
            {
                if (pageresponse.Page == pageresponse.TotalPages
                    && pageresponse.TotalItems % pageresponse.PageSize == 1)
                {
                    // array notation isn't used on the api if there is only one shout.
                    shouts.Add(Shout.ParseJToken(shoutsToken));
                }
                else
                {
                    shouts.AddRange(shoutsToken.Children().Select(Shout.ParseJToken));
                }
            }

            pageresponse.Content = shouts;

            return pageresponse;
        }
    }
}