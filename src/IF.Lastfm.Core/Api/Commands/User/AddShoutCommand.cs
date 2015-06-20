using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Enums;

namespace IF.Lastfm.Core.Api.Commands.User
{
    [ApiMethodName(LastMethodsNames.user_shout)]
    internal class AddShoutCommand : PostAsyncCommandBase<LastResponse>
    {
        public override string Method
        { get { return LastMethodsNames.user_shout; } }

        public string Recipient { get; set; }

        public string Message { get; set; }

        public AddShoutCommand(ILastAuth auth, string recipient, string message)
            : base(auth)
        {
            Recipient = recipient;
            Message = message;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Recipient);
            Parameters.Add("message", Message);
        }

        public async override Task<LastResponse> HandleResponse(HttpResponseMessage response)
        {
            return await LastResponse.HandleResponse(response);
        }
    }
}
