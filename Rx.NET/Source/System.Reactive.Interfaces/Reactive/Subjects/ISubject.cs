// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
