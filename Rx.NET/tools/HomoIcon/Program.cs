using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomoIconize
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Auto-homoiconizer for Qbservable[Ex]");
            Console.WriteLine("------------------------------------");
            Console.WriteLine();

            var uri = new Uri(Assembly.GetEntryAssembly().CodeBase);

            var root = Path.Combine(Path.GetDirectoryName(uri.LocalPath), @"..\..\..\..\Source");
            if (!Directory.Exists(root))
            {
                Console.WriteLine("Error:  Could not find directory \"" + root + "\"");
                return;
            }

            Process(root, "System.Reactive.Linq", "System.Reactive.Providers", @"Reactive\Linq\Qbservable.Generated.cs", "System.Reactive.Linq.Observable", "Qbservable", true);
            Console.WriteLine();

            Process(root, "System.Reactive.Experimental", "System.Reactive.Experimental", @"Reactive\Linq\QbservableEx.Generated.cs", "System.Reactive.Linq.ObservableEx", "QbservableEx");
            Console.WriteLine();
        }

        static void Process(string root, string sourceAssembly, string targetAssembly, string targetFile, string sourceTypeName, string targetTypeName, bool includeAsync = false)
        {

            var rxRoot = Path.Combine(root, sourceAssembly);
            if (!Directory.Exists(rxRoot))
            {
                Console.WriteLine("Error:  Could not find directory \"" + rxRoot + "\"");
                return;
            }

            var qbRoot = Path.Combine(root, targetAssembly);
            if (!Directory.Exists(qbRoot))
            {
                Console.WriteLine("Error:  Could not find directory \"" + qbRoot + "\"");
                return;
            }

            var dll = Path.Combine(rxRoot, @"bin\debug40\" + sourceAssembly + ".dll");
            if (!File.Exists(dll))
            {
                Console.WriteLine("Error:  Could not find file \"" + dll + "\"");
                return;
            }

            var xml = Path.Combine(rxRoot, @"bin\debug40\" + sourceAssembly + ".xml");
            if (!File.Exists(xml))
            {
                Console.WriteLine("Error:  Could not find file \"" + xml + "\"");
                return;
            }

            var qbsgen = Path.Combine(qbRoot, targetFile);
            if (!File.Exists(qbsgen))
            {
                Console.WriteLine("Error:  Could not find file \"" + qbsgen + "\"");
                return;
            }

            Generate(dll, xml, qbsgen, sourceTypeName, targetTypeName, includeAsync);
        }

        // Prototype interface to break dependencies. Only used for ToString2 ultimately.
        interface IQbservable<T>
        {
        }

        // Prototype interface to break dependencies. Only used for ToString2 ultimately.
        interface IOrderedQbservable<T>
        {
        }

        static void Generate(string input, string xml, string output, string sourceTypeName, string targetTypeName, bool includeAsync)
        {
            var docs = XDocument.Load(xml).Root.Element("members").Elements("member").ToDictionary(m => m.Attribute("name").Value, m => m);

            Console.WriteLine("Loading {0}...", input);

            var asm = Assembly.LoadFrom(input);
            var t = asm.GetType(sourceTypeName);
            _qbs = typeof(IQbservable<>); //asm.GetType("System.Reactive.Linq.IQbservable`1");
            _oqbs = typeof(IOrderedQbservable<>);

            Console.WriteLine("Checking {0}...", output);
            var attr = File.GetAttributes(output);
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                Console.Write("Attempting to check out generated files... ");

                try
                {
                    System.Diagnostics.Process.Start("tf.exe", "edit \"" + output + "\"").WaitForExit();
                }
                catch { /* no comment */ }

                attr = File.GetAttributes(output);

                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    Console.WriteLine("Failed.");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Making file writable. DON'T FORGET TO INCLUDE IN CHECK-IN!");
                    Console.ResetColor();

                    File.SetAttributes(output, attr & ~FileAttributes.ReadOnly);
                }
                else
                {
                    Console.WriteLine("Succeeded.");
                }
            }

            Console.WriteLine("Deleting {0}...", output);
            File.Delete(output);

            Console.WriteLine("Creating {0}...", output);
            using (var fs = File.OpenWrite(output))
            {
                using (Out = new StreamWriter(fs))
                {
                    Generate(t, docs, targetTypeName, includeAsync);
                }
            }
        }

        private const string OrderedObservableFullName = "System.Reactive.Linq.IOrderedObservable`1";
        static Type _qbs, _oqbs;

        static void Generate(Type t, IDictionary<string, XElement> docs, string typeName, bool includeAsync)
        {
            WriteLine(
@"/*
 * WARNING: Auto-generated file (" + DateTime.Now + @")
 * Run Rx's auto-homoiconizer tool to generate this file (in the HomoIcon directory).
 */
");
            WriteLine(
@"#pragma warning disable 1591
");
            WriteLine(
@"#if !NO_EXPRESSIONS
");

            WriteLine(
@"using System;
using System.Reactive.Concurrency;
using System.Collections.Generic;
using System.Reactive.Joins;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Reactive;
using System.Reactive.Subjects;
#if !NO_TPL
using System.Threading.Tasks;
#endif
#if !NO_REMOTING
using System.Runtime.Remoting.Lifetime;
#endif
");
            WriteLine(
@"namespace System.Reactive.Linq
{");

            Indent();

            WriteLine(
@"public static partial class " + typeName + @"
{");

            Indent();

            var except = new[] { "ToAsync", "FromAsyncPattern", "And", "Then", "GetEnumerator", "get_Provider", "Wait", "ForEach", "ForEachAsync", "GetAwaiter", "First", "FirstOrDefault", "Last", "LastOrDefault", "Single", "SingleOrDefault", "Subscribe", "AsQbservable", "AsObservable", "ToEvent", "ToEventPattern" };

            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Static).OrderBy(m => m.Name).ThenBy(m => !m.IsGenericMethod ? "" : string.Join(",", m.GetGenericArguments().Select(p => p.Name))).ThenBy(m => string.Join(",", m.GetParameters().Select(p => p.Name + ":" + p.ParameterType.FullName))).Where(m => !except.Contains(m.Name)))
            {
                var docName = ToDocName(m);
                var xmlDoc = default(XElement);
                if (!docs.TryGetValue(docName, out xmlDoc))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Missing XML documentation for {0}", docName);
                    Console.ResetColor();
                }

                var p = m.GetParameters();
                if (m.Name == "When" && p.Length == 1 && p.Single().ParameterType.ToString().Contains("Plan"))
                    continue;

                var funky = from pi in p
                            let pt = pi.ParameterType
                            where pt.IsGenericType
                            let ptgtd = pt.GetGenericTypeDefinition()
                            where ptgtd.Name.StartsWith("Func")
                            where ptgtd.GetGenericArguments().Count() > 5
                            select pi;

                var isLargeArity = funky.Any();

                var hasTask = p.Any(pa => ContainsTask(pa.ParameterType));

                var ret = m.ReturnType;
                if (ret.IsGenericType)
                {
                    var d = ret.GetGenericTypeDefinition();
                    if (d.Name.StartsWith("IConnectableObservable") || d.Name.StartsWith("ListObservable"))
                        continue;
                    if (d != typeof(IObservable<>) && d != typeof(IEnumerable<>) && d.FullName != OrderedObservableFullName)
                        throw new InvalidOperationException("Invalid return type for " + m.Name);
                }
                else
                    throw new InvalidOperationException("Invalid return type for " + m.Name);

                ret = ret.Iconize();

                var hasProvider = true;
                if (p.Length > 0)
                {
                    var f = p.First();
                    if (f.ParameterType.IsGenericType)
                    {
                        var d = f.ParameterType.GetGenericTypeDefinition();
                        if (d == typeof(IObservable<>) || d.FullName == OrderedObservableFullName) // Check - e.g. Amb    || d == typeof(IEnumerable<>))
                            hasProvider = false;
                    }
                }

                var nulls = new List<string>();
                var pars = new List<string>();
                var ptps = new List<string>();
                var args = new List<string>();

                var firstArg = hasProvider ? "IQbservableProvider" : p.First().ParameterType.Iconize().ToString2();
                var firstName = hasProvider ? "provider" : p.First().Name;
                pars.Add("this " + firstArg + " " + firstName);
                ptps.Add(firstArg);
                if (!hasProvider)
                    args.Add(firstName + ".Expression");
                else
                    args.Add("Expression.Constant(provider, typeof(IQbservableProvider))");
                nulls.Add(firstName);

                var rem = hasProvider ? p : p.Skip(1);
                var isCreateAsync = false;
                foreach (var q in rem)
                {
                    var pt = q.ParameterType;
                    if (pt.Name.StartsWith("Func") || pt.Name.StartsWith("Action"))
                    {
                        if (pt.Name.StartsWith("Func") && pt.GetGenericArguments().Last().Name.StartsWith("Task"))
                        {
                            isCreateAsync = true;
                        }
                        pt = typeof(Expression<>).MakeGenericType(pt);
                        args.Add(q.Name);
                    }
                    else
                    {
                        var isObs = new Func<Type, bool>(tt => tt.IsGenericType && (tt.GetGenericTypeDefinition() == typeof(IObservable<>) || tt.FullName == OrderedObservableFullName));
                        var isEnm = new Func<Type, bool>(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                        if (isObs(pt) || pt.IsArray && isObs(pt.GetElementType()) || isEnm(pt) || pt.IsArray && isEnm(pt.GetElementType()))
                            args.Add("GetSourceExpression(" + q.Name + ")");
                        else
                            args.Add("Expression.Constant(" + q.Name + ", typeof(" + pt.ToString2() + "))");
                    }

                    var pts = pt.ToString2();
                    var par = pts + " " + q.Name;
                    if (q.IsDefined(typeof(ParamArrayAttribute), false))
                        par = "params " + par;
                    pars.Add(par);
                    ptps.Add(pts);

                    if (!q.ParameterType.IsValueType && !q.ParameterType.IsGenericParameter)
                        nulls.Add(q.Name);
                }

                var factory = hasProvider ? "provider" : p.First().Name + ".Provider";
                var requiresQueryProvider = ret.GetGenericTypeDefinition() == typeof(IQueryable<>);
                if (requiresQueryProvider)
                    factory = "((IQueryProvider)" + factory + ")";
                else if (!ret.IsGenericType || ret.GetGenericTypeDefinition() != _qbs)
                    factory = "(" + ret.ToString2() + ")" + factory;

                var genArgs = m.GetGenericArguments().Select(a => a.ToString2()).ToList();
                var g = genArgs.Count > 0 ? "<" + string.Join(", ", genArgs) + ">" : "";
                var name = m.Name;
                if (name == "ToEnumerable")
                    name = "ToQueryable";

                var isExp = m.GetCustomAttributes(true).Where(a => a.GetType().Name.Equals("ExperimentalAttribute")).Any();
                if (isExp)
                    WriteLine("#if !STABLE", true);

                var obsolete = m.GetCustomAttributes(typeof(ObsoleteAttribute), false).Cast<ObsoleteAttribute>().SingleOrDefault();

                var poundIf = false;
                if (name == "ObserveOn" || name == "SubscribeOn")
                {
                    if (p.Last().ParameterType.Name == "DispatcherScheduler")
                    {
                        WriteLine("#if !MONO", true);
                        poundIf = true;
                    }
                    if (p.Last().ParameterType.Name == "ControlScheduler")
                    {
                        WriteLine("#if DESKTOPCLR", true);
                        poundIf = true;
                    }
                }
                if (name == "ObserveOnDispatcher" || name == "SubscribeOnDispatcher")
                {
                    WriteLine("#if !MONO", true);
                    poundIf = true;
                }
                if (isCreateAsync || hasTask)
                {
                    WriteLine("#if !NO_TPL", true);
                    poundIf = true;
                }
                //if (name == "Remotable")
                //{
                //    WriteLine("#if DESKTOPCLR", true);
                //    poundIf = true;
                //    if (nulls.Contains("lease"))
                //        nulls.Remove("lease");
                //}
                if (isLargeArity)
                {
                    WriteLine("#if !NO_LARGEARITY", true);
                }

                var isFep = m.Name == "FromEventPattern";
                var isGenFep = isFep && m.GetGenericArguments().Any(a => a.Name == "TEventArgs");
                var isNonGenFep = isFep && !isGenFep;

                for (var r = 0; r < (isNonGenFep ? 2 : 1); r++)
                {
                    var retStr = ret.ToString2();

                    if (isNonGenFep)
                    {
                        if (r == 0)
                        {
                            WriteLine("#if !NO_EVENTARGS_CONSTRAINT", true);
                        }
                        else if (r == 1)
                        {
                            WriteLine("#else", true);
                            retStr = retStr.Replace("EventPattern<EventArgs>", "EventPattern<object>");
                        }
                    }

                    if (xmlDoc != null)
                    {
                        foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        if (hasProvider)
                            WriteLine("/// <param name=\"provider\">Query provider used to construct the IQbservable&lt;T&gt; data source.</param>");

                        foreach (var docLine in xmlDoc.Elements().Where(e => e.Name != "summary").SelectMany(e => e.ToString().Split('\n')))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        if (requiresQueryProvider)
                            WriteLine("/// <remarks>This operator requires the source's <see cref=\"IQbservableProvider\"/> object (see <see cref=\"IQbservable.Provider\"/>) to implement <see cref=\"IQueryProvider\"/>.</remarks>");
                    }

                    if (isExp)
                        WriteLine("[Experimental]");

                    if (obsolete != null)
                        WriteLine("[Obsolete(\"" + obsolete.Message + "\")]");

                    WriteLine("public static " + retStr + " " + name + g + "(" + string.Join(", ", pars) + ")");
                    if (isGenFep)
                    {
                        WriteLine("#if !NO_EVENTARGS_CONSTRAINT", true);
                        Indent();
                        WriteLine("where TEventArgs : EventArgs");
                        Outdent();
                        WriteLine("#endif", true);
                    }
                    else
                    {
                        var genCons = (from a in m.GetGenericArguments()
                                       from c in a.GetGenericParameterConstraints()
                                       select new { a, c })
                                      .ToList();

                        if (genCons.Count > 0)
                        {
                            Indent();
                            foreach (var gc in genCons)
                                WriteLine("where " + gc.a.Name + " : " + gc.c.Name);
                            Outdent();
                        }
                    }


                    WriteLine("{");
                    Indent();
                    foreach (var n in nulls)
                    {
                        WriteLine("if (" + n + " == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(\"" + n + "\");");
                        Outdent();
                    }
                    WriteLine("");

                    var gArg = ret.GetGenericArguments().Single().ToString2();
                    if (isNonGenFep && r == 1)
                    {
                        gArg = gArg.Replace("EventPattern<EventArgs>", "EventPattern<object>");
                    }

                    WriteLine("return " + factory + ".CreateQuery<" + gArg + ">(");
                    Indent();
                    WriteLine("Expression.Call(");
                    Indent();
                    WriteLine("null,");
                    var cma = args.Count > 0 ? "," : "";
                    WriteLine("#if CRIPPLED_REFLECTION", true);
                    WriteLine("InfoOf(() => " + typeName + "." + name + g + "(" + string.Join(", ", ptps.Select(pt => "default(" + pt + ")")) + "))" + cma);
                    WriteLine("#else", true);
                    if (!m.IsGenericMethod)
                        WriteLine("(MethodInfo)MethodInfo.GetCurrentMethod()" + cma);
                    else
                        WriteLine("((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(" + string.Join(", ", m.GetGenericArguments().Select(ga => "typeof(" + ga.Name + ")").ToArray()) + ")" + cma);
                    WriteLine("#endif", true);
                    for (int j = 0; j < args.Count; j++)
                        WriteLine(args[j] + (j < args.Count - 1 ? "," : ""));
                    Outdent();
                    WriteLine(")");
                    Outdent();
                    WriteLine(");");
                    Outdent();
                    WriteLine("}");

                    if (isNonGenFep && r == 1)
                        WriteLine("#endif", true);
                }

                if (poundIf)
                    WriteLine("#endif", true);
                if (isExp)
                    WriteLine("#endif", true);
                if (isLargeArity)
                    WriteLine("#endif", true);
                WriteLine("");
            }

            if (includeAsync)
            {
                GenerateAsync(docs, typeName);
            }

            Outdent();

            WriteLine(
@"}");

            Outdent();

            WriteLine(
@"}
");
            WriteLine(
@"#endif
");
            WriteLine(
@"#pragma warning restore 1591
");

        }

        static bool ContainsTask(Type t)
        {
            if (t == typeof(Task))
                return true;

            if (t.IsGenericType)
            {
                if (t.GetGenericTypeDefinition() == typeof(Task<>))
                    return true;

                return t.GetGenericArguments().Any(ContainsTask);
            }

            if (t.IsArray)
                return ContainsTask(t.GetElementType());

            return false;
        }

        static void GenerateAsync(IDictionary<string, XElement> docs, string typeName)
        {
            foreach (var ret in new[] { "Unit", "TResult" })
            {
                for (int i = 0; i <= 16; i++)
                {
                    if (i == 5)
                        WriteLine("#if !NO_LARGEARITY", true);

                    foreach (var withScheduler in new[] { false, true })
                    {
                        var genArgs = default(string[]);
                        var lamPars = default(string[]);
                        if (i == 0)
                        {
                            genArgs = new string[0];
                            lamPars = new string[0];
                        }
                        //else if (i == 1)
                        //{
                        //    genArgs = new[] { "TSource" };
                        //    lamPars = new[] { "t" };
                        //}
                        else
                        {
                            genArgs = Enumerable.Range(1, i).Select(j => "TArg" + j).ToArray();
                            lamPars = Enumerable.Range(1, i).Select(j => "t" + j).ToArray();
                        }

                        var fParam = ret == "Unit" ? "action" : "function";
                        var gConst = ret == "Unit" ? "Action" : "Func";

                        var retType = "Func<" + string.Join(", ", genArgs.Concat(new[] { "IQbservable<" + ret + ">" }).ToArray()) + ">";

                        if (ret != "Unit")
                            genArgs = genArgs.Concat(new[] { "TResult" }).ToArray();

                        var docName = "M:System.Reactive.Linq.Observable.ToAsync";
                        if (genArgs.Length > 0)
                            docName += "``" + genArgs.Length;

                        var docArg = ret == "Unit" ? "System.Action" : "System.Func";
                        if (genArgs.Length > 0)
                            docArg += "{" + string.Join(",", Enumerable.Range(0, genArgs.Length).Select(j => "``" + j)) + "}";

                        docName += "(" + docArg + (withScheduler ? ",System.Reactive.Concurrency.IScheduler" : "") + ")";

                        var xmlDoc = default(XElement);
                        if (!docs.TryGetValue(docName, out xmlDoc))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Missing XML documentation for {0}", docName);
                            Console.ResetColor();
                        }

                        var actType = "Expression<" + gConst + (genArgs.Length > 0 ? "<" + string.Join(", ", genArgs) + ">" : "") + ">";

                        var genArgss = genArgs.Length > 0 ? "<" + string.Join(", ", genArgs) + ">" : "";

                        if (xmlDoc != null)
                        {
                            foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                                WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                            WriteLine("/// <param name=\"provider\">Query provider used to construct the IQbservable&lt;T&gt; data source.</param>");

                            foreach (var docLine in xmlDoc.Elements().Where(e => e.Name != "summary").SelectMany(e => e.ToString().Split('\n')))
                                WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));
                        }

                        WriteLine("public static " + retType + " ToAsync" + genArgss + "(this IQbservableProvider provider, " + actType + " " + fParam + (withScheduler ? ", IScheduler scheduler" : "") + ")");
                        WriteLine("{");

                        Indent();

                        WriteLine("if (provider == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(\"provider\");");
                        Outdent();

                        WriteLine("if (" + fParam + " == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(\"" + fParam + "\");");
                        Outdent();

                        if (withScheduler)
                        {
                            WriteLine("if (scheduler == null)");
                            Indent();
                            WriteLine("throw new ArgumentNullException(\"scheduler\");");
                            Outdent();
                        }

                        WriteLine("");
                        WriteLine("#if CRIPPLED_REFLECTION", true);
                        var aprs = new List<string> { "IQbservableProvider", actType };
                        if (withScheduler)
                            aprs.Add("IScheduler");
                        WriteLine("var m = InfoOf(() => " + typeName + ".ToAsync" + genArgss + "(" + string.Join(", ", aprs.Select(pt => "default(" + pt + ")")) + "));");
                        WriteLine("#else", true);
                        if (genArgs.Length == 0)
                            WriteLine("var m = (MethodInfo)MethodInfo.GetCurrentMethod();");
                        else
                            WriteLine("var m = ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(" + string.Join(", ", genArgs.Select(a => "typeof(" + a + ")").ToArray()) + ");");
                        WriteLine("#endif", true);

                        WriteLine("return (" + string.Join(", ", lamPars) + ") => provider.CreateQuery<" + ret + ">(");
                        Indent();
                        WriteLine("Expression.Invoke(");
                        Indent();
                        WriteLine("Expression.Call(");
                        Indent();
                        WriteLine("null,");
                        WriteLine("m,");
                        WriteLine("Expression.Constant(provider, typeof(IQbservableProvider)),");
                        WriteLine(fParam + (withScheduler ? "," : ""));
                        if (withScheduler)
                            WriteLine("Expression.Constant(scheduler, typeof(IScheduler))");
                        Outdent();
                        WriteLine(")" + (lamPars.Length > 0 ? "," : ""));
                        var k = 0;
                        foreach (var e in genArgs.Zip(lamPars, (g, l) => new { g, l }))
                        {
                            WriteLine("Expression.Constant(" + e.l + ", typeof(" + e.g + "))" + (k < i - 1 ? "," : ""));
                            k++;
                        }
                        Outdent();
                        WriteLine(")");
                        Outdent();
                        WriteLine(");");

                        Outdent();

                        WriteLine("}");
                        WriteLine("");
                    }

                    if (i == 16)
                        WriteLine("#endif", true);
                }
            }

            WriteLine("");

            foreach (var ret in new[] { "Unit", "TResult" })
            {
                for (int i = 0; i < 15; i++)
                {
                    if (i == 3)
                        WriteLine("#if !NO_LARGEARITY", true);

                    var genArgs = default(string[]);
                    var lamPars = default(string[]);
                    if (i == 0)
                    {
                        genArgs = new string[0];
                        lamPars = new string[0];
                    }
                    else
                    {
                        genArgs = Enumerable.Range(1, i).Select(j => "TArg" + j).ToArray();
                        lamPars = Enumerable.Range(1, i).Select(j => "t" + j).ToArray();
                    }

                    var fParam = ret == "Unit" ? "action" : "function";

                    var retType = "Func<" + string.Join(", ", genArgs.Concat(new[] { "IQbservable<" + ret + ">" }).ToArray()) + ">";

                    var begType = "Expression<Func<" + string.Join(", ", genArgs.Concat(new[] { "AsyncCallback", "object", "IAsyncResult" }).ToArray()) + ">>";
                    var endType = ret == "Unit" ? "Expression<Action<IAsyncResult>>" : "Expression<Func<IAsyncResult, TResult>>";

                    if (ret != "Unit")
                        genArgs = genArgs.Concat(new[] { "TResult" }).ToArray();

                    var docName = "M:System.Reactive.Linq.Observable.FromAsyncPattern";
                    if (genArgs.Length > 0)
                        docName += "``" + genArgs.Length;

                    if (ret == "Unit")
                    {
                        var docArgB = "System.Func{" + string.Join(",", Enumerable.Range(0, genArgs.Length).Select(j => "``" + j)) + (genArgs.Length > 0 ? "," : "") + "System.AsyncCallback,System.Object,System.IAsyncResult}";
                        var docArgE = "System.Action{System.IAsyncResult}";
                        docName += "(" + docArgB + "," + docArgE + ")";
                    }
                    else
                    {
                        var docArgB = "System.Func{" + string.Join(",", Enumerable.Range(0, genArgs.Length - 1).Select(j => "``" + j)) + (genArgs.Length > 1 ? "," : "") + "System.AsyncCallback,System.Object,System.IAsyncResult}";
                        var docArgE = "System.Func{System.IAsyncResult,``" + (genArgs.Length - 1) + "}";
                        docName += "(" + docArgB + "," + docArgE + ")";
                    }

                    var xmlDoc = default(XElement);
                    if (!docs.TryGetValue(docName, out xmlDoc))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Missing XML documentation for {0}", docName);
                        Console.ResetColor();
                    }

                    var genArgss = genArgs.Length > 0 ? "<" + string.Join(", ", genArgs) + ">" : "";

                    if (xmlDoc != null)
                    {
                        foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        WriteLine("/// <param name=\"provider\">Query provider used to construct the IQbservable&lt;T&gt; data source.</param>");

                        foreach (var docLine in xmlDoc.Elements().Where(e => e.Name != "summary").SelectMany(e => e.ToString().Split('\n')))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));
                    }

                    WriteLine("#if PREFERASYNC", true);
                    WriteLine("[Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]");
                    WriteLine("#endif", true);

                    WriteLine("public static " + retType + " FromAsyncPattern" + genArgss + "(this IQbservableProvider provider, " + begType + " begin, " + endType + "end)");
                    WriteLine("{");

                    Indent();

                    WriteLine("if (provider == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(\"provider\");");
                    Outdent();

                    WriteLine("if (begin == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(\"begin\");");
                    Outdent();

                    WriteLine("if (end == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(\"end\");");
                    Outdent();

                    WriteLine("");

                    WriteLine("#if CRIPPLED_REFLECTION", true);
                    var aprs = new List<string> { "IQbservableProvider", begType, endType };
                    WriteLine("var m = InfoOf(() => " + typeName + ".FromAsyncPattern" + genArgss + "(" + string.Join(", ", aprs.Select(pt => "default(" + pt + ")")) + "));");
                    WriteLine("#else", true);
                    if (genArgs.Length == 0)
                        WriteLine("var m = (MethodInfo)MethodInfo.GetCurrentMethod();");
                    else
                        WriteLine("var m = ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(" + string.Join(", ", genArgs.Select(a => "typeof(" + a + ")").ToArray()) + ");");
                    WriteLine("#endif", true);

                    WriteLine("return (" + string.Join(", ", lamPars) + ") => provider.CreateQuery<" + ret + ">(");
                    Indent();
                    WriteLine("Expression.Invoke(");
                    Indent();
                    WriteLine("Expression.Call(");
                    Indent();
                    WriteLine("null,");
                    WriteLine("m,");
                    WriteLine("Expression.Constant(provider, typeof(IQbservableProvider)),");
                    WriteLine("begin,");
                    WriteLine("end");
                    Outdent();
                    WriteLine(")" + (lamPars.Length > 0 ? "," : ""));
                    var k = 0;
                    foreach (var e in genArgs.Zip(lamPars, (g, l) => new { g, l }))
                    {
                        WriteLine("Expression.Constant(" + e.l + ", typeof(" + e.g + "))" + (k < i - 1 ? "," : ""));
                        k++;
                    }
                    Outdent();
                    WriteLine(")");
                    Outdent();
                    WriteLine(");");

                    Outdent();

                    WriteLine("}");
                    WriteLine("");

                    if (i == 14)
                        WriteLine("#endif", true);
                }
            }
        }

        static TextWriter Out { get; set; }
        static int _indent;

        static void WriteLine(string s, bool noIndent = false)
        {
            foreach (var t in s.Split('\n'))
                Out.WriteLine((noIndent ? "" : new string(' ', _indent * 4)) + t.TrimEnd('\r'));
        }

        static void Indent()
        {
            _indent++;
        }

        static void Outdent()
        {
            _indent--;
        }

        static Type Iconize(this Type type)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var g = type.GetGenericTypeDefinition();
                if (g == typeof(IObservable<>))
                {
                    return _qbs.MakeGenericType(type.GetGenericArguments());
                }
                else if (g.FullName == OrderedObservableFullName)
                {
                    return _oqbs.MakeGenericType(type.GetGenericArguments());
                }
                else if (g == typeof(IEnumerable<>))
                {
                    return typeof(IQueryable<>).MakeGenericType(type.GetGenericArguments());
                }
            }

            return type;
        }

        static string ToString2(this Type type)
        {
            if (type == typeof(int))
                return "int";
            else if (type == typeof(uint))
                return "uint";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(ulong))
                return "ulong";
            else if (type == typeof(float))
                return "float";
            else if (type == typeof(double))
                return "double";
            else if (type == typeof(byte))
                return "byte";
            else if (type == typeof(sbyte))
                return "sbyte";
            else if (type == typeof(bool))
                return "bool";
            else if (type == typeof(short))
                return "short";
            else if (type == typeof(ushort))
                return "ushort";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(object))
                return "object";
            else if (type == typeof(void))
                return "void";
            else if (type == typeof(decimal))
                return "decimal";

            if (type.IsArray)
                return type.GetElementType().ToString2() + "[" + new string(',', type.GetArrayRank() - 1) + "]";

            if (type.IsGenericType)
            {
                if (!type.IsGenericTypeDefinition)
                {
                    var g = type.GetGenericTypeDefinition();
                    if (g == typeof(Nullable<>))
                        return type.GetGenericArguments()[0].ToString2() + "?";
                    else
                        return g.ToString2() + "<" + string.Join(", ", type.GetGenericArguments().Select(t => t.ToString2()).ToArray()) + ">";
                }
                else
                {
                    var s = type.Name;
                    return s.Substring(0, s.LastIndexOf('`'));
                }
            }

            return type.Name;
        }

        static string ToDocName(MethodInfo method)
        {
            var name = "M:" + ToDocName(method.DeclaringType) + "." + method.Name;

            var genArgs = new Type[0];

            if (method.IsGenericMethod)
            {
                genArgs = method.GetGenericArguments();
                name += "``" + genArgs.Length;
            }

            var pars = method.GetParameters();
            if (pars.Length > 0)
            {
                name += "(" + string.Join(",", method.GetParameters().Select(p => ToDocName(p.ParameterType, genArgs))) + ")";
            }

            return name;
        }

        static string ToDocName(Type t, params Type[] genArgs)
        {
            var i = Array.IndexOf(genArgs, t);
            if (i >= 0)
                return "``" + i;

            if (t.IsArray)
            {
                return ToDocName(t.GetElementType(), genArgs) + "[]";
            }

            if (t.IsGenericType)
            {
                var def = t.GetGenericTypeDefinition();
                var name = def.FullName.Substring(0, def.FullName.LastIndexOf("`"));

                var args = t.GetGenericArguments();

                name += "{" + string.Join(",", args.Select(a => ToDocName(a, genArgs))) + "}";

                return name;
            }
            else
            {
                return t.FullName;
            }
        }
    }
}
