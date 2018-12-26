using System;
using System.Collections.Generic;
using System.Text;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using Moq;
using System.Reflection;
using System.IO;

namespace IF.Lastfm.Core.Tests
{
    public class LastFmTests
    {
        [Test]
        public void ApiUrlFormatReturnsCorrectly()
        {
            const string expected = "https://ws.audioscrobbler.com/2.0/?method=tobias.funke&api_key=suddenvalley&blue=performance&format=json&uncle=t-bag";

            var actual = LastFm.FormatApiUrl("tobias.funke", "suddenvalley", new Dictionary<string, string>
                {
                    {"uncle", "t-bag"},
                    {"blue", "performance"}
                }, true);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsResponseValid()
        {
            LastResponseStatus status;
            
            Assert.IsFalse(LastFm.IsResponseValid(null, out status));
            Assert.IsFalse(LastFm.IsResponseValid("{invalid json", out status));

            var error6 = GetFileContents("ArtistApi.ArtistGetTagsError.json");
            //var error6 = Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsError);
            Assert.IsFalse(LastFm.IsResponseValid(error6, out status));
            Assert.AreEqual(LastResponseStatus.MissingParameters, status);

            var goodResponse = GetFileContents("ArtistApi.ArtistGetInfoSuccess.json");
            //var goodResponse = Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetInfoSuccess);
            Assert.IsTrue(LastFm.IsResponseValid(goodResponse, out status));
            Assert.AreEqual(LastResponseStatus.Successful, status);
        }

        protected string GetFileContents(string sampleFile)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = string.Format("IF.Lastfm.Core.Tests.Resources.{0}", sampleFile);
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}
