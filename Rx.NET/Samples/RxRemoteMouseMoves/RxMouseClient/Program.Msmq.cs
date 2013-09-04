using System;
using System.Drawing;
using System.Messaging;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RxMouseClient
{
    partial class Program
    {
        static IObservable<Point> Msmq(string srv)
        {
            return FromQueue<Point>(srv + "\\Private$\\MouseService");
        }

        static IObservable<T> FromQueue<T>(string serverQueue)
        {
            return Observable.Create<T>(observer =>
            {
                var responseQueue = Environment.MachineName + "\\Private$\\" + Guid.NewGuid().ToString();
                var queue = MessageQueue.Create(responseQueue);

                var frm = new System.Messaging.BinaryMessageFormatter();
                var srv = new MessageQueue(serverQueue);
                srv.Formatter = frm;
                queue.Formatter = frm;

                srv.Send("S " + responseQueue);

                var loop = NewThreadScheduler.Default.ScheduleLongRunning(cancel =>
                {
                    while (!cancel.IsDisposed)
                    {
                        var msg = queue.Receive();
                        observer.OnNext((T)msg.Body);
                    }
                });

                return new CompositeDisposable(
                    loop,
                    Disposable.Create(() =>
                    {
                        srv.Send("D " + responseQueue);
                        MessageQueue.Delete(responseQueue);
                    })
                );
            });
        }
    }
}
