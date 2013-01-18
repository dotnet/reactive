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

            var input = (from evt in Observable.FromEventPattern(txt, "TextChanged")
                         select ((TextBox)evt.Sender).Text)
                         .Throttle(TimeSpan.FromSeconds(1))
                         .DistinctUntilChanged()
                         .Do(x => Console.WriteLine(x));

            var svc = new DictServiceSoapClient("DictServiceSoap");
            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>
                (svc.BeginMatchInDict, svc.EndMatchInDict);

            Func<string, IObservable<DictionaryWord[]>> matchInWordNetByPrefix =
                term => matchInDict("wn", term, "prefix");

            var res = from term in input
                      from words in matchInWordNetByPrefix(term)
                      select words;

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
            }
        }
    }
}
