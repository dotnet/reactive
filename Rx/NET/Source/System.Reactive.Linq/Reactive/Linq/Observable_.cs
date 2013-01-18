// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>
    public static partial class Observable
    {
        private static IQueryLanguage s_impl = QueryServices.GetQueryImpl<IQueryLanguage>(new QueryLanguage());
    }
}
