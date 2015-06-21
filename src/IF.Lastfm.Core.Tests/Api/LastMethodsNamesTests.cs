using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands;
using IF.Lastfm.Core.Api.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IF.Lastfm.Core.Tests.Api
{

    public class LastMethodsNamesTests
    {
        [Test]
        public void EnsureEachCommandHasAnApiMethodNameAttribute()
        {
            var commandBaseType = typeof(LastAsyncCommandBase);
            var allCommands = commandBaseType.Assembly.DefinedTypes
                .Where(t => t != commandBaseType && commandBaseType.IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract);

            var commandsWithoutAttribute = allCommands.Where(x => !x.GetCustomAttributes<ApiMethodNameAttribute>().Any());

            Assert.IsFalse(commandsWithoutAttribute.Any(), "Warning, all commands should have anApiMethodNameAttribute.");
        }
    }
}