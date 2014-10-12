using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class PageResponse<T> : LastResponse, IEnumerable<T> where T : new()
    {
        private int? _totalItems;
        private int? _pageSize;

        public PageResponse()
        {
            Page = 1;
            TotalPages = 1;
        }

        #region Properties

        public IEnumerable<T> Content { get; internal set; }

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
            if (Content != null)
            {
                return Content.GetEnumerator();
            }
            else
            {
                return null;
            }
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

        public new static PageResponse<T> CreateSuccessResponse()
        {
            var r = new PageResponse<T>
            {
                Success = true,
                Error = LastFmApiError.None
            };

            return r;
        }

        [Obsolete]
        public static PageResponse<T> CreateSuccessResponse(IEnumerable<T> content)
        {
            var r = new PageResponse<T>
            {
                Content = content,
                Success = true,
                Error = LastFmApiError.None
            };

            return r;
        }
        
        public static PageResponse<T> CreateSuccessResponse(JToken itemsToken, JToken pageInfoToken, Func<JToken, T> parseToken, bool isOpenQueryToken = false)
        {
            var pageresponse = CreateSuccessResponse();
            var content = new List<T>();

            if (itemsToken.Children().Any())
            {
                // array notation isn't used on the api when only one object is available
                if (itemsToken.Type != JTokenType.Array)
                {
                    var item = parseToken(itemsToken);
                    content.Add(item);
                }
                else
                {
                    var items = itemsToken.Children().Select(parseToken);
                    content.AddRange(items);
                }
            }

            if (pageInfoToken != null)
            {
                if (isOpenQueryToken)
                {
                    pageresponse.AddPageInfoFromOpenQueryJToken(pageInfoToken);
                }
                else
                {
                    pageresponse.AddPageInfoFromJToken(pageInfoToken);
                }
            }
            else
            {
                pageresponse.AddDefaultPageInfo(content.Count);
            }

            pageresponse.Content = content;

            return pageresponse;
        }

        private void AddDefaultPageInfo(int count)
        {
            Page = 1;
            TotalPages = 1;
            TotalItems = count;
            PageSize = count;
        }

        #endregion

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