using System;
using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Objects
{
    public class LastWeeklyChartList : ILastfmObject
    {
        public string Text { get; private set; }
        public DateTime FromDate { get; private set; }
        public double From { get; private set; }
        public DateTime ToDate { get; private set; }
        public double To { get; private set; }


       /// <summary>
        /// Parses the given <paramref name="token"/> to a
        /// <see cref="LastWeeklyChartList"/>.
        /// </summary>
        /// <param name="token">JToken to parse.</param>
        /// <returns>Parsed LastWeeklyChartList.</returns>
        internal static LastWeeklyChartList ParseJToken(JToken token)
        {
            var c = new LastWeeklyChartList
            {
                Text = token.Value<string>("#name"),
                FromDate = token.Value<double>("from").FromUnixTime().DateTime,
                From = token.Value<double>("from"),
                ToDate = token.Value<double>("to").FromUnixTime().DateTime,
                To =  token.Value<double>("to")
            };

            return c;
        }

    }
}