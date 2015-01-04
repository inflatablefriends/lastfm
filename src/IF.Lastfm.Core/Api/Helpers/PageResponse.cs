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

        #region Properties

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

        #endregion

        #region IEnumerable

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

        #endregion

        #region Factory methods

        public static PageResponse<T> CreateErrorResponse(LastFmApiError error)
        {
            var r = new PageResponse<T>
            {
                Success = false,
                Error = error
            };

            r.AddDefaultPageInfo();

            return r;
        }


        public new static PageResponse<T> CreateSuccessResponse()
        {
            var r = new PageResponse<T>
            {
                Success = true,
                Error = LastFmApiError.None
            };

            r.AddDefaultPageInfo();

            return r;
        }

        [Obsolete]
        public static PageResponse<T> CreateSuccessResponse(IEnumerable<T> content)
        {
            var r = new PageResponse<T>(content)
            {
                Success = true,
                Error = LastFmApiError.None
            };
            
            return r;
        }

        public static PageResponse<T> CreateSuccessResponse(JToken itemsToken, Func<JToken, T> parseToken)
        {
            return CreateSuccessResponse(itemsToken, null, parseToken, LastPageResultsType.None);
        }
        
        public static PageResponse<T> CreateSuccessResponse(JToken itemsToken, JToken pageInfoToken, Func<JToken, T> parseToken, LastPageResultsType pageResultsType)
        {
            IEnumerable<T> items;
            if (itemsToken != null && itemsToken.Children().Any())
            {
                // array notation isn't used on the api when only one object is available
                if (itemsToken.Type != JTokenType.Array)
                {
                    var item = parseToken(itemsToken);
                    items = new[] {item};

                }
                else
                {
                    items = itemsToken.Children().Select(parseToken);
                }
            }
            else
            {
                items = Enumerable.Empty<T>();
            }

            var pageresponse = new PageResponse<T>(items);

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

            pageresponse.Success = true;

            return pageresponse;
        }

        #endregion

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