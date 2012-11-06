// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.PlatformServices;

namespace System.Reactive.Linq
{
    internal static class QueryServices
    {
        private static Lazy<IQueryServices> s_services = new Lazy<IQueryServices>(Initialize);

        public static T GetQueryImpl<T>(T defaultInstance)
        {
            return s_services.Value.Extend(defaultInstance);
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
