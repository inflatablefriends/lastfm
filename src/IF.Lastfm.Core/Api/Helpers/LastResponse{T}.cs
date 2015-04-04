using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class LastResponse<T> : LastResponse
    {
        public T Content { get; set; }
        
        public static LastResponse<T> CreateSuccessResponse(T content)
        {
            var r = new LastResponse<T>
            {
                Content = content,
                Status = LastResponseStatus.Successful
            };

            return r;
        }
    }
}