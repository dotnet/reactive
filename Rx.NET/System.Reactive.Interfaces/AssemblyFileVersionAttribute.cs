// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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