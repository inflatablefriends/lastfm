using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class LastResponse<T> : LastResponse
    {
        #region Properties

        public T Content { get; set; }

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

        #endregion
    }
}