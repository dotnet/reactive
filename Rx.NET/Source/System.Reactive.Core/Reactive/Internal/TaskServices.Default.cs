// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL

using System.Threading;
using System.Threading.Tasks;

#if HAS_TPL46
namespace System.Reactive.PlatformServices
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal class DefaultTaskServices/*Impl*/ : ITaskServices
    {
        public bool TrySetCanceled<T>(TaskCompletionSource<T> tcs, CancellationToken token)
        {
            return tcs.TrySetCanceled(token);
        }
    }
}
#else
namespace System.Reactive.PlatformServices
{
    internal class DefaultTaskServices : ITaskServices
    {
        public bool TrySetCanceled<T>(TaskCompletionSource<T> tcs, CancellationToken token)
        {
            return tcs.TrySetCanceled();
        }
    }
}
#endif

#endif