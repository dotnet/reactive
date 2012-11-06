// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace ReactiveTests.Dummies
{
    class DummyDisposable : IDisposable
    {
        public static readonly DummyDisposable Instance = new DummyDisposable();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
