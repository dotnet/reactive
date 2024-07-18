// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Threading
{
    public readonly struct AsyncGateReleaser : IDisposable
    {
        private readonly IAsyncGate _parent;

        public AsyncGateReleaser(IAsyncGate parent) => _parent = parent;

        public void Dispose() => _parent.Release();
    }
}
