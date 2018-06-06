// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public interface ISubject<T> : ISubject<T, T>
    {
    }
}
