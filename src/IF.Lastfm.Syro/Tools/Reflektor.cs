using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IF.Lastfm.Core.Api.Commands;

namespace IF.Lastfm.Syro.Tools
{
    public class Reflektor
    {
        /// <summary>
        /// With thanks to Tim Murphy
        /// http://stackoverflow.com/a/4529684/268555
        /// </summary>
        public static IEnumerable<Type> FindClassesCastableTo(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            return assembly.DefinedTypes.Where(t => type.IsAssignableFrom(t) && t != type).Select(t => t.AsType());
        }

        /// <summary>
        /// Reflect on implemented commands
        /// </summary>
        internal static IEnumerable<Type> GetImplementedCommands()
        {
            var types = FindClassesCastableTo(typeof(LastAsyncCommandBase))
                .Select(t => t.GetTypeInfo())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => t.AsType());

            return types;
        }

        public static LastAsyncCommandBase CreateCommand(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            // assuming there is only one constructor
            var constructor = typeInfo.DeclaredConstructors.First();
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Count()]; // to keep reflection happy

            var instance = (LastAsyncCommandBase)Activator.CreateInstance(type, arguments);

            return instance;
        }
    }
}