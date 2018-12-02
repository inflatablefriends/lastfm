using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Tests
{
    public static class TestHelper
    {
        private static JsonSerializer GetTestSerialiser()
        {
            var serialiser = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateFormatString = "yyyy-MM-dd HH:mm:ss.ff",
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new OrderedContractResolver()
            };

            serialiser.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ff",
                DateTimeStyles = DateTimeStyles.AdjustToUniversal
            });

            return serialiser;
        }

        private static JObject WithSortedProperties(this JObject jo)
        {
            var result = new JObject();
            foreach (var prop in jo.Properties().OrderBy(p => p.Name))
            {
                var nestedJo = prop.Value as JObject;
                if (nestedJo != null)
                {
                    result.Add(new JProperty(prop.Name, nestedJo.WithSortedProperties()));
                }
                else
                {
                    result.Add(prop);
                }
            }
            return result;
        }

        public static string TestSerialise<T>(this T poco)
        {
            if (poco == null)
            {
                return "";
            }

            var serialiser = GetTestSerialiser();
            var jt = JToken.FromObject(poco, serialiser);

            string ordered;
            if (jt.Type == JTokenType.Array)
            {
                var ja = (JArray) jt;

                var orderedJa =
                    new JArray(
                        ja.Descendants()
                            .Where(t => t.Type == JTokenType.Object)
                            .Cast<JObject>()
                            .Select(o => o.WithSortedProperties()));

                ordered = orderedJa.ToString();
            }
            else if (jt.Type == JTokenType.Object)
            {
                var jo = (JObject) jt;
                var orderedJo = jo.WithSortedProperties();

                ordered = orderedJo.ToString();
            }
            else
            {
                ordered = jt.ToString();
            }

            return ordered;
        }

        public static void AssertSerialiseEqual<T>(T one, T two)
        {
            var ones = one.TestSerialise();
            var twos = two.TestSerialise();

            Assert.AreEqual(ones, twos, ones.DifferencesTo(twos));
        }

        public static string DifferencesTo<T>(this IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var first = String.Join(Environment.NewLine, expected);
            var second = String.Join(Environment.NewLine, actual);

            return first.DifferencesTo(second);
        }

        public static string DifferencesTo(this string first, string second)
        {
            const string lineDiffTemplate = "{0}E: {1}\n{0}A: {2}";
            var start = Environment.NewLine + Environment.NewLine + "Differences:" + Environment.NewLine;

            var sb = new StringBuilder(start);
            var sr1 = new StringReader(first);
            var sr2 = new StringReader(second);
            int count = 1;
            using (sr1)
            {
                using (sr2)
                {
                    while (true)
                    {
                        var line1 = sr1.ReadLine();
                        var line1Read = !String.IsNullOrEmpty(line1);
                        var line2 = sr2.ReadLine();
                        var line2Read = !String.IsNullOrEmpty(line2);

                        if (line1Read && line2Read)
                        {
                            if (line1 != line2)
                            {
                                var line = String.Format(lineDiffTemplate, count, line1, line2);
                                sb.AppendLine(line);
                            }
                        }
                        else if (!line1Read && !line2Read)
                        {
                            break;
                        }
                        else // one string still has lines
                        {
                            var line = string.Format(lineDiffTemplate, count, line1, line2);
                            sb.AppendLine(line);
                        }

                        count++;
                    }
                }
            }
            return sb.ToString();
        }

        public static bool LineRead(this StringReader reader, out string line)
        {
            line = reader.ReadLine();

            return !string.IsNullOrEmpty(line);
        }

        public static IEnumerable<T> WrapEnumerable<T>(this T t)
        {
            return new[]
            {
                t
            };
        }

        public static DateTimeOffset RoundToNearestSecond(this DateTimeOffset dt)
        {
            var ms = dt.Millisecond;

            return ms < 500
                ? dt.AddMilliseconds(-ms)
                : dt.AddMilliseconds(1000 - ms);
        }

        public static void AssertValues<T>(
            this PageResponse<T> pageResponse,
            bool success,
            int totalItems,
            int pageSize,
            int page,
            int totalPages) where T : new()
        {
            const string messageFormat = "Page response:\n{0}\n\nExpected {1} to equal {2}";
            var json = pageResponse.TestSerialise();
            Func<string, dynamic, string> testMessage =
                (property, count) => string.Format(messageFormat, json, property, count);

            Assert.IsTrue(pageResponse.Success == success, testMessage("success", success));
            Assert.IsTrue(pageResponse.TotalItems == totalItems, testMessage("totalitems", totalItems));
            Assert.IsTrue(pageResponse.PageSize == pageSize, testMessage("pagesize", pageSize));
            Assert.IsTrue(pageResponse.Page == page, testMessage("page", page));
            Assert.IsTrue(pageResponse.TotalPages == totalPages, testMessage("totalpages", totalPages));

            Assert.IsNotNull(pageResponse.Content, "page content is null");
            Assert.IsTrue(pageResponse.Content.Count == totalItems, testMessage("content length", totalItems));
        }
        
        public static HttpResponseMessage CreateResponseMessage(HttpStatusCode status, byte[] resource)
        {
            var now = new DateTimeOffset(2015, 03, 04, 20, 07, 21, TimeSpan.Zero);
            var responseJson = Encoding.UTF8.GetString(resource);
            var stringContent = new StringContent(responseJson, Encoding.UTF8, "application/json");

            var testResponseMessage = new HttpResponseMessage(status)
            {
                Content = stringContent
            };

            return testResponseMessage;
        }
    }
}
