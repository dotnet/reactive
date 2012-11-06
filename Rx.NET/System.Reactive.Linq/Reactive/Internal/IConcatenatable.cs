// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace System.Reactive
{
    interface IConcatenatable<TSource>
    {
        IEnumerable<IObservable<TSource>> GetSources();
    }
}
