// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Subjects
{
    public sealed class SequentialReplayAsyncSubject<T> : ReplayAsyncSubject<T>
    {
        public SequentialReplayAsyncSubject()
            : base(false)
        {
        }

        public SequentialReplayAsyncSubject(int bufferSize)
            : base(false, bufferSize)
        {
        }

        public SequentialReplayAsyncSubject(IAsyncScheduler scheduler)
            : base(false, scheduler)
        {
        }

        public SequentialReplayAsyncSubject(int bufferSize, IAsyncScheduler scheduler)
            : base(false, bufferSize, scheduler)
        {
        }

        public SequentialReplayAsyncSubject(TimeSpan window)
            : base(false, window)
        {
        }

        public SequentialReplayAsyncSubject(TimeSpan window, IAsyncScheduler scheduler)
            : base(false, window, scheduler)
        {
        }

        public SequentialReplayAsyncSubject(int bufferSize, TimeSpan window)
            : base(false, bufferSize, window)
        {
        }

        public SequentialReplayAsyncSubject(int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
            : base(false, bufferSize, window, scheduler)
        {
        }
    }
}
