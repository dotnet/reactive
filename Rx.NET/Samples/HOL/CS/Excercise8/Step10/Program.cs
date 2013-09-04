using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;
using Excercise8.DictionarySuggestService;

namespace Excercise8
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

            // Turn the user input into a tamed sequence of strings.
            var textChanged = from evt in Observable.FromEventPattern(txt, "TextChanged")
                              select ((TextBox)evt.Sender).Text;
                       
            var input = textChanged
                        .Throttle(TimeSpan.FromSeconds(1))
                        .DistinctUntilChanged();

            // Bridge with the web service's MatchInDict method.
            var svc = new DictServiceSoapClient("DictServiceSoap");
            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>
                (svc.BeginMatchInDict, svc.EndMatchInDict);

            Func<string, IObservable<DictionaryWord[]>> matchInWordNetByPrefix =
                term => matchInDict("wn", term, "prefix");

            // The grand composition connecting the user input with the web service.
            var res = from term in input
                      from word in matchInWordNetByPrefix(term)
                                    // .Finally(() => Console.WriteLine("Disposed request for " + term))
                                    .TakeUntil(input)
                      select word;

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
