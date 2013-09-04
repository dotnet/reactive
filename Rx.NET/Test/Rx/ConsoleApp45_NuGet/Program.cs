using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PortableLibraryProfile7;

namespace ConsoleApp45_NuGet
{
    static class Program
    {
        static void Main(string[] args)
        {
            var tests = typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Where(m => m.IsDefined(typeof(TestAttribute), false));

            foreach (var t in tests)
            {
                Console.WriteLine(t.Name);

                var e = new ManualResetEvent(false);

                var res = false;
                var done = new Action<bool>(b =>
                {
                    res = b;
                    e.Set();
                });

                t.Invoke(null, new[] { done });

                e.WaitOne();

                Console.WriteLine(res ? "Succeeded!" : "Failed!");
                Console.WriteLine();
            }
        }

        [Test]
        static void Clock(Action<bool> done)
        {
            var clock = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => DateTime.Now);

            var res = clock.Take(5);

            res.Subscribe(now => { Console.WriteLine(now); }, () => done(true));
        }

        [Test]
        static void Portable(Action<bool> done)
        {
            var clock = MyExtensions.GetClock();

            var res = clock.Take(5);

            res.Subscribe(now => { Console.WriteLine(now); }, () => done(true));
        }

        [Test]
        static void Providers(Action<bool> done)
        {
            var res = Qbservable.Range(Qbservable.Provider, 0, 10, Scheduler.Default).Zip(Observable.Range(0, 10, Scheduler.Default).AsQbservable().Where(_ => true).AsObservable(), (x, y) => x - y).All(d => d == 0);

            res.Subscribe(done);
        }

        [Test]
        static void Remoting(Action<bool> done)
        {
            var d = AppDomain.CreateDomain("RemotingTest");

            var xs = Observable.Range(0, 10, Scheduler.Default).Remotable();
            var dn = new Done(done);

            d.SetData("xs", xs);
            d.SetData("dn", dn);

            d.DoCallBack(() =>
            {
                var ys = (IObservable<int>)AppDomain.CurrentDomain.GetData("xs");

                var res = ys.ToArray().Wait();

                var b = res.SequenceEqual(Enumerable.Range(0, 10));

                ((Done)AppDomain.CurrentDomain.GetData("dn")).Set(b);
            });
        }

        class Done : MarshalByRefObject
        {
            private readonly Action<bool> _done;

            public Done(Action<bool> done)
            {
                _done = done;
            }

            public void Set(bool result)
            {
                _done(result);
            }

            public override object InitializeLifetimeService()
            {
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    class TestAttribute : Attribute
    {
    }
}
