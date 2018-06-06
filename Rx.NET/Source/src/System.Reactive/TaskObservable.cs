// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace System.Reactive
{
    /// <summary>
    /// Extension of the <see cref="IObservable{T}"/> interface compatible with async method return types.
    /// </summary>
    /// <remarks>
    /// This class implements a "task-like" type that can be used as the return type of an asynchronous
    /// method in C# 7.0 and beyond. For example:
    /// <code>
    /// async ITaskObservable&lt;int&gt; RxAsync()
    /// {
    ///     var res = await Observable.Return(21).Delay(TimeSpan.FromSeconds(1));
    ///     return res * 2;
    /// }
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    [AsyncMethodBuilder(typeof(TaskObservableMethodBuilder<>))]
    public interface ITaskObservable<out T> : IObservable<T>
    {
        // NB: An interface type is preferred to enable the use of covariance.

        /// <summary>
        /// Gets an awaiter that can be used to await the eventual completion of the observable sequence.
        /// </summary>
        /// <returns>An awaiter that can be used to await the eventual completion of the observable sequence.</returns>
        ITaskObservableAwaiter<T> GetAwaiter();
    }

    /// <summary>
    /// Interface representing an awaiter for an <see cref="ITaskObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public interface ITaskObservableAwaiter<out T> : INotifyCompletion
    {
        /// <summary>
        /// Gets a Boolean indicating whether the observable sequence has completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets the result produced by the observable sequence.
        /// </summary>
        /// <returns>The result produced by the observable sequence.</returns>
        T GetResult();
    }
}
