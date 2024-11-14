// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Disposables;

/// <summary>
/// Extension methods associated with the IDisposable interface.
/// </summary>
public static class DisposableMixins
{
    /// <summary>
    /// Ensures the provided disposable is disposed with the specified ICollection of IDisposable./&gt;.
    /// </summary>
    /// <typeparam name="T">The type of the disposable.</typeparam>
    /// <param name="item">The disposable we are going to want to be disposed by the disposable collection.</param>
    /// <param name="disposableCollection">The composite disposable.</param>
    /// <returns>
    /// The disposable.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">compositeDisposable</exception>
    public static T DisposeWith<T>(this T item, ICollection<IDisposable> disposableCollection)
        where T : IDisposable
    {
        if (disposableCollection == null)
        {
            throw new ArgumentNullException(nameof(disposableCollection));
        }

        disposableCollection.Add(item);
        return item;
    }
}
