using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Disposables;
using Excercise7.DictionarySuggestService;

namespace Excercise7
{
    class Program
    {
        static void Main()
        {
            var svc = new DictServiceSoapClient("DictServiceSoap");
            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>
                (svc.BeginMatchInDict, svc.EndMatchInDict);

            Func<string, IObservable<DictionaryWord[]>> matchInWordNetByPrefix =
                term => matchInDict("wn", term, "prefix");

            var input = "reactive";
            for (int len = 3; len <= input.Length; len++)
            {
                var req = input.Substring(0, len);
                matchInWordNetByPrefix(req).Subscribe(
                    words => Console.WriteLine(req + " --> " + words.Length)
                );
            }

            Console.ReadLine();
        }
    }
}
