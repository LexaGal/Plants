using System;

namespace PlantingLib.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcViewLocationFormatAttribute : Attribute
    {
        public AspMvcViewLocationFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}