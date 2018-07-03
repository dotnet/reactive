// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarks.System.Interactive
{
    /// <summary>
    /// Some helper extension methods to allow the same pattern to be used
    /// in both Rx and Ix benchmarks
    /// </summary>
    internal static class BenchmarkInterop
    {
        internal static void Subscribe<T>(this IEnumerable<T> enumerable, Action<T> onNext)
        {
            foreach (var v in enumerable)
            {
                onNext(v);
            }
        }

        internal static void Subscribe<T>(this IEnumerable<T> enumerable, Action<T> onNext, Action onCompleted)
        {
            foreach (var v in enumerable)
            {
                onNext(v);
            }
            onCompleted();
        }
    }
}
