// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using ReactiveTests.Tests;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class CombineLatestBenchmark
    {
        private CombineLatestTest _zipTest = new CombineLatestTest();

        [Benchmark]
        public void CombineLatest_Typical2()
        {
            _zipTest.CombineLatest_Typical2();
        }

        [Benchmark]
        public void CombineLatest_Typical3()
        {
            _zipTest.CombineLatest_Typical3();
        }

        [Benchmark]
        public void CombineLatest_Typical4()
        {
            _zipTest.CombineLatest_Typical4();
        }

        [Benchmark]
        public void CombineLatest_Typical5()
        {
            _zipTest.CombineLatest_Typical5();
        }

        [Benchmark]
        public void CombineLatest_Typical6()
        {
            _zipTest.CombineLatest_Typical6();
        }

        [Benchmark]
        public void CombineLatest_Typical7()
        {
            _zipTest.CombineLatest_Typical7();
        }

        [Benchmark]
        public void CombineLatest_Typical8()
        {
            _zipTest.CombineLatest_Typical8();
        }

        [Benchmark]
        public void CombineLatest_Typical9()
        {
            _zipTest.CombineLatest_Typical9();
        }

        [Benchmark]
        public void CombineLatest_Typical10()
        {
            _zipTest.CombineLatest_Typical10();
        }

        [Benchmark]
        public void CombineLatest_Typical11()
        {
            _zipTest.CombineLatest_Typical11();
        }

        [Benchmark]
        public void CombineLatest_Typical12()
        {
            _zipTest.CombineLatest_Typical12();
        }
        
        [Benchmark]
        public void CombineLatest_Typical13()
        {
            _zipTest.CombineLatest_Typical13();
        }

        [Benchmark]
        public void CombineLatest_Typical14()
        {
            _zipTest.CombineLatest_Typical14();
        }

        [Benchmark]
        public void CombineLatest_Typical15()
        {
            _zipTest.CombineLatest_Typical15();
        }

        [Benchmark]
        public void CombineLatest_Typical16()
        {
            _zipTest.CombineLatest_Typical16();
        }
    }
}
