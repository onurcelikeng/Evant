using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
