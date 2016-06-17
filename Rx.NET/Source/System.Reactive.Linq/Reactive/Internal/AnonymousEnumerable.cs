// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive
{
    internal sealed class AnonymousEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> getEnumerator;

        public AnonymousEnumerable(Func<IEnumerator<T>> getEnumerator)
        {
            this.getEnumerator = getEnumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return getEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
