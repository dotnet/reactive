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
            
            var res = matchInDict("wn", "react", "prefix");
            var subscription = res.Subscribe(words =>
            {
                foreach (var word in words)
                    Console.WriteLine(word.Word);
            });

            Console.ReadLine();

        }
    }
}
