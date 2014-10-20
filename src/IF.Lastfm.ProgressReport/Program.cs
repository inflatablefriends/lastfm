using HtmlAgilityPack;
using IF.Lastfm.Core.Api.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace IF.Lastfm.ProgressReport
{
    /// <summary>
    /// Scrapes Last.fm/api for available methods, compares to what commands are in IF.Lastfm.Core and generates some Markdown.
    /// Maybe some images if I'm feeling adventurous
    /// </summary>
    public class Program
    {
        public const string API_INTRO_PAGE = "http://www.last.fm/api/intro";

        public static void Main(string[] args)
        {
            // scrape Last.fm API documentation
            var apiGroup = GetApiMethods();
            if (apiGroup == null)
            {
                return;
            }

            // reflect on Last.fm assembly to find all implemented commands
            var allImplemented = GetImplementedCommands().ToList();

            foreach (var group in apiGroup)
            {
                var apiGroupName = group.Key;
                var implemented = allImplemented.Where(m => m.StartsWith(apiGroupName.ToLowerInvariant(), StringComparison.Ordinal)).ToList();

                var matches = group.Value.Intersect(implemented);
                var notImplemented = group.Value.Except(implemented);
                var secret = implemented.Except(group.Value);

                // TODO got all the information, now need to generate something!
            }
        }

        #region Scrape

        /// <summary>
        /// Scrape the API documentation for all the method names
        /// </summary>
        private static Dictionary<string, IEnumerable<string>> GetApiMethods()
        {
            var client = new HttpClient();
            var response = client.GetAsync(API_INTRO_PAGE);
            response.Wait();

            if (!response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine("Server returned {0} fetching {1}\n{2}", 
                    response.Result.StatusCode, API_INTRO_PAGE, response.Result.ReasonPhrase);
                Console.ReadLine();
                return null;
            }

            var html = response.Result.Content.ReadAsStringAsync();
            html.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(html.Result);

            var wspanel = doc.DocumentNode.SelectNodes("//ul[@class='wspanel']").LastOrDefault();
            if (wspanel == null)
            {
                Console.WriteLine("Couldn't parse HTML");
                Console.ReadLine();
                return null;
            }

            // each package is a section of the API
            var packages = wspanel.Descendants().Where(li => HasClass(li, "package"));

            var allMethods = new Dictionary<string, IEnumerable<string>>();
            foreach (var package in packages)
            {
                var h3 = package.Element("h3");

                var ul = package.Element("ul");

                var methodLinks = ul.SelectNodes("child::li");
                var methods = methodLinks.Select(a => a.InnerText);

                allMethods.Add(h3.InnerText, methods);
            }

            return allMethods;
        }

        private static bool HasClass(HtmlNode stay, string classy)
        {
            return stay.Attributes.Contains("class") && stay.Attributes["class"].Value.Contains(classy);
        }

        #endregion

        #region Reflect
        
        /// <summary>
        /// With thanks to Tim Murphy
        /// http://stackoverflow.com/a/4529684/268555
        /// </summary>
        public static IEnumerable<Type> FindSubClassesOf(Type type)
        {
            var assembly = type.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(type));
        }

        /// <summary>
        /// Reflect on implemented commands
        /// </summary>
        private static IEnumerable<string> GetImplementedCommands()
        {
            var types = FindSubClassesOf(typeof(LastAsyncCommandBase)).Where(t => t.IsClass && !t.IsAbstract);
            var methods = types.Select(GetApiMethodFromCommandType);

            return methods;
        }

        private static string GetApiMethodFromCommandType(Type type)
        {
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Count()]; // to keep reflection happy

            var instance = (LastAsyncCommandBase)Activator.CreateInstance(type, arguments);

            return instance.Method;
        }

        #endregion
    }
}
