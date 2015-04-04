using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;

namespace IF.Lastfm.Core.Api.Helpers
{
    public interface ILastResponse
    {
        LastResponseStatus Status { get; }
    }

    public class LastResponse : ILastResponse
    {
        public bool Success
        {
            get { return Status == LastResponseStatus.Successful; }
        }

        public LastResponseStatus Status { get; internal set; }

        [Obsolete("This property has been renamed to Status and will be removed soon.")]
        public LastResponseStatus Error { get { return Status; } }
        
        public static LastResponse CreateSuccessResponse()
        {
            var r = new LastResponse
            {
                Status = LastResponseStatus.Successful
            };

            return r;
        }

        public static T CreateErrorResponse<T>(LastResponseStatus status) where T : LastResponse, new()
        {
            var r = new T
            {
                Status = status
            };

            return r;
        }

        public async static Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                return LastResponse.CreateSuccessResponse();
            }
            else
            {
                return LastResponse.CreateErrorResponse<LastResponse>(status);
            }
        }
    }
}