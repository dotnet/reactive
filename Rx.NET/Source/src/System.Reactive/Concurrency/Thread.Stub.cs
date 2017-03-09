// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_THREAD
using System;
using System.Threading;

namespace System.Reactive.Concurrency
{
    class Thread
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
            System.Threading.Tasks.Task.Factory.StartNew(Run, System.Threading.Tasks.TaskCreationOptions.LongRunning);
        }

        private void Run()
        {
            _start();
        }
    }

    delegate void ThreadStart();
}
#endif