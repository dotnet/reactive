// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;

namespace System.Reactive
{
    internal sealed class AnonymousEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _getEnumerator;

        public AnonymousEnumerable(Func<IEnumerator<T>> getEnumerator)
        {
            _getEnumerator = getEnumerator;
        }

        public IEnumerator<T> GetEnumerator() => _getEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _getEnumerator();
    }
}
