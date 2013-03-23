// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace System.Collections.Generic
{
    public static class AsyncEnumerator
    {
        /// <summary>
        /// Advances the enumerator to the next element in the sequence, returning the result asynchronously.
        /// </summary>
        /// <returns>
        /// Task containing the result of the operation: true if the enumerator was successfully advanced 
        /// to the next element; false if the enumerator has passed the end of the sequence.
        /// </returns>
        public static Task<bool> MoveNext<T>(this IAsyncEnumerator<T> enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            return enumerator.MoveNext(CancellationToken.None);
        }
    }
}
