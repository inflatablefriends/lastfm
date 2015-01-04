using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Syro.Helpers
{
    public class DummyPostAsyncCommand<T> : PostAsyncCommandBase<T>, IDummyCommand where T : LastResponse, new()
    {
        public JObject Response { get; private set; }
        public DummyPostAsyncCommand(ILastAuth auth) : base(auth)
        {
        }

        public override void SetParameters()
        {
            AddPagingParameters();
        }

        public async override Task<T> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            Response = JObject.Parse(json);

            return null;
        }
    }
}