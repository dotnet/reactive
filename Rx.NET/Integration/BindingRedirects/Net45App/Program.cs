using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCodeInPcl;

namespace Net45App
{
    class Program
    {
        static void Main(string[] args)
        {
            var disp = Class1.GetFoo().Subscribe(null);
        }
    }
}
