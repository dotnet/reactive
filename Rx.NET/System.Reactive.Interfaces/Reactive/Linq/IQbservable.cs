// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_EXPRESSIONS
using System.Linq.Expressions;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides functionality to evaluate queries against a specific data source wherein the type of the data is known.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data in the data source.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Justification = "What a pleasure to write 'by design' here.")]
    public interface IQbservable<
#if !NO_VARIANCE
        out
#endif
        T> : IQbservable, IObservable<T>
    {
    }

    /// <summary>
    /// Provides functionality to evaluate queries against a specific data source wherein the type of the data is not specified.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Justification = "What a pleasure to write 'by design' here.")]
    public interface IQbservable
    {
        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQbservable is executed.
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of IQbservable.
        /// </summary>
        Expression Expression { get; }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        IQbservableProvider Provider { get; }
    }
}
#endif