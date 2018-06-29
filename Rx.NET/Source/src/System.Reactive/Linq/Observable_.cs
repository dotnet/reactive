// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>
    public static partial class Observable
    {
#pragma warning disable IDE1006 // Naming Styles: 3rd party code is known to reflect for this specific field name
        private static IQueryLanguage s_impl = QueryServices.GetQueryImpl<IQueryLanguage>(new QueryLanguage());
#pragma warning restore IDE1006 // Naming Styles
    }
}
