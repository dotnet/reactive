/*
 * Example showing the use of Rx for monitoring correlated activity event tracing streams.
 * 
 * bartde - 10/10/2012
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace EventCorrelationSample
{
    class Program
    {
        //
        // Those fields represent the ETW infrastructure.
        //
        private static Subject<BeginActivity> _beginActivities = new Subject<BeginActivity>();
        private static Subject<EndActivity> _endActivities = new Subject<EndActivity>();
        private static Subject<BeginTaskA> _beginAs = new Subject<BeginTaskA>();
        private static Subject<EndTaskA> _endAs = new Subject<EndTaskA>();
        private static Subject<BeginTaskB> _beginBs = new Subject<BeginTaskB>();
        private static Subject<EndTaskB> _endBs = new Subject<EndTaskB>();

        static void Main(string[] args)
        {
            //
            // Timestamps would really be inserted by ETW, but we'll add them ourselves using Rx's Timestamp operator.
            //
            var beginActivities = _beginActivities.Timestamp();
            var endActivities = _endActivities.Timestamp();
            var beginAs = _beginAs.Timestamp();
            var endAs = _endAs.Timestamp();
            var beginBs = _beginBs.Timestamp();
            var endBs = _endBs.Timestamp();

            //
            // Analyze length of requests.
            //
            var requests = from b in beginActivities
                           from e in endActivities.Where(e => e.Value.Id == b.Value.Id)
                           select new { Id = b.Value.Id, Length = e.Timestamp - b.Timestamp };

            requests.Subscribe(Print(ConsoleColor.Yellow));

            //
            // Correlate task information.
            //
            var info = from beginAct in beginActivities
                       let activityId = beginAct.Value.Id /* improve readability */
                       //
                       // Obtain correlated streams. Subscription doesn't happen here yet.
                       //
                       let endAct = endActivities.FirstAsync(e => e.Value.Id == activityId) /* correlated end event */
                       let taskAs = /* correlated task A events; will contain payload and start/end times */
                            from beginA in beginAs.TakeUntil(endAct).Where(a => a.Value.ActivityId == activityId)
                            from endA in endAs.FirstAsync(e => e.Value.Id == beginA.Value.Id) /* correlation between the task's begin/end events */
                            select new { Value = (int?)beginA.Value.Value, Start = beginA.Timestamp, End = endA.Timestamp }
                       let taskBs = /* correlated task B events; will contain payload and start/end times */
                            from beginB in beginBs.TakeUntil(endAct).Where(b => b.Value.ActivityId == activityId)
                            from endB in endBs.FirstAsync(e => e.Value.Id == beginB.Value.Id) /* correlation between the task's begin/end events */
                            select new { Value = beginB.Value.Value, Start = beginB.Timestamp, End = endB.Timestamp }
                       //
                       // Parallel observation of all of the above, using CombineLatest with seed values for events that can be absent.
                       // The SelectMany operator, bound to by from...from... syntax causes the flattening of beginActivities and the result of parallel observation.
                       //
                       from res in Observable.CombineLatest(
                            endAct,
                            taskAs.StartWith(new { Value = default(int?), Start = DateTimeOffset.MinValue, End = DateTimeOffset.MinValue }),
                            taskBs.StartWith(new { Value = default(string), Start = DateTimeOffset.MinValue, End = DateTimeOffset.MinValue }),
                            (e, a, b) => new { e, a, b }).LastAsync() /* the stream will end because all substreams end (due to FirstAsync and TakeUntil use); only forward the last combined result */
                       select new { 
                           Activity = activityId, StartTime = beginAct.Timestamp, EndTime = res.e.Timestamp,
                           PayloadA = res.a.Value != null ? res.a.Value.ToString() : "(none)", DurationA = res.a.End - res.a.Start,
                           PayloadB = res.b.Value ?? "(none)", DurationB = res.b.End - res.b.Start
                       };

            info.Subscribe(Print(ConsoleColor.Cyan));

            StartService();
        }

        static void StartService()
        {
            //
            // Mimics talking to ETW.
            //
            var beginActivitiesObserver = Observer.Synchronize(_beginActivities);
            var endActivitiesObserver = Observer.Synchronize(_endActivities);
            var beginAsObserver = Observer.Synchronize(_beginAs);
            var endAsObserver = Observer.Synchronize(_endAs);
            var beginBsObserver = Observer.Synchronize(_beginBs);
            var endBsObserver = Observer.Synchronize(_endBs);

            //
            // Master random number generator + throttle to max 10 requests at the same time.
            //
            var rnd = new Random();
            var semaphore = new Semaphore(10, 10);

            while (true)
            {
                semaphore.WaitOne();

                var seed = rnd.Next();
                Task.Run(async () =>
                {
                    var rand = new Random(seed);

                    var requestId = Guid.NewGuid();

                    Print(ConsoleColor.Green)("Starting request " + requestId);

                    var measure = Stopwatch.StartNew();
                    beginActivitiesObserver.OnNext(new BeginActivity { Id = requestId });

                    Thread.Sleep(rand.Next(50, 300));

                    var tossA = rand.Next(2);
                    var tossB = rand.Next(2);

                    var tasks = new List<Task>(tossA + tossB);
                    var diag = new List<string>();

                    if (tossA == 1)
                    {
                        var aDelay = rand.Next(20, 70);
                        var aLength = rand.Next(100, 2000);
                        var aValue = rand.Next(0, 43);

                        diag.Add(string.Format("A(d = {0}, t = {1}, x = {2})", aDelay, aLength, aValue));

                        tasks.Add(Task.Run(() =>
                        {
                            var taskId = Guid.NewGuid();

                            Thread.Sleep(aDelay);
                            beginAsObserver.OnNext(new BeginTaskA { Id = taskId, ActivityId = requestId, Value = aValue });
                            Thread.Sleep(aLength);
                            endAsObserver.OnNext(new EndTaskA { Id = taskId });
                        }));
                    }

                    if (tossB == 1)
                    {
                        var bDelay = rand.Next(10, 40);
                        var bLength = rand.Next(100, 2000);
                        var alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                        var bValue = new string(Enumerable.Range(0, rand.Next(0, 10)).Select(x => alphabet[rand.Next(alphabet.Length)]).ToArray());

                        diag.Add(string.Format("B(d = {0}, t = {1}, x = {2})", bDelay, bLength, bValue));

                        tasks.Add(Task.Run(() =>
                        {
                            var taskId = Guid.NewGuid();

                            Thread.Sleep(bDelay);
                            beginBsObserver.OnNext(new BeginTaskB { Id = taskId, ActivityId = requestId, Value = bValue });
                            Thread.Sleep(bLength);
                            endBsObserver.OnNext(new EndTaskB { Id = taskId });
                        }));
                    }

                    await Task.WhenAll(tasks);

                    Thread.Sleep(rand.Next(50, 100));

                    measure.Stop();
                    Print(ConsoleColor.Red)(string.Format("Ending request {0} (d = {1}) {2}", requestId, measure.Elapsed, string.Join(" ", diag)));
                    endActivitiesObserver.OnNext(new EndActivity { Id = requestId });

                    semaphore.Release();
                });
            }
        }

        static object s_gate = new object();

        static void WriteLine(ConsoleColor color, object message)
        {
            lock (s_gate)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        static Action<object> Print(ConsoleColor color)
        {
            return s => WriteLine(color, s);
        }
    }

    class Event
    {
        public Guid Id { get; set; }
    }

    class BeginActivity : Event
    {
    }

    class EndActivity : Event
    {
    }

    class BeginTaskA : Event
    {
        public Guid ActivityId { get; set; }
        public int Value { get; set; }
    }

    class EndTaskA : Event
    {
    }

    class BeginTaskB : Event
    {
        public Guid ActivityId { get; set; }
        public string Value { get; set; }
    }

    class EndTaskB : Event
    {
        public string Value { get; set; }
    }
}
