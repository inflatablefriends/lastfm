using IF.Lastfm.Core.Api.Helpers;

namespace IF.Lastfm.Core.Api.Enums
{
    public enum LastStatsTimeSpan
    {
        [ApiName("overall")]
        Overall,

        [ApiName("7day")]
        Week,

        [ApiName("1month")]
        Month,

        [ApiName("3month")]
        Quarter,

        [ApiName("6month")]
        Half,

        [ApiName("12month")]
        Year
    }

    public enum Gender
    {
        Other = 0,
        Male,
        Female
    }
}