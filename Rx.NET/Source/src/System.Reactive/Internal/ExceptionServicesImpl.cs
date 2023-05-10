// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace System.Reactive.PlatformServices
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal sealed class /*Default*/ExceptionServicesImpl : IExceptionServices
    {
#if NO_NULLABLE_ATTRIBUTES
#pragma warning disable CS8763 // NB: On down-level platforms, Throw is not marked as DoesNotReturn.
#endif
        [DoesNotReturn]
        public void Rethrow(Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
#if NO_NULLABLE_ATTRIBUTES
#pragma warning restore CS8763
#endif
    }
}
