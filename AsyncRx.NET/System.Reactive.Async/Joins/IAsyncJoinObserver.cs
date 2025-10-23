﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal interface IAsyncJoinObserver : IAsyncDisposable
    {
        Task SubscribeAsync(IAsyncGate gate);

        void Dequeue();
    }
}
