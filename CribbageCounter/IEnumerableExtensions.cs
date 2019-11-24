using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CribbageCounter {
    public static class IEnumerableExtensions {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> obj, Action<T> action) {
            foreach (var item in obj) {
                action(item);
            }
        }
    }
}