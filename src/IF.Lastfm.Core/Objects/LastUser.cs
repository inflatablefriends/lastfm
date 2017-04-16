using System;
using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json.Linq;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Objects
{
    public class LastUser : ILastfmObject
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public LastImageSet Avatar { get; private set; }
        public string Id { get; private set; }
        public int Age { get; private set; }
        public string Country { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsSubscriber { get; private set; }
        public int Playcount { get; private set; }
        public int Playlists { get; private set; }
        public DateTime TimeRegistered { get; private set; }
        public int Bootstrap { get; private set; }
        public string Type { get; private set; }

        /// <summary>
        /// Parses the given <paramref name="token"/> to a
        /// <see cref="LastUser"/>.
        /// </summary>
        /// <param name="token">JToken to parse.</param>
        /// <returns>Parsed LastUser.</returns>
        internal static LastUser ParseJToken(JToken token)
        {
            var u = new LastUser
            {
                Name = token.Value<string>("name"),
                FullName = token.Value<string>("realname"),
                Country = token.Value<string>("country"),
                Id = token.Value<string>("id"),
                Playcount = token.Value<int>("playcount"),
                Playlists = token.Value<int>("playlists"),
                Gender = ParseGender(token.Value<string>("gender")),
                IsSubscriber = Convert.ToBoolean(token.Value<int>("subscriber")),
                TimeRegistered = token.Value<double>("registered.unixtime").FromUnixTime().DateTime,
                Bootstrap = token.Value<int>("bootstrap"),
                Type = token.Value<string>("type")
            };

            var images = token.SelectToken("image");
            if (images != null)
            {
                u.Avatar = LastImageSet.ParseJToken(images);
            }

            return u;
        }

        /// <summary>
        /// Parses the given string into a <see cref="Gender"/>.
        /// </summary>
        /// <param name="gender">String to parse.</param>
        /// <returns>Parsed <see cref="Gender"/>.</returns>
        internal static Gender ParseGender(string gender)
        {
            switch (gender)
            {
                case "m":
                    return Gender.Male;
                case "f":
                    return Gender.Female;
                default:
                    return Gender.Other;
            }
        }
    }
}