// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Xunit;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Tests
{
    public partial class Tests
    {
        [Fact]
        public void Queryable_Enumerable_Parity()
        {
            var enu = typeof(EnumerableEx).GetRuntimeMethods().Where(m => m.IsStatic && m.IsPublic).ToList();
            var qry = typeof(QueryableEx).GetRuntimeMethods().Where(m => m.IsStatic && m.IsPublic).ToList();

            var onlyInObs = enu.Select(m => m.Name).Except(qry.Select(m => m.Name)).Except(new[] { "ForEach", "ToEnumerable", "Multicast", "GetAwaiter", "ToEvent", "ToEventPattern", "ForEachAsync" }).ToList();
            var onlyInQbs = qry.Select(m => m.Name).Except(enu.Select(m => m.Name)).Except(new[] { "ToQueryable", "get_Provider", "Empty", "Range" }).ToList();

            Assert.True(onlyInObs.Count == 0, "Missing Queryable operator: " + string.Join(", ", onlyInObs.ToArray()));
            Assert.True(onlyInQbs.Count == 0, "Missing Enumerable operator: " + string.Join(", ", onlyInQbs.ToArray()));

            var enus = enu.GroupBy(m => m.Name);
            var qrys = qry.GroupBy(m => m.Name);
            var mtch = (from o in enus
                        join q in qrys on o.Key equals q.Key
                        select new { Name = o.Key, Enumerable = o.ToList(), Queryable = q.ToList() })
                       .ToList();

            Func<Type, bool> filterReturn = t =>
            {
                if (t.GetTypeInfo().IsGenericType)
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
                var oss = group.Enumerable
                    .Where(m => filterReturn(m.ReturnType))
                    .Select(m => GetSignature(m, false))
                    .OrderBy(x => x).ToList();

                var qss = group.Queryable
                    .Where(m => filterHelper(m))
                    .Select(m => GetSignature(m, true))
                    .OrderBy(x => x).ToList();

                if (!group.Name.Equals("Create"))
                    Assert.True(oss.SequenceEqual(qss), "Mismatch between QueryableEx and EnumerableEx for " + group.Name);
            }
        }

        public static string GetSignature(MethodInfo m, bool correct)
        {
            var ps = m.GetParameters();
            var pss = ps.AsEnumerable();
            if (correct && ps.Length > 0 && ps[0].ParameterType == typeof(IQueryProvider))
                pss = pss.Skip(1);

            var gens = m.IsGenericMethod ? string.Format("<{0}>", string.Join(", ", m.GetGenericArguments().Select(a => GetTypeName(a, correct)).ToArray())) : "";

            var pars = string.Join(", ", pss.Select(p => (p.IsDefined(typeof(ParamArrayAttribute)) ? "params " : "") + GetTypeName(p.ParameterType, correct) + " " + p.Name).ToArray());
            if (m.IsDefined(typeof(ExtensionAttribute)))
            {
                if (pars.StartsWith("IQbservable") || pars.StartsWith("IQueryable"))
                    pars = "this " + pars;
            }

            return string.Format("{0} {1}{2}({3})", GetTypeName(m.ReturnType, correct), m.Name, gens, pars);
        }

        public static string GetTypeName(Type t, bool correct)
        {
            if (t.GetTypeInfo().IsGenericType)
            {
                var gtd = t.GetGenericTypeDefinition();
                if (gtd == typeof(Expression<>))
                    return GetTypeName(t.GenericTypeArguments[0], false);

                var args = string.Join(", ", t.GenericTypeArguments.Select(a => GetTypeName(a, false)).ToArray());

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

        [Fact]
        public void QueryableRetarget1()
        {
            var res = QueryableEx.Provider.Empty<int>().AsEnumerable().ToList();
            Assert.True(res.SequenceEqual(new int[0]));
        }

        [Fact]
        public void QueryableRetarget2()
        {
            var res = QueryableEx.Provider.Return(42).AsEnumerable().ToList();
            Assert.True(res.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void QueryableRetarget3()
        {
            var res = Enumerable.Range(0, 10).AsQueryable().TakeLast(2).AsEnumerable().ToList();
            Assert.True(res.SequenceEqual(new[] { 8, 9 }));
        }

        [Fact]
        public void QueryableRetarget4()
        {
            var res = QueryableEx.Provider.Range(0, 10).AsEnumerable().ToList();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }
    }
}
