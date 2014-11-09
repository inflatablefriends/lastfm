using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IF.Lastfm.Syro.Helpers
{
    public class DummyGetAsyncCommand<T> : GetAsyncCommandBase<T>, IDummyCommand where T : LastResponse, new()
    {
        public JObject Response { get; set; }

        public DummyGetAsyncCommand(ILastAuth auth)
            : base(auth)
        {
        }

        public override void SetParameters()
        {
        }

        public async override Task<T> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            Response = JObject.Parse(json);

            return null;
        }
    }
}