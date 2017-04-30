using System;

namespace PlantingLib.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RazorImportNamespaceAttribute : Attribute
    {
        public RazorImportNamespaceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}