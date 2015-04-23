using System.Collections.Generic;
using System.Linq;

namespace IF.Lastfm.Core.Helpers
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/a/419063
        /// </summary>
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int max)
        {
            List<T> toReturn = new List<T>(max);
            foreach (var item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == max)
                {
                    yield return toReturn;
                    toReturn = new List<T>(max);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }
    }
}