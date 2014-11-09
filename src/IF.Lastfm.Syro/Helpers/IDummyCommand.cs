using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Syro.Helpers
{
    public interface IDummyCommand
    {
        JObject Response { get; }
    }
}