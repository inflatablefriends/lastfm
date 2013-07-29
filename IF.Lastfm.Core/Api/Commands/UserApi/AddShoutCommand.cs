using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Commands.UserApi
{
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public string Recipient { get; set; }
        public string Message { get; set; }

        public AddShoutCommand(IAuth auth, string recipient, string message) : base(auth)
        {
            Method = "user.shout";
            Recipient = recipient;
            Message = message;
        }

        public async override Task<LastResponse> ExecuteAsync()
        {
            var parameters = new Dictionary<string, string>
                             {
                                 {"user", Recipient},
                                 {"message", Message}
                             };

            return await ExecuteInternal(parameters);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
