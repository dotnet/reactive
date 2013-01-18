// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    interface IEvaluatableObservable<T>
    {
        IObservable<T> Eval();
    }
}
