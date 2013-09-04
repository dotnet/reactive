using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HistoricalScheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LogScheduler<int>(GetLog());

            log.Process((xs, s) =>
            {
                var res0 = xs.Timestamp(s);
                res0.Subscribe(t => Console.WriteLine("0> " + t));

                var res1 = xs.Where(x => x % 2 != 0).Timestamp(s);
                res1.Subscribe(t => Console.WriteLine("1> " + t));

                var res2 = xs.Buffer(TimeSpan.FromDays(63), s).Select(b => b.Count).Timestamp(s);
                res2.Subscribe(t => Console.WriteLine("2> " + t));

                var res3 = Observable.Interval(TimeSpan.FromDays(1), s).TakeUntil(new DateTimeOffset(2013, 1, 1, 12, 0, 0, TimeSpan.Zero), s).Select(_ => s.Clock);
                res3.Subscribe(t =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("It's now " + t);
                    Console.ResetColor();
                });

                //
                // If the end of the log should cause the scheduler to stop, add the following line:
                //
                // xs.Subscribe(_ => { }, s.Stop);
            });
        }

        static IEnumerable<Timestamped<int>> GetLog()
        {
            for (int i = 1; i <= 12; i++)
            {
                var date = new DateTimeOffset(2012, i, 1, 12, 0, 0, TimeSpan.Zero);
                var value = i * i;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Log read for {0} - Value = {1}", date, value);
                Console.ResetColor();

                yield return new Timestamped<int>(i * i, date);
            }
        }
    }

    class LogScheduler<T>
    {
        private readonly IEnumerable<Timestamped<T>> _source;

        public LogScheduler(IEnumerable<Timestamped<T>> source)
        {
            _source = source;
        }

        public void Process(Action<IObservable<T>, HistoricalScheduler> query)
        {
            var enumerator = _source.GetEnumerator();

            var scheduler = new Scheduler(enumerator);

            query(scheduler.Source, scheduler);

            scheduler.Start();
        }

        class Scheduler : HistoricalScheduler
        {
            private readonly Subject<T> _subject = new Subject<T>();
            private readonly IEnumerator<Timestamped<T>> _enumerator;

            public Scheduler(IEnumerator<Timestamped<T>> enumerator)
            {
                _enumerator = enumerator;

                MoveNext(true);
            }

            public Scheduler(IEnumerator<Timestamped<T>> enumerator, DateTimeOffset startTime)
            {
                _enumerator = enumerator;

                this.AdvanceTo(startTime);
                MoveNext();
            }

            public void MoveNext(bool initializeInitialTimeFromLog = false)
            {
                var nextLog = default(Timestamped<T>);
                if (TryMoveNext(out nextLog))
                {
                    if (initializeInitialTimeFromLog)
                        this.AdvanceTo(nextLog.Timestamp);

                    ScheduleOnNext(nextLog);
                }
                else
                {
                    this.Schedule(_subject.OnCompleted);
                }
            }

            public IObservable<T> Source
            {
                get { return _subject.AsObservable(); }
            }

            private bool TryMoveNext(out Timestamped<T> value)
            {
                try
                {
                    if (_enumerator.MoveNext())
                    {
                        value = _enumerator.Current;
                        return true;
                    }
                }
                catch
                {
                    _enumerator.Dispose();
                    throw;
                }

                _enumerator.Dispose();

                value = default(Timestamped<T>);
                return false;
            }

            private void ScheduleOnNext(Timestamped<T> value)
            {
                this.Schedule(value.Timestamp, () =>
                {
                    _subject.OnNext(value.Value);
                    MoveNext();
                });
            }
        }
    }
}
