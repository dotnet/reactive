// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace System.Reactive.Concurrency
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal class /*Default*/StopwatchImpl : IStopwatch
    {
        private readonly Stopwatch _sw;

        public StopwatchImpl()
        {
            _sw = Stopwatch.StartNew();
        }

        public TimeSpan Elapsed => _sw.Elapsed;
    }
}
