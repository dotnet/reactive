// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Subjects
{
    public sealed class ConcurrentReplayAsyncSubject<T> : ReplayAsyncSubject<T>
    {
        public ConcurrentReplayAsyncSubject()
            : base(true)
        {
        }

        public ConcurrentReplayAsyncSubject(int bufferSize)
            : base(true, bufferSize)
        {
        }

        public ConcurrentReplayAsyncSubject(IAsyncScheduler scheduler)
            : base(true, scheduler)
        {
        }

        public ConcurrentReplayAsyncSubject(int bufferSize, IAsyncScheduler scheduler)
            : base(true, bufferSize, scheduler)
        {
        }

        public ConcurrentReplayAsyncSubject(TimeSpan window)
            : base(false, window)
        {
        }

        public ConcurrentReplayAsyncSubject(TimeSpan window, IAsyncScheduler scheduler)
            : base(false, window, scheduler)
        {
        }

        public ConcurrentReplayAsyncSubject(int bufferSize, TimeSpan window)
            : base(false, bufferSize, window)
        {
        }

        public ConcurrentReplayAsyncSubject(int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
            : base(false, bufferSize, window, scheduler)
        {
        }
    }
}
