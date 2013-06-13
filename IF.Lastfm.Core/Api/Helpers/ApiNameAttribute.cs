using System;

namespace IF.Lastfm.Core.Api.Helpers
{
    public class ApiNameAttribute : Attribute
    {
        public string Text { get; private set; }

        public ApiNameAttribute(string name)
        {
            Text = name;
        }
    }
}