// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;

namespace ReactiveTests.Dummies
{
    internal class DummyDisposable : IDisposable
    {
        public static readonly DummyDisposable Instance = new DummyDisposable();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
