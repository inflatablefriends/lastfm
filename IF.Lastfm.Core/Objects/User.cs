using System;
using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Objects
{
    public class User
    {
        #region Properties

        public string Name { get; set; }
        public string FullName { get; set; }
        public LastImageCollection Avatar { get; set; }
        public string Id { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public Gender Gender { get; set; }
        public bool IsSubscriber { get; set; }
        public int Playcount { get; set; }
        public DateTime TimeRegistered { get; set; }

        #endregion

        /// <summary>
        /// TODO 
        ///     "gender": "m",
        //"playcount": "79972",
        //"playlists": "4",
        //"bootstrap": "0",
        //"registered": {
        //  "#text": "2002-11-20 11:50",
        //  "unixtime": "1037793040"
        //},
        //"type": "alumni"
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static User ParseJToken(JToken token)
        {
            var u = new User();

            u.Name = token.Value<string>("name");
            u.FullName = token.Value<string>("realname");
            u.Country = token.Value<string>("country");
            u.Id = token.Value<string>("id");

            var subscribed = token.SelectToken("subscriber");
            if (subscribed != null)
            {
                u.IsSubscriber = Convert.ToBoolean(subscribed.Value<int>());
            }

            var images = token.SelectToken("image");
            if (images != null)
            {
                u.Avatar = LastImageCollection.ParseJToken(images);
            }

            return u;
        }
    }
}