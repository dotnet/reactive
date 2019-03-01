// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace System.Linq
{
    /// <summary>
    /// Interface for an awaiter of an asynchronous operation.
    /// </summary>
    public interface IAwaiter : ICriticalNotifyCompletion
    {
        /// <summary>
        /// Gets a Boolean value indicating whether the asynchronous operation has completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets the result of the asynchronous operation.
        /// </summary>
        /// <remarks>
        /// This method may cause blocking if invoked prior to <see cref="IsCompleted"/> returning <c>true</c>.
        /// Completion of the asynchronous operation can be observed using <see cref="INotifyCompletion.OnCompleted(Action)"/>.
        /// </remarks>
        void GetResult();
    }
}
