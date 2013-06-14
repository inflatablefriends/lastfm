using System.Collections;
using System.Collections.Generic;
using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class ListResponse<T> : IEnumerable<T>
    {
        #region Properties

        public IEnumerable<T> Content { get; set; }
        public bool Success { get; set; }
        public LastFmApiError Error { get; set; }

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

        public static ListResponse<T> CreateSuccessResponse(IEnumerable<T> content)
        {
            var r = new ListResponse<T>
            {
                Content = content,
                Success = true,
                Error = LastFmApiError.None
            };

            return r;
        }

        public static ListResponse<T> CreateErrorResponse(LastFmApiError error)
        {
            var r = new ListResponse<T>
            {
                Content = new[] {default(T)},
                Success = false,
                Error = error
            };

            return r;
        }

        #endregion
    }
}