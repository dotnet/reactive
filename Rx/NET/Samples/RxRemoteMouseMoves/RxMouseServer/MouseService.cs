using System;
using System.Drawing;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RxMouseService;

namespace RxMouseServer
{
    public class MouseService : MarshalByRefObject, IMouseService, IObserver<Point>
    {
        private ReplaySubject<Point> _points;

        public MouseService()
        {
            _points = new ReplaySubject<Point>();
        }

        public IObservable<Point> GetPoints()
        {
            var src = _points.ObserveOn(NewThreadScheduler.Default);
            return Log(src).Remotable();
        }

        public IObservable<T> Log<T>(IObservable<T> source)
        {
            return Observable.Create<T>(observer =>
            {
                Console.WriteLine("Client connected!");

                var d = source.Subscribe(observer);

                return Disposable.Create(() =>
                {
                    Console.WriteLine("Client disconnected!");
                    d.Dispose();
                });
            });
        }

        public void OnNext(Point value)
        {
            _points.OnNext(value);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
