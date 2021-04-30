using System;
using System.Collections.Generic;

namespace Reega.Shared.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T v in source) action(v);
        }
    }
}
