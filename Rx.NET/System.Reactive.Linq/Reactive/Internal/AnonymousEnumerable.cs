// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
