// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public interface IAsyncScheduler : IClock
    {
        Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action);
        Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action, TimeSpan dueTime);
        Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action, DateTimeOffset dueTime);
    }
}
