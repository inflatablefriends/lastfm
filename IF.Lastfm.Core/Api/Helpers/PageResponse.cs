using System;
using System.Collections;
using System.Collections.Generic;
using IF.Lastfm.Core.Api.Enums;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class PageResponse<T> : LastResponse, IEnumerable<T>
    {
        public PageResponse()
        {
            Page = 1;
            TotalPages = 1;
        }

        #region Properties

        public IEnumerable<T> Content { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }

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

            var totalItems = attrToken.Value<string>("total");
            TotalItems = !string.IsNullOrWhiteSpace(totalItems) ? Convert.ToInt32(totalItems) : 1;

            var pagesize = attrToken.Value<string>("perPage");
            PageSize = !string.IsNullOrWhiteSpace(pagesize) ? Convert.ToInt32(pagesize) : 1;
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