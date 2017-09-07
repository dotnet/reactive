// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public static class AsyncScheduler
    {
        // TODO: Implement proper RendezVous semantics.

        public static ConfiguredTaskAwaitable RendezVous(this Task task, IAsyncScheduler scheduler)
        {
            return task.ConfigureAwait(true);
        }

        public static ConfiguredTaskAwaitable<T> RendezVous<T>(this Task<T> task, IAsyncScheduler scheduler)
        {
            return task.ConfigureAwait(true);
        }
    }
}
