// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// WARNING: The full namespace-qualified type name should stay the same for the discovery in System.Reactive.Core to work!
//
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Provider for platform-specific framework enlightenments.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CurrentPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
    {
        /// <summary>
        /// (Infrastructure) Tries to gets the specified service.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="args">Optional set of arguments.</param>
        /// <returns>Service instance or <c>null</c> if not found.</returns>
        public virtual T GetService<T>(object[] args) where T : class
        {
            var t = typeof(T);

            if (t == typeof(IExceptionServices))
            {
                return (T)(object)new ExceptionServicesImpl();
            }

#if !NO_THREAD || WINDOWS
            if (t == typeof(IConcurrencyAbstractionLayer))
            {
                return (T)(object)new ConcurrencyAbstractionLayerImpl();
            }
#endif

            if (t == typeof(IScheduler) && args != null)
            {
                switch ((string)args[0])
                {
                    case "ThreadPool":
                        return (T)(object)ThreadPoolScheduler.Instance;
                    case "TaskPool":
                        return (T)(object)TaskPoolScheduler.Default;
                    case "NewThread":
                        return (T)(object)NewThreadScheduler.Default;
                }
            }

#if WINDOWS
            if (t == typeof(IHostLifecycleNotifications))
            {
                return (T)(object)new HostLifecycleNotifications();
            }
#endif

            if (t == typeof(IQueryServices))
            {
                //
                // We perform this Debugger.IsAttached check early rather than deferring
                // the decision to intercept query operator methods to the debugger
                // assembly that's dynamically discovered here. Also, it's a reasonable
                // expectation it'd be pretty hard to turn on interception dynamically
                // upon a debugger attach event, so we should make this check early.
                //
                // In the initial release of v2.0 (RTM), we won't have the corresponding
                // debugger assembly available yet, so the dynamic load would always
                // fail. We also don't want to take the price of (an attempt to) a dynamic
                // assembly load for the regular production case.
                //
                if (Debugger.IsAttached)
                {

#if (CRIPPLED_REFLECTION && HAS_WINRT)
                    var ifType = t.GetTypeInfo();
#else
                    var ifType = t;
#endif
                    var asm = new AssemblyName(ifType.Assembly.FullName)
                    {
                        Name = "System.Reactive"
                    };
                    var name = "System.Reactive.Linq.QueryDebugger, " + asm.FullName;

                    var dbg = Type.GetType(name, false);
                    if (dbg != null)
                    {
                        return (T)Activator.CreateInstance(dbg);
                    }
                }
            }

            return null;
        }
    }
}
