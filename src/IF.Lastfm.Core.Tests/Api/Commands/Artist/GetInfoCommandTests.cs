using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Artist
{
    public class GetInfoCommandTests : CommandTestsBase
    {
        private GetInfoCommand _command;

        [SetUp]
        public void Initialise()
        {
            _command = new GetInfoCommand(MAuth.Object)
            {
                ArtistName = "Frightened Rabbit"
            };
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var expectedArtist = new LastArtist()
            {
                Name = "Frightened Rabbit",
                Mbid = "dc21d171-7204-4759-9fd0-77d031aeb40c",
                Url = new Uri("http://www.last.fm/music/Frightened+Rabbit"),
                MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/64/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/126/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/252/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/_/50340089/Frightened+Rabbit+frabbit.jpg"),
                    // todo streamable
                    OnTour = false,
                Similar = new List<LastArtist>
                {
                    new LastArtist
                    {
                        Name = "Admiral Fallow",
                        Url = new Uri("http://www.last.fm/music/Admiral+Fallow"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/64/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/126/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/252/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/500/48454975/Admiral+Fallow+l_1185fb2755064ccfbab2871ecec8.jpg")
                    },
                    new LastArtist
                    {
                        Name = "The Twilight Sad",
                        Url = new Uri("http://www.last.fm/music/The+Twilight+Sad"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/64/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/126/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/252/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/500/18201771/The+Twilight+Sad+hi+how+are+you.jpg"),
                    },
                    new LastArtist
                    {
                        Name = "Owl John",
                        Url = new Uri("http://www.last.fm/music/Owl+John"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/101981791.png",
                            "http://userserve-ak.last.fm/serve/64/101981791.png",
                            "http://userserve-ak.last.fm/serve/126/101981791.png",
                            "http://userserve-ak.last.fm/serve/252/101981791.png",
                            "http://userserve-ak.last.fm/serve/500/101981791/Owl+John+owl.png"),
                    },
                    new LastArtist
                    {
                        Name = "We Were Promised Jetpacks",
                        Url = new Uri("http://www.last.fm/music/We+Were+Promised+Jetpacks"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/64/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/126/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/252/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/_/53527397/We+Were+Promised+Jetpacks+wwpj.jpg"),
                    },
                    new LastArtist
                    {
                        Name = "Meursault",
                        Url = new Uri("http://www.last.fm/music/Meursault"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/64/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/126/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/252/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/_/41921789/Meursault+lovely+fuckwits.jpg"),
                    }
                },
                Tags = new List<LastTag>
                {
                    new LastTag("indie", "http://www.last.fm/tag/indie"),
                    new LastTag("scottish", "http://www.last.fm/tag/scottish"),
                    new LastTag("indie rock", "http://www.last.fm/tag/indie%20rock"),
                    new LastTag("folk", "http://www.last.fm/tag/folk"),
                    new LastTag("folk rock", "http://www.last.fm/tag/folk%20rock"),
                },
                Bio = new LastWiki
                {
                    Content = "Frightened Rabbit are an <a href=\"http://www.last.fm/tag/indie%20rock\" class=\"bbcode_tag\" rel=\"tag\">indie rock</a> band which formed in 2003 in Glasgow, Scotland. The band currently consists of Scott Hutchison (vocals, guitar), Billy Kennedy (guitar, keyboards), Grant Hutchison (drums, vocals), Andy Monaghan (guitar, keyboards) and Gordon Skene (guitar, keyboards). The band has released four albums: &quot;Sing the Greys&quot; (2006), &quot;The Midnight Organ Fight&quot; (2008), &quot;The Winter of Mixed Drinks&quot; (2010) and &quot;Pedestrian Verse&quot; (2013).  \n\n        <a href=\"http://www.last.fm/music/Frightened+Rabbit\">Read more about Frightened Rabbit on Last.fm</a>.\n    \n    \nUser-contributed text is available under the Creative Commons By-SA License and may also be available under the GNU FDL.",
                    Summary = "Frightened Rabbit are an <a href=\"http://www.last.fm/tag/indie%20rock\" class=\"bbcode_tag\" rel=\"tag\">indie rock</a> band which formed in 2003 in Glasgow, Scotland. The band currently consists of Scott Hutchison (vocals, guitar), Billy Kennedy (guitar, keyboards), Grant Hutchison (drums, vocals), Andy Monaghan (guitar, keyboards) and Gordon Skene (guitar, keyboards). The band has released four albums: &quot;Sing the Greys&quot; (2006), &quot;The Midnight Organ Fight&quot; (2008), &quot;The Winter of Mixed Drinks&quot; (2010) and &quot;Pedestrian Verse&quot; (2013).  \n\n        <a href=\"http://www.last.fm/music/Frightened+Rabbit\">Read more about Frightened Rabbit on Last.fm</a>.",
                    Published = new DateTimeOffset(2013, 2, 6, 0, 4, 40, TimeSpan.Zero),
                    YearFormed = 2003
                },
                Stats = new LastStats
                {
                    Listeners = 513447,
                    Plays = 0,
                    UserPlayCount = null
                }
            };
            
            var file = GetFileContents("ArtistApi.ArtistGetInfoSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetInfoSucess));
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var expectedJson = expectedArtist.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleSuccessResponseForUser()
        {
            _command = new GetInfoCommand(MAuth.Object)
            {
                ArtistName = "Frightened Rabbit",
                UserName = "test-user"
            };

            var expectedArtist = new LastArtist()
            {
                Name = "Frightened Rabbit",
                Mbid = "dc21d171-7204-4759-9fd0-77d031aeb40c",
                Url = new Uri("http://www.last.fm/music/Frightened+Rabbit"),
                MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/64/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/126/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/252/50340089.jpg",
                    "http://userserve-ak.last.fm/serve/_/50340089/Frightened+Rabbit+frabbit.jpg"),
                // todo streamable
                OnTour = false,
                Similar = new List<LastArtist>
                {
                    new LastArtist
                    {
                        Name = "Admiral Fallow",
                        Url = new Uri("http://www.last.fm/music/Admiral+Fallow"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/64/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/126/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/252/48454975.jpg",
                            "http://userserve-ak.last.fm/serve/500/48454975/Admiral+Fallow+l_1185fb2755064ccfbab2871ecec8.jpg")
                    },
                    new LastArtist
                    {
                        Name = "The Twilight Sad",
                        Url = new Uri("http://www.last.fm/music/The+Twilight+Sad"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/64/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/126/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/252/18201771.jpg",
                            "http://userserve-ak.last.fm/serve/500/18201771/The+Twilight+Sad+hi+how+are+you.jpg"),
                    },
                    new LastArtist
                    {
                        Name = "Owl John",
                        Url = new Uri("http://www.last.fm/music/Owl+John"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/101981791.png",
                            "http://userserve-ak.last.fm/serve/64/101981791.png",
                            "http://userserve-ak.last.fm/serve/126/101981791.png",
                            "http://userserve-ak.last.fm/serve/252/101981791.png",
                            "http://userserve-ak.last.fm/serve/500/101981791/Owl+John+owl.png"),
                    },
                    new LastArtist
                    {
                        Name = "We Were Promised Jetpacks",
                        Url = new Uri("http://www.last.fm/music/We+Were+Promised+Jetpacks"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/64/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/126/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/252/53527397.jpg",
                            "http://userserve-ak.last.fm/serve/_/53527397/We+Were+Promised+Jetpacks+wwpj.jpg"),
                    },
                    new LastArtist
                    {
                        Name = "Meursault",
                        Url = new Uri("http://www.last.fm/music/Meursault"),
                        MainImage = new LastImageSet("http://userserve-ak.last.fm/serve/34/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/64/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/126/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/252/41921789.jpg",
                            "http://userserve-ak.last.fm/serve/_/41921789/Meursault+lovely+fuckwits.jpg"),
                    }
                },
                Tags = new List<LastTag>
                {
                    new LastTag("indie", "http://www.last.fm/tag/indie"),
                    new LastTag("scottish", "http://www.last.fm/tag/scottish"),
                    new LastTag("indie rock", "http://www.last.fm/tag/indie%20rock"),
                    new LastTag("folk", "http://www.last.fm/tag/folk"),
                    new LastTag("folk rock", "http://www.last.fm/tag/folk%20rock"),
                },
                Bio = new LastWiki
                {
                    Content = "Frightened Rabbit are an <a href=\"http://www.last.fm/tag/indie%20rock\" class=\"bbcode_tag\" rel=\"tag\">indie rock</a> band which formed in 2003 in Glasgow, Scotland. The band currently consists of Scott Hutchison (vocals, guitar), Billy Kennedy (guitar, keyboards), Grant Hutchison (drums, vocals), Andy Monaghan (guitar, keyboards) and Gordon Skene (guitar, keyboards). The band has released four albums: &quot;Sing the Greys&quot; (2006), &quot;The Midnight Organ Fight&quot; (2008), &quot;The Winter of Mixed Drinks&quot; (2010) and &quot;Pedestrian Verse&quot; (2013).  \n\n        <a href=\"http://www.last.fm/music/Frightened+Rabbit\">Read more about Frightened Rabbit on Last.fm</a>.\n    \n    \nUser-contributed text is available under the Creative Commons By-SA License and may also be available under the GNU FDL.",
                    Summary = "Frightened Rabbit are an <a href=\"http://www.last.fm/tag/indie%20rock\" class=\"bbcode_tag\" rel=\"tag\">indie rock</a> band which formed in 2003 in Glasgow, Scotland. The band currently consists of Scott Hutchison (vocals, guitar), Billy Kennedy (guitar, keyboards), Grant Hutchison (drums, vocals), Andy Monaghan (guitar, keyboards) and Gordon Skene (guitar, keyboards). The band has released four albums: &quot;Sing the Greys&quot; (2006), &quot;The Midnight Organ Fight&quot; (2008), &quot;The Winter of Mixed Drinks&quot; (2010) and &quot;Pedestrian Verse&quot; (2013).  \n\n        <a href=\"http://www.last.fm/music/Frightened+Rabbit\">Read more about Frightened Rabbit on Last.fm</a>.",
                    Published = new DateTimeOffset(2013, 2, 6, 0, 4, 40, TimeSpan.Zero),
                    YearFormed = 2003
                },
                Stats = new LastStats
                {
                    Listeners = 513447,
                    Plays = 0,
                    UserPlayCount = 59452
                }
            };

            var file = GetFileContents("ArtistApi.ArtistGetInfoForUserSuccess.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);

            var expectedJson = expectedArtist.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.AreEqual(expectedJson, actualJson, expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public void ArtistGetInfo_SetLangParam_Success()
        {
            GetInfoCommand _command2 = new GetInfoCommand(MAuth.Object)
            {
                ArtistName = "Frightened Rabbit",
                BioLanguage = "fr"
            };
            
            //call the commands SetParameter method - this is ususally done in Command.ExecuteAsync
            _command2.SetParameters();
            
            string langValue; 
            Assert.IsTrue(_command2.Parameters.TryGetValue("lang", out langValue));
            Assert.AreEqual("fr", langValue);
        }

        [Test]
        public void ArtistGetInfo_SetUserNameParam_Success()
        {
            string expectedUserName = "test-user";

            GetInfoCommand getInfoCommand = new GetInfoCommand(MAuth.Object)
            {
                ArtistName = "Frightened Rabbit",
                UserName = expectedUserName
            };

            getInfoCommand.SetParameters();
            string userNameValue;
            Assert.IsTrue(getInfoCommand.Parameters.TryGetValue("username", out userNameValue));
            Assert.AreEqual(expectedUserName, userNameValue);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("ArtistApi.ArtistGetInfoMissing.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetInfoMissing));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
        }
    }
}
