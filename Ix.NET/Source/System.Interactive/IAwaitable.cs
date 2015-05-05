// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if HAS_AWAIT
using System.Runtime.CompilerServices;

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