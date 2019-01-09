using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Helpers
{
    public interface IPageResponse<out T> : ILastResponse, IEnumerable<T> where T : new()
    {
        IReadOnlyList<T> Content { get; }

        int Page { get; }

        int PageSize { get; }

        int TotalPages { get; }

        int TotalItems { get; }
        
        double? From { get; }
        
        double? To { get; }
    }

    [JsonConverter(typeof(PageResponseJsonConverter))]
    public class PageResponse<T> : LastResponse, IPageResponse<T> where T : new()
    {
        private int? _totalItems;
        private int? _pageSize;

        public PageResponse() : this(Enumerable.Empty<T>())
        {
        }

        public PageResponse(IEnumerable<T> content)
        {
            Page = 1;
            TotalPages = 1;
            Content = new ReadOnlyCollection<T>(content.ToList());
        }

        public IReadOnlyList<T> Content { get; internal set; }

        public int Page { get; internal set; }

        public int TotalPages { get; internal set; }

        public int TotalItems
        {
            get { return _totalItems ?? Content.CountOrDefault(); }
            internal set { _totalItems = value; }
        }

        public int PageSize
        {
            get { return _pageSize ?? Content.CountOrDefault(); }
            internal set { _pageSize = value; }
        }

        public double? From { get; internal set; }

        public double? To { get; internal set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Content != null
                ? Content.GetEnumerator()
                : null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Content != null)
            {
                return Content.GetEnumerator();
            }
            else
            {
                return null;
            }
        }

        public static PageResponse<T> CreateErrorResponse(LastResponseStatus status)
        {
            var r = new PageResponse<T>
            {
                Status = status
            };

            r.AddDefaultPageInfo();

            return r;
        }


        public new static PageResponse<T> CreateSuccessResponse()
        {
            var r = new PageResponse<T>
            {
                Status = LastResponseStatus.Successful
            };

            r.AddDefaultPageInfo();

            return r;
        }

        [Obsolete]
        public static PageResponse<T> CreateSuccessResponse(IEnumerable<T> content)
        {
            var r = new PageResponse<T>(content)
            {
                Status = LastResponseStatus.Successful
            };
            
            return r;
        }

        public static PageResponse<T> CreateSuccessResponse(JToken itemsToken, Func<JToken, T> parseToken)
        {
            return CreateSuccessResponse(itemsToken, null, parseToken, LastPageResultsType.None);
        }

        public static IEnumerable<T> ParseItemsToken(JToken itemsToken, Func<JToken, T> parseToken)
        {
            IEnumerable<T> items;
            if (itemsToken != null && itemsToken.Children().Any())
            {
                // array notation isn't used on the api when only one object is available
                if (itemsToken.Type == JTokenType.Object)
                {
                    items = new[] { parseToken(itemsToken) };
                }
                else if (itemsToken.Type == JTokenType.Array)
                {
                    items = itemsToken.Children().Select(parseToken);
                }
                else
                {
                    throw new ArgumentException(String.Format("Couldn't parse items token\r\n\r\n{0}", itemsToken.ToString()));
                }
            }
            else
            {
                items = Enumerable.Empty<T>();
            }

            return items;
        }
        
        public static PageResponse<T> CreateSuccessResponse(JToken itemsToken, JToken pageInfoToken, Func<JToken, T> parseToken, LastPageResultsType pageResultsType)
        {
            var items = ParseItemsToken(itemsToken, parseToken);

            var pageresponse = new PageResponse<T>(items)
            {
                Status = LastResponseStatus.Successful
            };

            switch (pageResultsType)
            {
                case LastPageResultsType.Attr:
                    pageresponse.AddPageInfoFromJToken(pageInfoToken);
                    break;
                case LastPageResultsType.OpenQuery:
                    pageresponse.AddPageInfoFromOpenQueryJToken(pageInfoToken);
                    break;
                case LastPageResultsType.None:
                default:
                    pageresponse.AddDefaultPageInfo(pageresponse.Content);
                    break;
            }

            return pageresponse;
        }

        private void AddDefaultPageInfo()
        {
            AddDefaultPageInfo(Enumerable.Empty<T>().ToList());
        }

        private void AddDefaultPageInfo(IReadOnlyCollection<T> items)
        {
            Page = 1;
            TotalPages = 1;
            TotalItems = items.Count;
            PageSize = items.Count;
        }

        internal void AddPageInfoFromJToken(JToken attrToken)
        {
            if (attrToken == null)
            {
                return;
            }

            var page = attrToken.Value<string>("page");
            Page = !string.IsNullOrWhiteSpace(page) ? Convert.ToInt32(page) : 1;

            var totalPages = attrToken.Value<string>("totalPages");
            TotalPages = !string.IsNullOrWhiteSpace(totalPages) ? Convert.ToInt32(totalPages) : 1;

            var totalItems = attrToken.Value<string>("total");
            TotalItems = !string.IsNullOrWhiteSpace(totalItems) ? Convert.ToInt32(totalItems) : 1;

            var pagesize = attrToken.Value<string>("perPage");
            PageSize = !string.IsNullOrWhiteSpace(pagesize) ? Convert.ToInt32(pagesize) : 1;

            var from = attrToken.Value<string>("from");
            From = !string.IsNullOrWhiteSpace(from) ? Convert.ToDouble(from) : (double?)null;

            var to = attrToken.Value<string>("to");
            To = !string.IsNullOrWhiteSpace(to) ? Convert.ToDouble(to) : (double?)null;
        }

        private void AddPageInfoFromOpenQueryJToken(JToken queryToken)
        {
            if (queryToken == null)
            {
                return;
            }

            var page = queryToken.SelectToken("opensearch:Query").Value<string>("startPage");
            Page = !string.IsNullOrWhiteSpace(page) ? Convert.ToInt32(page) : 1;

            var totalItems = queryToken.Value<string>("opensearch:totalResults");
            TotalItems = !string.IsNullOrWhiteSpace(totalItems) ? Convert.ToInt32(totalItems) : 1;

            var pagesize = queryToken.Value<string>("opensearch:itemsPerPage");
            PageSize = !string.IsNullOrWhiteSpace(pagesize) ? Convert.ToInt32(pagesize) : 1;

            // the response doesn't include total pages, bit of improv then.
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }
}