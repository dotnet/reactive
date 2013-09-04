using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;
using Excercise9.DictionarySuggestService;

namespace Excercise9
{
    class Program
    {
        static void Main()
        {
            var txt = new TextBox();
            var lst = new ListBox { Top = txt.Height + 10 };

            var frm = new Form()
            {
                Controls = { txt, lst }
            };

#if PRODUCTION
            // Turn the user input into a tamed sequence of strings.
            var textChanged = from evt in Observable.FromEventPattern(txt, "TextChanged")
                              select ((TextBox)evt.Sender).Text;
                       
            var input = textChanged
                        .Throttle(TimeSpan.FromSeconds(1))
                        .DistinctUntilChanged();
#else
            const string INPUT = "reactive";

            var rand = new Random();

            var input = Observable.Generate(
                3,
                len => len <= INPUT.Length,
                len => len + 1,
                len => INPUT.Substring(0, len),
                _ => TimeSpan.FromMilliseconds(rand.Next(200, 1200))
            )
            .ObserveOn(txt)
            .Do(term => txt.Text = term)
            .Throttle(TimeSpan.FromSeconds(1));
#endif

            // Bridge with the web service's MatchInDict method.
            var svc = new DictServiceSoapClient("DictServiceSoap");
            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>
                (svc.BeginMatchInDict, svc.EndMatchInDict);

#if PRODUCTION
            Func<string, IObservable<DictionaryWord[]>> matchInWordNetByPrefix =
                term => matchInDict("wn", term, "prefix");
#else
            Func<string, IObservable<DictionaryWord[]>> matchInWordNetByPrefix =
                term => Observable.Return
                (
                    (from i in Enumerable.Range(0, rand.Next(0, 50))
                     select new DictionaryWord { Word = term + i })
                     .ToArray()
                ).Delay(TimeSpan.FromSeconds(rand.Next(1, 10)));
#endif

            // The grand composition connecting the user input with the web service.
            var res = (from term in input
                      select matchInWordNetByPrefix(term))
                      .Switch();

            // Synchronize with the UI thread and populate the ListBox or signal an error.
            using (res.ObserveOn(lst).Subscribe(
                words =>
                {
                    lst.Items.Clear();
                    lst.Items.AddRange((from word in words select word.Word).ToArray());
                },
                ex =>
                {
                    MessageBox.Show(
                        "An error occurred: " + ex.Message, frm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                }))
            {
                Application.Run(frm);
            } // Proper disposal happens upon exiting the application.
        }
    }
}
