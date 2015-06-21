using IF.Lastfm.Core.Api.Commands;
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

    /// <summary>
    /// This attribute defines the api method name (i.e: "album.getInfo") for a Command.
    /// When applied on a <see cref="LastAsyncCommandBase"/> implementation, the <see cref="LastAsyncCommandBase.Method"/> property is set to the attribute value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ApiMethodNameAttribute : ApiNameAttribute
    {
        public ApiMethodNameAttribute(string name) : base(name) { }
    }
}