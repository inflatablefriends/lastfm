using System;
using System.Collections;
using System.Collections.Generic;
using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class PageResponse<T> : IEnumerable<T>
    {
        private int _page  = 1;
        private int _totalPages = 1;

        #region Properties

        public IEnumerable<T> Content { get; set; }
        public bool Success { get; set; }
        public LastFmApiError Error { get; set; }

        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
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

        public static PageResponse<T> CreateErrorResponse(LastFmApiError error)
        {
            var r = new PageResponse<T>
            {
                Content = new[] {default(T)},
                Success = false,
                Error = error
            };

            return r;
        }

        #endregion

        public void AddPageInfoFromJToken(JToken attrToken)
        {
            if (attrToken == null)
            {
                return;
            }

            var page = attrToken.Value<string>("page");
            Page = !string.IsNullOrWhiteSpace(page) ? Convert.ToInt32(page) : 1;

            var totalPages = attrToken.Value<string>("totalPages");
            TotalPages = !string.IsNullOrWhiteSpace(totalPages) ? Convert.ToInt32(totalPages) : 1;
        }

//        {"@attr": {
//  "user": "tehrikkit",
//  "page": "",
//  "perPage": "",
//  "totalPages": "",
//  "total": "15"
//}}
    }
}