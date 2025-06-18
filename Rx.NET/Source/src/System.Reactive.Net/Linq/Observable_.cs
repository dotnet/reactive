// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>
    public static partial class Observable
    {
#pragma warning disable IDE1006 // Naming Styles: 3rd party code is known to reflect for this specific field name
#pragma warning disable IDE0044 // Make readonly: since 3rd party code reflects for this, we shouldn't pretend it won't change
        private static IQueryLanguage s_impl = QueryServices.GetQueryImpl<IQueryLanguage>(new QueryLanguage());
#pragma warning restore IDE1006, IDE0044 // Naming Styles, Make readonly
    }
}
