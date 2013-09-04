// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !SILVERLIGHTM7

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void Queryable_Enumerable_Parity()
        {
            var enu = typeof(EnumerableEx).GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();
            var qry = typeof(QueryableEx).GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            var onlyInObs = enu.Select(m => m.Name).Except(qry.Select(m => m.Name)).Except(new[] { "ForEach", "ToEnumerable", "Multicast", "GetAwaiter", "ToEvent", "ToEventPattern", "ForEachAsync" }).ToList();
            var onlyInQbs = qry.Select(m => m.Name).Except(enu.Select(m => m.Name)).Except(new[] { "ToQueryable", "get_Provider", "Empty", "Range" }).ToList();

            Assert.IsTrue(onlyInObs.Count == 0, "Missing Queryable operator: " + string.Join(", ", onlyInObs.ToArray()));
            Assert.IsTrue(onlyInQbs.Count == 0, "Missing Enumerable operator: " + string.Join(", ", onlyInQbs.ToArray()));

            var enus = enu.GroupBy(m => m.Name);
            var qrys = qry.GroupBy(m => m.Name);
            var mtch = (from o in enus
                        join q in qrys on o.Key equals q.Key
                        select new { Name = o.Key, Enumerable = o.ToList(), Queryable = q.ToList() })
                       .ToList();

            Func<Type, bool> filterReturn = t =>
            {
                if (t.IsGenericType)
                {
                    var gd = t.GetGenericTypeDefinition();
                    if (gd == typeof(IBuffer<>))
                        return false;
                }
                return true;
            };

            Func<MethodInfo, bool> filterHelper = m =>
            {
                return !m.IsDefined(typeof(EditorBrowsableAttribute), false);
            };

            foreach (var group in mtch)
            {
                var oss = group.Enumerable.Where(m => filterReturn(m.ReturnType)).Select(m => GetSignature(m, false)).OrderBy(x => x).ToList();
                var qss = group.Queryable.Where(m => filterHelper(m)).Select(m => GetSignature(m, true)).OrderBy(x => x).ToList();

                Assert.IsTrue(oss.SequenceEqual(qss), "Mismatch between QueryableEx and EnumerableEx for " + group.Name);
            }
        }

        public static string GetSignature(MethodInfo m, bool correct)
        {
            var ps = m.GetParameters();
            var pss = ps.AsEnumerable();
            if (correct && ps.Length > 0 && ps[0].ParameterType == typeof(IQueryProvider))
                pss = pss.Skip(1);

            var gens = m.IsGenericMethod ? string.Format("<{0}>", string.Join(", ", m.GetGenericArguments().Select(a => GetTypeName(a, correct)).ToArray())) : "";

            var pars = string.Join(", ", pss.Select(p => (Attribute.IsDefined(p, typeof(ParamArrayAttribute)) ? "params " : "") + GetTypeName(p.ParameterType, correct) + " " + p.Name).ToArray());
            if (Attribute.IsDefined(m, typeof(ExtensionAttribute)))
            {
                if (pars.StartsWith("IQbservable") || pars.StartsWith("IQueryable"))
                    pars = "this " + pars;
            }

            return string.Format("{0} {1}{2}({3})", GetTypeName(m.ReturnType, correct), m.Name, gens, pars);
        }

        public static string GetTypeName(Type t, bool correct)
        {
            if (t.IsGenericType)
            {
                var gtd = t.GetGenericTypeDefinition();
                if (gtd == typeof(Expression<>))
                    return GetTypeName(t.GetGenericArguments()[0], false);

                var args = string.Join(", ", t.GetGenericArguments().Select(a => GetTypeName(a, false)).ToArray());

                var len = t.Name.IndexOf('`');
                var name = len >= 0 ? t.Name.Substring(0, len) : t.Name;
                if (correct && name == "IQbservable")
                    name = "IObservable";
                if (correct && name == "IQueryable")
                    name = "IEnumerable";

                return string.Format("{0}<{1}>", name, args);
            }

            if (t.IsArray)
            {
                return GetTypeName(t.GetElementType(), correct) + "[]";
            }

            return t.Name;
        }

        [TestMethod]
        public void QueryableRetarget1()
        {
            var res = QueryableEx.Provider.Empty<int>().AsEnumerable().ToList();
            Assert.IsTrue(res.SequenceEqual(new int[0]));
        }

        [TestMethod]
        public void QueryableRetarget2()
        {
            var res = QueryableEx.Provider.Return(42).AsEnumerable().ToList();
            Assert.IsTrue(res.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void QueryableRetarget3()
        {
            var res = Enumerable.Range(0, 10).AsQueryable().TakeLast(2).AsEnumerable().ToList();
            Assert.IsTrue(res.SequenceEqual(new[] { 8, 9 }));
        }

        [TestMethod]
        public void QueryableRetarget4()
        {
            var res = QueryableEx.Provider.Range(0, 10).AsEnumerable().ToList();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
        }
    }
}

#endif