// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public abstract class AsyncObserverBase<T> : IAsyncObserver<T>
    {
        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        protected abstract Task OnCompletedAsyncCore();

        public Task OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            throw new NotImplementedException();
        }

        protected abstract Task OnErrorAsyncCore(Exception error);

        public Task OnNextAsync(T value)
        {
            throw new NotImplementedException();
        }

        protected abstract Task OnNextAsyncCore(T value);
    }
}
