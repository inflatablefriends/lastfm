using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class LastResponse<T>
    {
        #region Properties

        public T Content { get; set; }
        public bool Success { get; set; }
        public LastFmApiError Error { get; set; }

        #endregion

        #region Factory methods

        public static LastResponse<T> CreateSuccessResponse(T content)
        {
            var r = new LastResponse<T>
            {
                Content = content,
                Success = true,
                Error = LastFmApiError.None
            };

            return r;
        }

        public static LastResponse<T> CreateErrorResponse(LastFmApiError error)
        {
            var r = new LastResponse<T>
            {
                Content = default(T),
                Success = false,
                Error = error
            };

            return r;
        }

        #endregion
    }
}