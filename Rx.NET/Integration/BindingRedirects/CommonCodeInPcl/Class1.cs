using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace CommonCodeInPcl
{
    public class Class1
    {
        ISubject<Class1> subject;
        IDisposable disposable = Disposable.Empty;
        public static IObservable<int> GetFoo()
        {

            return Observable.Interval(TimeSpan.FromMinutes(1)).Select(_ => 3);

        }
    }
}
