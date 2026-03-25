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

            var uri = new Uri(Assembly.GetEntryAssembly().Location);

            var root = Path.Combine(Path.GetDirectoryName(uri.LocalPath), @"..\..\..\..\..\Source\src");
            if (!Directory.Exists(root))
            {
                Console.WriteLine("Error:  Could not find directory \"" + root + "\"");
                return;
            }

            Process(root,
                "System.Reactive", 
                @"System.Reactive\Linq\Qbservable.Homoicon.cs", 
                "System.Reactive.Linq.Observable", "Qbservable",
                includeAsync: true, excludeFromCodeCoverage:true);
            Console.WriteLine();

            Process(root, 
                "System.Reactive",
                @"System.Reactive\Linq\QbservableEx.Homoicon.cs", 
                "System.Reactive.Linq.ObservableEx", "QbservableEx");            
            Console.WriteLine();

            Process(root, 
                "System.Reactive.Observable.Aliases",
                @"System.Reactive.Observable.Aliases\Qbservable.Aliases.Homoicon.cs",
                "System.Reactive.Observable.Aliases.QueryLanguage", "QbservableAliases",
                includeAsync: false, createAliases: true, excludeFromCodeCoverage: true,
                tfm: "netstandard2.0",
                includeNullability: false);
            Console.WriteLine();

            Console.WriteLine("Processing complete, press enter to continue.");
            Console.ReadLine();
        }

        static void Process(string root, string sourceAssembly, string targetFile, string sourceTypeName, string targetTypeName, bool includeAsync = false, bool createAliases = false, bool excludeFromCodeCoverage = false, string tfm = "net8.0", bool includeNullability = true)
        {
            var rxRoot = Path.Combine(root, sourceAssembly);
            if (!Directory.Exists(rxRoot))
            {
                Console.WriteLine("Error:  Could not find directory \"" + rxRoot + "\"");
                return;
            }

            var dll = Path.Combine(rxRoot, @$"bin\debug\{tfm}\" + sourceAssembly + ".dll");
            if (!File.Exists(dll))
            {
                Console.WriteLine("Error:  Could not find file \"" + dll + "\"");
                return;
            }

            var xml = Path.Combine(rxRoot, @$"bin\debug\{tfm}\" + sourceAssembly + ".xml");
            if (!File.Exists(xml))
            {
                Console.WriteLine("Error:  Could not find file \"" + xml + "\"");
                return;
            }

            var qbsgen = Path.Combine(root, targetFile);
            if (!File.Exists(qbsgen))
            {
                Console.WriteLine("Error:  Could not find file \"" + qbsgen + "\"");
                return;
            }

            Generate(dll, xml, qbsgen, sourceTypeName, targetTypeName, includeAsync, createAliases, excludeFromCodeCoverage, includeNullability);
        }

        // Prototype interface to break dependencies. Only used for ToString2 ultimately.
        interface IQbservable<T>
        {
        }

        static void Generate(string input, string xml, string output, string sourceTypeName, string targetTypeName, bool includeAsync, bool createAliases, bool exludeFromCodeCoverage, bool includeNullability)
        {
            var docs = XDocument.Load(xml).Root.Element("members").Elements("member").ToDictionary(m => m.Attribute("name").Value, m => m);

            Console.WriteLine("Loading {0}...", input);

            var asm = Assembly.LoadFrom(input);
            var t = asm.GetType(sourceTypeName);
            _qbs = typeof(IQbservable<>); 

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
                    Generate(t, docs, targetTypeName, includeAsync, createAliases, exludeFromCodeCoverage, includeNullability);
                }
            }
        }

        static Type _qbs;

        static void Generate(Type t, IDictionary<string, XElement> docs, string typeName, bool includeAsync, bool createAliases, bool exludeFromCodeCoverage, bool includeNullability)
        {
            WriteLine(
@"/*
 * WARNING: Auto-generated file (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @")
 * Run Rx's auto-homoiconizer tool to generate this file (in the HomoIcon directory).
 */
");
            WriteLine(
@"#nullable enable
#pragma warning disable 1591
");

            WriteLine(
@"using System.Collections.Generic;");

            if (exludeFromCodeCoverage)
                WriteLine("using System.Diagnostics.CodeAnalysis;");

            WriteLine(
@"using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
");
            WriteLine(
@"namespace System.Reactive.Linq
{");

            Indent();

            if (exludeFromCodeCoverage)
                WriteLine("[ExcludeFromCodeCoverage]");

            WriteLine(
@"public static partial class " + typeName + @"
{");

            Indent();

            var except = new[] { "ToAsync", "FromAsyncPattern", "And", "Then", "GetEnumerator", "get_Provider", "Wait", "ForEach", "ForEachAsync", "GetAwaiter", "RunAsync", "First", "FirstOrDefault", "Last", "LastOrDefault", "Single", "SingleOrDefault", "Subscribe", "AsQbservable", "AsObservable", "ToEvent", "ToEventPattern" };

            var items = t.GetMethods(BindingFlags.Public | BindingFlags.Static).OrderBy(m => m.Name).ThenBy(m => !m.IsGenericMethod ? "" : string.Join(",", m.GetGenericArguments().Select(p => p.Name))).ThenBy(m => string.Join(",", m.GetParameters().Select(p => p.Name + ":" + p.ParameterType.FullName))).Where(m => !except.Contains(m.Name)).ToList();
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

                var ret = m.ReturnType;
                if (ret.IsGenericType)
                {
                    var d = ret.GetGenericTypeDefinition();
                    if (d.Name.StartsWith("IConnectableObservable") || d.Name.StartsWith("ListObservable"))
                        continue;
                    if (d != typeof(IObservable<>) && d != typeof(IEnumerable<>))
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
                        if (d == typeof(IObservable<>)) // Check - e.g. Amb    || d == typeof(IEnumerable<>))
                            hasProvider = false;
                    }
                }

                var nulls = new List<string>();
                var pars = new List<string>();
                var parNames = new List<string>();
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
                parNames.Add(firstName);

                var rem = hasProvider ? p : p.Skip(1);
                foreach (var q in rem)
                {
                    var pt = q.ParameterType;
                    if (pt.Name.StartsWith("Func") || pt.Name.StartsWith("Action"))
                    {
                        pt = typeof(Expression<>).MakeGenericType(pt);
                        args.Add(q.Name);
                    }
                    else
                    {
                        var isObs = new Func<Type, bool>(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IObservable<>));
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
                    parNames.Add(q.Name);

                    if (!q.ParameterType.IsValueType && !q.ParameterType.IsGenericParameter)
                        nulls.Add(q.Name);
                }

                var factory = hasProvider ? "provider" : p.First().Name + ".Provider";
                var requiresQueryProvider = ret.GetGenericTypeDefinition() == typeof(IQueryable<>);
                if (requiresQueryProvider)
                    factory = "((IQueryProvider)" + factory + ")";
                
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

                {
                    var retStr = ret.ToString2(m.ReturnParameter.GetCustomAttributesData());

                    if (xmlDoc != null)
                    {
                        SimplifyCrefAttribute(xmlDoc);

                        foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        if (hasProvider)
                            WriteLine("/// <param name=\"provider\">Query provider used to construct the <see cref=\"IQbservable{T}\"/> data source.</param>");

                        var rest = xmlDoc
                            .Elements()
                            .Where(e => e.Name != "summary")
                            .SelectMany(e => e.ToString().Split('\n'));

                        foreach (var docLine in rest)
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        if (requiresQueryProvider)
                            WriteLine("/// <remarks>This operator requires the source's <see cref=\"IQbservableProvider\"/> object (see <see cref=\"IQbservable.Provider\"/>) to implement <see cref=\"IQueryProvider\"/>.</remarks>");
                    }

                    if (isExp)
                        WriteLine("[Experimental]");

                    if (obsolete != null)
                        WriteLine("[Obsolete(\"" + obsolete.Message + "\")]");

                    WriteLine("public static " + retStr + " " + name + g + "(" + string.Join(", ", pars) + ")");

                    var genCons = (from a in m.GetGenericArguments()
                                   from c in a.GetGenericParameterConstraints()
                                   select new { a, c })
                                  .ToList();

                    HashSet<string> alreadyEmittedTypeConstraintFor = [];
                    if (genCons.Count > 0)
                    {
                        Indent();
                        foreach (var gc in genCons)
                        {
                            WriteLine("where " + gc.a.Name + " : " + gc.c.Name);
                            alreadyEmittedTypeConstraintFor.Add(gc.a.Name);
                        }
                        Outdent();
                    }

                    foreach (var a in m.GetGenericArguments())
                    {
                        if (alreadyEmittedTypeConstraintFor.Contains(a.Name))
                        {
                            continue;
                        }

                        bool isNotNull = true;
                        if (a.GetCustomAttributesData().SingleOrDefault((CustomAttributeData a) => a.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute")
                            is CustomAttributeData gana)
                        {
                            switch (gana.ConstructorArguments.Single().Value)
                            {
                                case (byte)2:
                                    isNotNull = false;
                                    break;
                            }
                        }
                        if (isNotNull && includeNullability)
                        {
                            WriteLine($"where {a.Name} : notnull");
                        }
                    }


                    if (createAliases)
                    {
                        string underlying = "";

                        switch (name)
                        {
                            case "Map":
                                underlying = "Select";
                                break;
                            case "FlatMap":
                                underlying = "SelectMany";
                                break;
                            case "Filter":
                                underlying = "Where";
                                break;
                        }

                        WriteLine("{");
                        Indent();
                        WriteLine("return Qbservable." + underlying + g + "(" + string.Join(", ", parNames) + ");");
                        Outdent();
                        WriteLine("}");
                        WriteLine("");
                        continue;
                    }

                    WriteLine("{");
                    Indent();
                    foreach (var n in nulls)
                    {
                        WriteLine("if (" + n + " == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(nameof(" + n + "));");
                        Outdent();
                    }
                    WriteLine("");

                    var gArg = ret.GetGenericArguments().Single().ToString2(m.ReturnParameter.GetCustomAttributesData());

                    WriteLine("return " + factory + ".CreateQuery<" + gArg + ">(");
                    Indent();
                    WriteLine("Expression.Call(");
                    Indent();
                    WriteLine("null,");
                    var cma = args.Count > 0 ? "," : "";
                    WriteLine($"new Func<{string.Join(", ", ptps)}, {retStr}>({name}{g}).Method,");
                    for (int j = 0; j < args.Count; j++)
                        WriteLine(args[j] + (j < args.Count - 1 ? "," : ""));
                    Outdent();
                    WriteLine(")");
                    Outdent();
                    WriteLine(");");
                    Outdent();
                    WriteLine("}");

                }

                if (poundIf)
                    WriteLine("#endif", true);
                if (isExp)
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
@"#pragma warning restore 1591
");

        }

        private static void SimplifyCrefAttribute(XElement parent)
        {
            foreach (var element in parent.Descendants())
            {
                var att = element.Attribute("cref");
                if (att != null)
                {
                    element.Attribute("cref").SetValue(SimplyfyDocType(att.Value as string));
                }
            }
        }

        private static string SimplyfyDocType(string cref)
        {
            var v = cref.Replace("T:System.", "")
                 .Replace("F:System.", "");
            switch (v)
            {
                case "Double": return "double";
                case "Decimal": return "decimal";
                case "Int32": return "int";
                case "Inte16": return "short";
                case "Int64": return "long";
                case "Single": return "float";
                case "IObservable`1": return "IObservable{T}";
            }

            if (v.Contains(".") || v.Contains("`"))
                return cref;
            return v;
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
                    foreach (var withScheduler in new[] { false, true })
                    {
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
                            SimplifyCrefAttribute(xmlDoc);

                            foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                                WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                            WriteLine("/// <param name=\"provider\">Query provider used to construct the <see cref=\"IQbservable{T}\"/> data source.</param>");

                            foreach (var docLine in xmlDoc.Elements().Where(e => e.Name != "summary").SelectMany(e => e.ToString().Split('\n')))
                                WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));
                        }

                        WriteLine("public static " + retType + " ToAsync" + genArgss + "(this IQbservableProvider provider, " + actType + " " + fParam + (withScheduler ? ", IScheduler scheduler" : "") + ")");
                        WriteLine("{");

                        Indent();

                        WriteLine("if (provider == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(nameof(provider));");
                        Outdent();

                        WriteLine("if (" + fParam + " == null)");
                        Indent();
                        WriteLine("throw new ArgumentNullException(nameof(" + fParam + "));");
                        Outdent();

                        if (withScheduler)
                        {
                            WriteLine("if (scheduler == null)");
                            Indent();
                            WriteLine("throw new ArgumentNullException(nameof(scheduler));");
                            Outdent();
                        }

                        WriteLine("");
                        WriteLine($"var m = new Func<IQbservableProvider, {actType}, {retType}>(ToAsync).Method;");

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
                }
            }

            WriteLine("");

            foreach (var ret in new[] { "Unit", "TResult" })
            {
                for (int i = 0; i < 15; i++)
                {
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
                        SimplifyCrefAttribute(xmlDoc);
                        foreach (var docLine in xmlDoc.Element("summary").ToString().Split('\n'))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));

                        WriteLine("/// <param name=\"provider\">Query provider used to construct the <see cref=\"IQbservable{T}\"/> data source.</param>");

                        foreach (var docLine in xmlDoc.Elements().Where(e => e.Name != "summary").SelectMany(e => e.ToString().Split('\n')))
                            WriteLine("/// " + docLine.TrimStart().TrimEnd('\r'));
                    }

                    WriteLine("#if PREFERASYNC", true);
                    WriteLine("[Obsolete(Constants_Linq.USE_TASK_FROMASYNCPATTERN)]");
                    WriteLine("#endif", true);

                    WriteLine("public static " + retType + " FromAsyncPattern" + genArgss + "(this IQbservableProvider provider, " + begType + " begin, " + endType + " end)");
                    WriteLine("{");

                    Indent();

                    WriteLine("if (provider == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(nameof(provider));");
                    Outdent();

                    WriteLine("if (begin == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(nameof(begin));");
                    Outdent();

                    WriteLine("if (end == null)");
                    Indent();
                    WriteLine("throw new ArgumentNullException(nameof(end));");
                    Outdent();

                    WriteLine("");
                    WriteLine($"var m = new Func<IQbservableProvider, {begType}, {endType}, {retType}>(FromAsyncPattern).Method;");

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
                }
            }
        }

        static TextWriter Out { get; set; }
        static int _indent;

        static void WriteLine(string s, bool noIndent = false)
        {
            foreach (var t in s.Split('\n'))
            {
                var indentspace = noIndent || string.IsNullOrWhiteSpace(t)
                    ? "" 
                    : new string(' ', _indent * 4);
                Out.WriteLine(indentspace + t.TrimEnd('\r'));
            }
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
                else if (g == typeof(IEnumerable<>))
                {
                    return typeof(IQueryable<>).MakeGenericType(type.GetGenericArguments());
                }
            }

            return type;
        }

        static string ToString2(this Type type, IEnumerable<CustomAttributeData> customAttributeData = null)
        {
            CustomAttributeData methodNullableAttribute = customAttributeData
                ?.SingleOrDefault((CustomAttributeData a) => a.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

            bool returnTypeIsNullable = false;
            if (methodNullableAttribute is not null)
            {
                if (methodNullableAttribute.ConstructorArguments.Single().Value is (byte)2)
                {
                    returnTypeIsNullable = true;
                }
            }

            CustomAttributeData tupleElementNamesAttribute = customAttributeData
                ?.SingleOrDefault((CustomAttributeData a) => a.AttributeType.FullName == "System.Runtime.CompilerServices.TupleElementNamesAttribute");

            string returnTypeSuffix = returnTypeIsNullable ? "?" : "";

            if (type == typeof(int))
                return "int" + returnTypeSuffix;
            else if (type == typeof(uint))
                return "uint" + returnTypeSuffix;
            else if (type == typeof(long))
                return "long" + returnTypeSuffix;
            else if (type == typeof(ulong))
                return "ulong" + returnTypeSuffix;
            else if (type == typeof(float))
                return "float" + returnTypeSuffix;
            else if (type == typeof(double))
                return "double" + returnTypeSuffix;
            else if (type == typeof(byte))
                return "byte" + returnTypeSuffix;
            else if (type == typeof(sbyte))
                return "sbyte" + returnTypeSuffix;
            else if (type == typeof(bool))
                return "bool" + returnTypeSuffix;
            else if (type == typeof(short))
                return "short" + returnTypeSuffix;
            else if (type == typeof(ushort))
                return "ushort" + returnTypeSuffix;
            else if (type == typeof(string))
                return "string" + returnTypeSuffix;
            else if (type == typeof(object))
                return "object" + returnTypeSuffix;
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
                    var genericArgs = type.GetGenericArguments();

                    if (type.Name.StartsWith("ValueTuple") && tupleElementNamesAttribute is not null)
                    {
                        static IEnumerable<Type> FlattenValueTupleTypeArguments(Type[] vt)
                        {
                            if (vt[^1].Name.StartsWith("ValueTuple"))
                            {
                                var nested = vt[^1].GetGenericArguments();
                                return vt.Take(vt.Length - 1).Concat(FlattenValueTupleTypeArguments(nested));

                            }
                            return vt;
                        }

                        if (tupleElementNamesAttribute.ConstructorArguments.Single().Value is IList<CustomAttributeTypedArgument> data)
                        {
                            var names = data.Select(a => a.Value as string).ToArray();
                            var valueTupleTypes = FlattenValueTupleTypeArguments(genericArgs);
                            return "(" + string.Join(", ", valueTupleTypes.Select((t, i) => t.ToString2() + " " + names[i]).ToArray()) + ")";
                        }
                    }

                    bool[] genericArgIsNullable = null;
                    if (methodNullableAttribute is not null)
                    {
                        if (methodNullableAttribute.ConstructorArguments.Single().Value is IList<CustomAttributeTypedArgument> data)
                        {
                            genericArgIsNullable = new bool[genericArgs.Length];
                            for (int i = 0; i < genericArgs.Length; i++)
                            {
                                if ((i + 1) < data.Count)
                                {
                                    genericArgIsNullable[i] = data[i + 1].Value is (byte)2;
                                }
                            }
                        }
                    }
                    string GetGenericTypeArgNullabilitySuffix(int index)
                    {
                        if (genericArgIsNullable != null && index < genericArgIsNullable.Length && genericArgIsNullable[index])
                        {
                            return "?";
                        }
                        return "";
                    }


                        var g = type.GetGenericTypeDefinition();
                    if (g == typeof(Nullable<>))
                        return type.GetGenericArguments()[0].ToString2() + "?";
                    else
                        return g.ToString2() + "<" + string.Join(", ", genericArgs.Select((t, i) => t.ToString2(customAttributeData) + GetGenericTypeArgNullabilitySuffix(i)).ToArray()) + ">";
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
