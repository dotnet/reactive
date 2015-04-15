// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            var e = new EventLoopScheduler();
            e.Schedule(new TimeSpan((long)int.MaxValue * 2), () => { });
            Console.ReadLine();
            //Observable.Defer<string>(() => {
            //    throw new InvalidOperationException("Foo");
            //    return Observable.Return("Success");
            //})
            //.Catch(Observable.Return("Failed the first time"))
            //.Catch<string, Exception>(ex => Observable.Return("Failed the second time"))
            //.Do(Console.WriteLine)
            //.Wait();

            //try
            //{
            //    Observable.Create<bool>(async (observer, cancel) =>
            //    {
            //        throw new Exception("Before or after await - it doesn't matter when this is thrown.");

            //        return Disposable.Empty;
            //    })
            //    .Subscribe(
            //        _ => { },
            //        ex => { throw new Exception(); }); // The default OnError behavior.

            //    Console.WriteLine("Shouldn't get here."); // This code executes every time.
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("Should get here.");  // This code never executes.
            //}

        }
    }
}