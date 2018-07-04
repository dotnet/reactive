// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        ///     Creates an enumerable sequence based on an enumerable factory function.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="enumerableFactory">Enumerable factory function.</param>
        /// <returns>Sequence that will invoke the enumerable factory upon a call to GetEnumerator.</returns>
        public static IEnumerable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
        {
            if (enumerableFactory == null)
            {
                throw new ArgumentNullException(nameof(enumerableFactory));
            }

            return Defer_(enumerableFactory);
        }

        private static IEnumerable<TSource> Defer_<TSource>(Func<IEnumerable<TSource>> enumerableFactory)
        {
            return new DeferEnumerable<TSource>(enumerableFactory);
        }

        private sealed class DeferEnumerable<TSource> : IEnumerable<TSource>
        {
            private readonly Func<IEnumerable<TSource>> _enumerableFactory;

            public DeferEnumerable(Func<IEnumerable<TSource>> enumerableFactory)
            {
                _enumerableFactory = enumerableFactory;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                return _enumerableFactory().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
