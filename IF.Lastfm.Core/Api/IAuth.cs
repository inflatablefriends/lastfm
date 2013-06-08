using System.Threading.Tasks;

namespace IF.Lastfm.Core.Api
{
    public interface IAuth
    {
        string ApiKey { get; }

        /// <summary>
        /// Gets the session token which is used as authentication for any service calls.
        /// Username and password aren't stored.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">User's password</param>
        /// <returns>Session token used to authenticate calls to last.fm</returns>
        Task<UserSession> GetSessionTokenAsync(string username, string password);
    }
}