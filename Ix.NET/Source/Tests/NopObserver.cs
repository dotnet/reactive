// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_RXINTERFACES
using System;

namespace Tests
{
    public class NopObserver<T> : IObserver<T>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }
}
#endif