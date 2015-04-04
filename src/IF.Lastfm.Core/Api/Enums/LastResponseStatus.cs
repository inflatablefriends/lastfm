namespace IF.Lastfm.Core.Api.Enums
{
    public enum LastResponseStatus
    {
        Unknown = 0,

        /// <summary>
        /// The request was successful!
        /// </summary>
        Successful,

        /// <summary>
        /// The request has been cached, it will be sent later
        /// </summary>
        Cached,

        /// <summary>
        /// The request could not be sent, and could not be cached.
        /// Check the Exception property of the response for details.
        /// </summary>
        CacheFailed,

        /// <summary>
        /// The request failed, check for network connectivity
        /// </summary>
        RequestFailed,

        /// <summary>
        /// The service requested does not exist (2)
        /// </summary>
        BadService,

        /// <summary>
        /// The method requested does not exist in this service (3)
        /// </summary>
        BadMethod,

        /// <summary>
        /// This credential does not have permission to access the service requested (4)
        /// </summary>
        BadAuth,

        /// <summary>
        /// This service doesn't exist in the requested format
        /// </summary>
        BadFormat,

        /// <summary>
        /// Required parameters were missing from the request (6)
        /// </summary>
        MissingParameters,

        /// <summary>
        /// The requested resource is invalid (7)
        /// </summary>
        BadResource,

        /// <summary>
        /// An unknown failure occured when creating the response (8)
        /// </summary>
        Failure,

        /// <summary>
        /// The session has expired, reauthenticate before retrying (9)
        /// </summary>
        SessionExpired,

        /// <summary>
        /// The provided API key was invalid (10)
        /// </summary>
        BadApiKey,

        /// <summary>
        /// This service is temporarily offline, retry later (11)
        /// </summary>
        ServiceDown,

        /// <summary>
        /// The request signature was invalid. Check that your API key and secret are valid. (13)
        /// You can generate new keys at http://www.last.fm/api/accounts
        /// </summary>
        BadMethodSignature,

        /// <summary>
        /// There was a temporary error while processing the request, retry later (16)
        /// </summary>
        TemporaryFailure,

        /// <summary>
        /// This API key has been suspended, please generate a new key at http://www.last.fm/api/accounts (26)
        /// </summary>
        KeySuspended,

        /// <summary>
        /// This API key has been rate-limited because too many requests have been made in a short period. Retry later (29)
        /// </summary>
        RateLimited
    }
}