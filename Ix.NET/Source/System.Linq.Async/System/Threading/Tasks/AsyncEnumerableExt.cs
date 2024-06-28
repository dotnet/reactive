// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal static class AsyncEnumerableExt
    {
#if NET6_0_OR_GREATER
#pragma warning disable CA1068 // CancellationToken parameters must come last. The arguments in favour of doing that also apply to the continueOnCapturedContext parameter
#endif
        public static ConfiguredCancelableAsyncEnumerable<T>.Enumerator GetConfiguredAsyncEnumerator<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken, bool continueOnCapturedContext)
#if NET6_0_OR_GREATER
#pragma warning restore CA1068
#endif
        {
            return enumerable.ConfigureAwait(continueOnCapturedContext).WithCancellation(cancellationToken).GetAsyncEnumerator();
        }
    }
}
