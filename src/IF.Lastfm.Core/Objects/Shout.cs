using Newtonsoft.Json.Linq;
using System;

namespace IF.Lastfm.Core.Objects
{
    public class Shout : ILastfmObject
    {
        #region Properties

        public string Body { get; set; }

        public string Author { get; set; }

        public DateTimeOffset TimePosted { get; set; }

        #endregion

        public Shout()
        {
        }

        public Shout(string author, string body, string time)
        {
            Author = author;
            Body = body;
            TimePosted = DateTime.Parse(time);
        }

        public static Shout ParseJToken(JToken token)
        {
            var s = new Shout();

            s.Body = token.Value<string>("body");
            s.Author = token.Value<string>("author");
            
            var date = token.Value<string>("date");
            DateTimeOffset postedAt;
            if (DateTimeOffset.TryParse(date, out postedAt))
            {
                s.TimePosted = postedAt;
            }

            return s;
        }
    }
}