// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_STOPWATCH
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

        public TimeSpan Elapsed
        {
            get { return _sw.Elapsed; }
        }
    }
}
#endif