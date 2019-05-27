// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Generates a sequence that's dependent on a resource object whose lifetime is determined by the sequence usage
        /// duration.
        /// </summary>
        /// <typeparam name="TSource">Source element type.</typeparam>
        /// <typeparam name="TResource">Resource type.</typeparam>
        /// <param name="resourceFactory">Resource factory function.</param>
        /// <param name="enumerableFactory">Enumerable factory function, having access to the obtained resource.</param>
        /// <returns>Sequence whose use controls the lifetime of the associated obtained resource.</returns>
        public static IEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw new ArgumentNullException(nameof(enumerableFactory));

            return UsingCore(resourceFactory, enumerableFactory);
        }

        private static IEnumerable<TSource> UsingCore<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            using var res = resourceFactory();

            foreach (var item in enumerableFactory(res))
            {
                yield return item;
            }
        }
    }
}
