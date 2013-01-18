// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if WINDOWSPHONE7
#if !DEBUG_NO_AGENT_SUPPORT
//
// The Windows Phone Marketplace Test Kit disallows usage of types
// in the Microsoft.Phone.Shell namespace, determined by static code
// analysis, for background agents.
//
// However, with a null check for PhoneApplicationService.Current,
// we can safely use this the lifecycle events for dormant state
// transitions. In a background agent this property will be null;
// trying to create an instance of PhoneApplicationService throws
// a ComException and is required to set the Current property.
//
// In order to access the PhoneApplicationService functionality for
// non-background agent assemblies, we build a late bound wrapper
// around the APIs.
//
// See appplat\src\Frameworks\Microsoft\Phone\PhoneApplicationService.cs
// for implementation details of the class we use.
//
namespace System.Reactive.PlatformServices.Phone.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.Phone;

    class PhoneApplicationService
    {
        private  static readonly object       s_gate = new object();
        internal static readonly Assembly     s_phshAsm;
        private  static readonly Type         s_pasType;
        private  static readonly PropertyInfo s_curProp;
        private  static readonly EventInfo    s_actdEvt;
        private  static readonly EventInfo    s_deacEvt;

        private         readonly object        _target;

        static PhoneApplicationService()
        {
            s_phshAsm = typeof(BackgroundAgent).Assembly; // Has to be in Microsoft.Phone.dll.
            s_pasType = s_phshAsm.GetType("Microsoft.Phone.Shell.PhoneApplicationService");
            s_curProp = s_pasType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
            s_actdEvt = s_curProp.PropertyType.GetEvent("Activated", BindingFlags.Public | BindingFlags.Instance);
            s_deacEvt = s_curProp.PropertyType.GetEvent("Deactivated", BindingFlags.Public | BindingFlags.Instance);
        }

        private PhoneApplicationService(object target)
        {
            _target = target;
        }

        private static PhoneApplicationService s_current;

        public static PhoneApplicationService Current
        {
            get
            {
                lock (s_gate)
                {
                    if (s_current == null)
                    {
                        var current = s_curProp.GetValue(null, null);
                        if (current == null)
                            return null;

                        //
                        // Current provides a singleton. The constructor
                        // of PhoneApplicationService guarantees this,
                        // throwing an InvalidOperationException if more
                        // than one instance is created.
                        //
                        s_current = new PhoneApplicationService(current);
                    }
                }

                return s_current;
            }
        }

        private Dictionary<object, object> _actdHandlers = new Dictionary<object, object>();

        public event EventHandler<ActivatedEventArgs> Activated
        {
            add
            {
                AddHandler<ActivatedEventArgs>(s_actdEvt, _actdHandlers, value);
            }

            remove
            {
                RemoveHandler(s_actdEvt, _actdHandlers, value);
            }
        }

        private Dictionary<object, object> _deacHandlers = new Dictionary<object, object>();

        public event EventHandler<DeactivatedEventArgs> Deactivated
        {
            add
            {
                AddHandler<DeactivatedEventArgs>(s_deacEvt, _deacHandlers, value);
            }

            remove
            {
                RemoveHandler(s_deacEvt, _deacHandlers, value);
            }
        }

        private void AddHandler<TEventArgs>(EventInfo evt, Dictionary<object, object> map, object handler)
            where TEventArgs : EventArgs
        {
            var h = GetHandler<TEventArgs>(evt, handler);
            var add = evt.GetAddMethod();

            lock (s_gate)
            {
                map.Add(handler, h);
                add.Invoke(_target, new object[] { h });
            }
        }

        private void RemoveHandler(EventInfo evt, Dictionary<object, object> map, object handler)
        {
            var rem = evt.GetRemoveMethod();

            lock (s_gate)
            {
                var h = default(object);
                if (map.TryGetValue(handler, out h))
                {
                    //
                    // We assume only one handler will be attached to
                    // the event, hence we shouldn't worry about having
                    // multiple delegate instances with the same target
                    // being attached. This guarantee is made by the
                    // reference counting in HostLifecycleService.
                    //
                    // The use of a dictionary allows for reuse with
                    // multiple distinct handlers going forward.
                    //
                    map.Remove(handler);
                    rem.Invoke(_target, new object[] { h });
                }
            }
        }

        private static object GetHandler<TEventArgsThunk>(EventInfo evt, object call)
            where TEventArgsThunk : EventArgs
        {
            var ht = evt.EventHandlerType;
            var hp = ht.GetMethod("Invoke").GetParameters();

            var po = Expression.Parameter(hp[0].ParameterType, hp[0].Name);
            var pe = Expression.Parameter(hp[1].ParameterType, hp[1].Name);

            var h = Expression.Lambda(
                ht,
                Expression.Invoke(
                    Expression.Constant(call),
                    po,
                    Expression.New(
                        typeof(TEventArgsThunk).GetConstructor(new[] { typeof(object) }),
                        pe
                    )
                ),
                po,
                pe
            );

            return h.Compile();
        }
    }

    class ActivatedEventArgs : EventArgs
    {
        private static readonly Type         s_aeaType;
        private static readonly PropertyInfo s_aipProp;

        private        readonly object        _target;

        static ActivatedEventArgs()
        {
            s_aeaType = PhoneApplicationService.s_phshAsm.GetType("Microsoft.Phone.Shell.ActivatedEventArgs");
            s_aipProp = s_aeaType.GetProperty("IsApplicationInstancePreserved", BindingFlags.Public | BindingFlags.Instance);
        }

        public ActivatedEventArgs(object target)
        {
            _target = target;
        }

        public bool IsApplicationInstancePreserved
        {
            get
            {
                return (bool)s_aipProp.GetValue(_target, null);
            }
        }
    }

    class DeactivatedEventArgs : EventArgs
    {
        public DeactivatedEventArgs(object target)
        {
        }
    }
}
#endif
#endif