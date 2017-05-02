using System;

namespace PlantingLib.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
    {
        public AspMvcAreaMasterLocationFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}