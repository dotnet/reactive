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
            svc.BeginMatchInDict("wn", "react", "prefix",
                iar =>
                {
                    var words = svc.EndMatchInDict(iar);
                    foreach (var word in words)
                        Console.WriteLine(word.Word);
                },
                null
            );

            Console.ReadLine();

        }
    }
}
