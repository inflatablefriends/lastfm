using System.Collections.Generic;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public interface ILastAuth
    {
        bool Authenticated { get; }
        string ApiKey { get; }
        LastUserSession UserSession { get; }

        /// <summary>
        /// Load an existing user session object.
        /// </summary>
        /// <param name="session"></param>
        /// <returns>Whether session object is valid</returns>
        bool LoadSession(LastUserSession session);

        /// <summary>
        /// Gets the session token which is used as authentication for any service calls.
        /// Username and password aren't stored.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">UserSession's password</param>
        /// <returns>Session token used to authenticate calls to last.fm</returns>
        /// <remarks>API: Auth.getMobileSession</remarks>
        Task<LastResponse> GetSessionTokenAsync(string username, string password);

        string GenerateMethodSignature(string method, Dictionary<string, string> parameters = null);
    }
}