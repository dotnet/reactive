// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.PlatformServices;

namespace System.Reactive.Linq
{
    internal static class QueryServices
    {
        private static readonly IQueryServices s_services = Initialize();

        public static T GetQueryImpl<T>(T defaultInstance) => s_services.Extend(defaultInstance);

        private static IQueryServices Initialize()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return PlatformEnlightenmentProvider.Current.GetService<IQueryServices>() ?? new DefaultQueryServices();
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }

    internal interface IQueryServices
    {
        T Extend<T>(T baseImpl);
    }

    internal sealed class DefaultQueryServices : IQueryServices
    {
        public T Extend<T>(T baseImpl) => baseImpl;
    }
}
