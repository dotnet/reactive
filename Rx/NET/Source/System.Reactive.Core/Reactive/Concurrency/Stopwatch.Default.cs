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
    internal class DefaultStopwatch/*Impl*/ : IStopwatch
    {
        private readonly Stopwatch _sw;

        public DefaultStopwatch()
        {
            _sw = Stopwatch.StartNew();
        }

        public TimeSpan Elapsed
        {
            get { return _sw.Elapsed; }
        }
    }
}
#else
namespace System.Reactive.Concurrency
{
    // This class is only used on Silverlight in the browser. It mimicks !Stopwatch.HighResolution behavior and suffers from
    // use of absolute time. See work item 486045.
    internal class DefaultStopwatch : IStopwatch
    {
        private readonly DateTime _start;

        public DefaultStopwatch()
        {
            _start = DateTime.UtcNow;
        }

        public TimeSpan Elapsed
        {
            get { return DateTime.UtcNow - _start; }
        }
    }
}
#endif