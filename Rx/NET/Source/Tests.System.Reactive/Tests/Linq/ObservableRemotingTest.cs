// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_REMOTING

using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableRemotingTest : ReactiveTest
    {
        [TestMethod]
        public void Remotable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => RemotingObservable.Remotable(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => RemotingObservable.Remotable(default(IObservable<int>), new MyLease()));

            ReactiveAssert.Throws<ArgumentNullException>(() => RemotingObservable.Remotable(default(IQbservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => RemotingObservable.Remotable(default(IQbservable<int>), new MyLease()));

            RemotingObservable.Remotable(Observable.Return(42));
            RemotingObservable.Remotable(Observable.Return(42), null /* valid lease object */);

            RemotingObservable.Remotable(Qbservable.Return(Qbservable.Provider, 42));
            RemotingObservable.Remotable(Qbservable.Return(Qbservable.Provider, 42), null /* valid lease object */);
        }

        class MyLease : ILease
        {
            public TimeSpan CurrentLeaseTime
            {
                get { throw new NotImplementedException(); }
            }

            public LeaseState CurrentState
            {
                get { throw new NotImplementedException(); }
            }

            public TimeSpan InitialLeaseTime
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Register(ISponsor obj)
            {
                throw new NotImplementedException();
            }

            public void Register(ISponsor obj, TimeSpan renewalTime)
            {
                throw new NotImplementedException();
            }

            public TimeSpan Renew(TimeSpan renewalTime)
            {
                throw new NotImplementedException();
            }

            public TimeSpan RenewOnCallTime
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public TimeSpan SponsorshipTimeout
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Unregister(ISponsor obj)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Remotable_Empty()
        {
            var evt = new ManualResetEvent(false);

            var e = GetRemoteObservable(t => t.Empty());
            using (e.Subscribe(_ => { Assert.Fail(); }, _ => { Assert.Fail(); }, () => { evt.Set(); }))
            {
                evt.WaitOne();
            }
        }

        [TestMethod]
        public void Remotable_Return()
        {
            var evt = new ManualResetEvent(false);

            bool next = false;
            var e = GetRemoteObservable(t => t.Return(42));
            using (e.Subscribe(value => { next = true; Assert.AreEqual(42, value); }, _ => { Assert.Fail(); }, () => { evt.Set(); }))
            {
                evt.WaitOne();
                Assert.IsTrue(next);
            }
        }

        [TestMethod]
        public void Remotable_Return_LongLease()
        {
            var evt = new ManualResetEvent(false);

            bool next = false;
            var e = GetRemoteObservable(t => t.ReturnLongLease(42));
            using (e.Subscribe(value => { next = true; Assert.AreEqual(42, value); }, _ => { Assert.Fail(); }, () => { evt.Set(); }))
            {
                evt.WaitOne();
                Assert.IsTrue(next);
            }
        }

        [TestMethod]
        public void Remotable_Throw()
        {
            var ex = new InvalidOperationException("Oops!");

            var evt = new ManualResetEvent(false);

            bool error = false;
            var e = GetRemoteObservable(t => t.Throw(ex));
            using (e.Subscribe(value => { Assert.Fail(); }, err => { error = true; Assert.IsTrue(err is InvalidOperationException && err.Message == ex.Message); evt.Set(); }, () => { Assert.Fail(); }))
            {
                evt.WaitOne();
                Assert.IsTrue(error);
            }
        }

        [TestMethod]
        public void Remotable_Disposal()
        {
            var test = GetRemoteTestObject();
            test.Disposal().Subscribe().Dispose();
            Assert.IsTrue(test.Disposed);
        }

        private IObservable<int> GetRemoteObservable(Func<RemotingTest, IObservable<int>> f)
        {
            var test = GetRemoteTestObject();
            return f(test);
        }

        private RemotingTest GetRemoteTestObject()
        {
            var ads = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
            var ad = AppDomain.CreateDomain("test", null, ads);
            var test = (RemotingTest)ad.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "ReactiveTests.Tests.RemotingTest");
            return test;
        }
    }

    public class RemotingTest : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public IObservable<int> Empty()
        {
            return Observable.Empty<int>().Remotable();
        }

        public IObservable<int> Return(int value)
        {
            return Observable.Return<int>(value).Remotable();
        }

        public IObservable<int> ReturnLongLease(int value)
        {
            return Observable.Return<int>(value).Remotable(null);
        }

        public IObservable<int> Throw(Exception ex)
        {
            return Observable.Throw<int>(ex).Remotable();
        }

        public IObservable<int> Disposal()
        {
            return Observable.Create<int>(obs =>
            {
                return () => { Disposed = true; };
            }).Remotable();
        }

        public bool Disposed { get; set; }
    }
}
#endif