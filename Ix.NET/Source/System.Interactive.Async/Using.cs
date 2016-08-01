// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (enumerableFactory == null)
                throw new ArgumentNullException(nameof(enumerableFactory));

            return CreateEnumerable(
                () =>
                {
                    var resource = resourceFactory();
                    var e = default(IAsyncEnumerator<TSource>);

                    try
                    {
                        e = enumerableFactory(resource)
                            .GetEnumerator();
                    }
                    catch (Exception)
                    {
                        resource.Dispose();
                        throw;
                    }

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, resource, e);

                    var current = default(TSource);

                    return CreateEnumerator(
                        async ct =>
                        {
                            bool res;
                            try
                            {
                                res = await e.MoveNext(cts.Token)
                                             .ConfigureAwait(false);
                            }
                            catch (Exception)
                            {
                                d.Dispose();
                                throw;
                            }
                            if (res)
                            {
                                current = e.Current;
                                return true;
                            }
                            d.Dispose();
                            return false;
                        },
                        () => current,
                        d.Dispose,
                        d
                    );
                });
        }
    }
}