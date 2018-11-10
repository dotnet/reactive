// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

// See https://github.com/dotnet/csharplang/blob/master/proposals/async-streams.md for the definition of this interface
// and the design rationale. (8/30/2017)

#if !HAS_ASYNCDISPOSABLE

using System.Threading.Tasks;

namespace System
{
    public interface IAsyncDisposable
    {
        ValueTask DisposeAsync();
    }
}

#endif
