// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_ASSEMBLYFILEVERSIONATTRIBUTE
namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyFileVersionAttribute : Attribute
    {
        private readonly string _version;

        public AssemblyFileVersionAttribute(string version)
        {
             _version = version;     
        }

        public string Version
        {
            get { return _version; }
        }
    }
}
#endif