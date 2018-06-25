// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using ReactiveTests.Tests;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class ZipBenchmark
    {
        private ZipTest _zipTest = new ZipTest();

        [Benchmark]
        public void Zip_NAry_Asymmetric()
        {
            _zipTest.Zip_NAry_Asymmetric();
        }

        [Benchmark]
        public void Zip_NAry_Asymmetric_Selector()
        {
            _zipTest.Zip_NAry_Asymmetric_Selector();
        }

        [Benchmark]
        public void Zip_NAry_Symmetric()
        {
            _zipTest.Zip_NAry_Symmetric();
        }

        [Benchmark]
        public void Zip_NAry_Symmetric_Selector()
        {
            _zipTest.Zip_NAry_Symmetric_Selector();
        }

        [Benchmark]
        public void Zip_NAry_Enumerable_Simple()
        {
            _zipTest.Zip_NAry_Enumerable_Simple();
        }

        [Benchmark]
        public void Zip_AllCompleted2()
        {
            _zipTest.Zip_AllCompleted2();
        }

        [Benchmark]
        public void Zip_AllCompleted3()
        {
            _zipTest.Zip_AllCompleted3();
        }

        [Benchmark]
        public void Zip_AllCompleted4()
        {
            _zipTest.Zip_AllCompleted4();
        }

        [Benchmark]
        public void Zip_AllCompleted5()
        {
            _zipTest.Zip_AllCompleted5();
        }

        [Benchmark]
        public void Zip_AllCompleted6()
        {
            _zipTest.Zip_AllCompleted6();
        }

        [Benchmark]
        public void Zip_AllCompleted7()
        {
            _zipTest.Zip_AllCompleted7();
        }

        [Benchmark]
        public void Zip_AllCompleted8()
        {
            _zipTest.Zip_AllCompleted8();
        }

        [Benchmark]
        public void Zip_AllCompleted9()
        {
            _zipTest.Zip_AllCompleted9();
        }

        [Benchmark]
        public void Zip_AllCompleted10()
        {
            _zipTest.Zip_AllCompleted10();
        }

        [Benchmark]
        public void Zip_AllCompleted11()
        {
            _zipTest.Zip_AllCompleted11();
        }

        [Benchmark]
        public void Zip_AllCompleted12()
        {
            _zipTest.Zip_AllCompleted12();
        }

        [Benchmark]
        public void Zip_AllCompleted13()
        {
            _zipTest.Zip_AllCompleted13();
        }

        [Benchmark]
        public void Zip_AllCompleted14()
        {
            _zipTest.Zip_AllCompleted14();
        }

        [Benchmark]
        public void Zip_AllCompleted15()
        {
            _zipTest.Zip_AllCompleted15();
        }

        [Benchmark]
        public void Zip_AllCompleted16()
        {
            _zipTest.Zip_AllCompleted16();
        }
    }
}
