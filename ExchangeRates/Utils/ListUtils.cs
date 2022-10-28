using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRates.Utils
{
    public static class ListUtils
    {
        public static void Remove<T>(this IList<T> list, Type type)
        {
            list.Where(x => x.GetType() == type)
                .ToList()
                .ForEach(obj => list.Remove(obj));
        }
    }
}