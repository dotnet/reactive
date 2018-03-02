// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    public class Clock : IClock
    {
        public static Clock Default { get; } = new Clock();

        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
