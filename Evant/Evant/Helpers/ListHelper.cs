using System.Collections.Generic;

namespace Evant.Helpers
{
    public static class ListHelper
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return (list == null || list.Count < 1);
        }
    }
}
