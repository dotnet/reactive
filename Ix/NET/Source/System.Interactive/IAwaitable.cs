// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if HAS_AWAIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public interface IAwaitable
    {
        IAwaiter GetAwaiter();
    }

    public interface IAwaiter : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }
}
#endif