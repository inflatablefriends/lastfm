using System;
using IF.Lastfm.Core.Api.Helpers;
using Newtonsoft.Json;

namespace IF.Lastfm.Core.Json
{
    public class PageResponseJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var pageResponse = (IPageResponse<object>) value;

            dynamic container = new
            {
                items = pageResponse.Content,
                page = new
                {
                    totalItems = pageResponse.TotalItems,
                    pageSize = pageResponse.PageSize,
                    page = pageResponse.Page,
                    totalPages = pageResponse.TotalPages
                }
            };

            serializer.Serialize(writer, container);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (PageResponse<>);
        }
    }
}