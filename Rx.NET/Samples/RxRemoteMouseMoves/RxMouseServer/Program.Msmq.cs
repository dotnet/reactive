using System;
using System.Collections.Generic;
using System.Drawing;
using System.Messaging;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxMouseServer
{
    partial class Program
    {
        static IObserver<Point> Msmq()
        {
            var q = "BARTDE-M6500\\Private$\\MouseService";
            var queue = default(MessageQueue);
            if (MessageQueue.Exists(q))
            {
                queue = new MessageQueue(q);
            }
            else
            {
                queue = MessageQueue.Create(q);
            }

            var format = new System.Messaging.BinaryMessageFormatter();
            queue.Formatter = format;

            var incoming = Observable.Create<string>(observer =>
            {
                return NewThreadScheduler.Default.ScheduleLongRunning(cancel =>
                {
                    while (!cancel.IsDisposed)
                    {
                        var msg = queue.Receive();
                        observer.OnNext((string)msg.Body);
                    }
                });
            });

            var sub = new ReplaySubject<Point>();

            var map = new Dictionary<string, IDisposable>();

            incoming.Subscribe(clientQueue =>
            {
                var command = clientQueue[0];
                var target = clientQueue.Substring(2);

                switch (command)
                {
                    case 'S':
                        {
                            var cq = new MessageQueue(target);

                            var crm = new System.Messaging.BinaryMessageFormatter();
                            cq.Formatter = crm;

                            map[target] = sub.Subscribe(pt =>
                            {
                                cq.Send(pt);
                            });
                        }
                        break;
                    case 'D':
                        {
                            var d = default(IDisposable);
                            if (map.TryGetValue(target, out d))
                                d.Dispose();
                        }
                        break;
                    default:
                        throw new Exception("Don't know what you're talking about!");
                }
            });

            return sub;
        }
    }
}
