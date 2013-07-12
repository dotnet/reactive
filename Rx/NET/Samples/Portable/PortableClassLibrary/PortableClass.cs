using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace PortableClassLibrary
{
    public class PortableClass
    {
        public IObservable<long> CreateTimer(int numSamples, TimeSpan timespan)
        {
            var fromTime = DateTimeOffset.Now.Add(timespan);
            return Observable.Timer(fromTime, timespan)
                    .Take(numSamples);
        }

        public IObservable<int> CreateList(IScheduler scheduler)
        {
            var values = new [] {1,2,5,7,8,324,4234,654654};

            return values.ToObservable(scheduler);
        }        
    }
}
