// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.PlatformServices;

namespace System.Reactive.Linq
{
    internal static class QueryServices
    {
        private static IQueryServices s_services = Initialize();

        public static T GetQueryImpl<T>(T defaultInstance)
        {
            return s_services.Extend(defaultInstance);
        }

        private static IQueryServices Initialize()
        {
            return PlatformEnlightenmentProvider.Current.GetService<IQueryServices>() ?? new DefaultQueryServices();
        }
    }

    internal interface IQueryServices
    {
        T Extend<T>(T baseImpl);
    }

    class DefaultQueryServices : IQueryServices
    {
        public T Extend<T>(T baseImpl)
        {
            return baseImpl;
        }
    }
}
