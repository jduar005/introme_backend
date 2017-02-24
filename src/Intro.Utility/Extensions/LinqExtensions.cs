using System;
using System.Collections.Generic;

namespace Intro.Utility.Extensions
{
    public static class LinqExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}