// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public interface IAsyncScheduler : IClock
    {
        ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action);
        ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action, TimeSpan dueTime);
        ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action, DateTimeOffset dueTime);
    }
}
