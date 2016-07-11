using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace CommonCodeInPcl
{
    public class Class1
    {
        public static IObservable<int> GetFoo()
        {
            return Observable.Interval(TimeSpan.FromMinutes(1)).Select(_ => 3);
        }
    }
}
