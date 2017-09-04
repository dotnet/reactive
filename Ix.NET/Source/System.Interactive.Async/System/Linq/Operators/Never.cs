// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TValue> Never<TValue>()
        {
            //
            // REVIEW: The C# 8.0 proposed interfaces don't allow for cancellation, so this "Never" is
            //         as never as never can be; it can't be interrupted *at all*, similar to the sync
            //         variant in Ix. Passing a *hot* CancellationToken to the Never operator doesn't
            //         seem correct either, given that we return a *cold* sequence.
            //

            var tcs = new TaskCompletionSource<bool>();

            return AsyncEnumerable.CreateEnumerable(() => AsyncEnumerable.CreateEnumerator<TValue>(() => tcs.Task, current: null, dispose: null));
        }
    }
}
