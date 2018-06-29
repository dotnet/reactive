// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_THREAD
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    internal sealed class Thread
    {
        private readonly ThreadStart _start;

        public Thread(ThreadStart start)
        {
            _start = start;
        }

        public string Name { get; set; }
        public bool IsBackground { get; set; }

        public void Start()
        {
            Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }

        private void Run() => _start();
    }

    internal delegate void ThreadStart();
}
#endif
