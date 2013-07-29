using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class LastResponse
    {
        #region Properties

        public bool Success { get; set; }
        public LastFmApiError Error { get; set; }

        #endregion

        #region Factory methods

        public static LastResponse CreateSuccessResponse()
        {
            var r = new LastResponse
            {
                Success = true,
                Error = LastFmApiError.None
            };

            return r;
        }

        public static LastResponse CreateErrorResponse(LastFmApiError error)
        {
            var r = new LastResponse
            {
                Success = false,
                Error = error
            };

            return r;
        }

        public async static Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastFmApiError error;
            if (LastFm.IsResponseValid(json, out error) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse(error);
            }
        }

        #endregion
    }
}