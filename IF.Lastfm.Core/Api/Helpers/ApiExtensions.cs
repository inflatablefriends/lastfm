using System;

namespace IF.Lastfm.Core.Api.Helpers
{
    public static class ApiExtensions
    {
        public static string GetApiName(this Enum en)
        {
            var type = en.GetType();

            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof (ApiNameAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((ApiNameAttribute) attrs[0]).Text;
                }
            }

            return en.ToString();
        }
    }
}