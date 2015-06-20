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

    public class ApiMethodNameAttribute : ApiNameAttribute
    {
        public ApiMethodNameAttribute(string name) : base(name) { }
    }
}