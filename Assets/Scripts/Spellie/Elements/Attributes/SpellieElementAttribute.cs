using System;

namespace Spellie.Elements.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SpellieElementAttribute : Attribute
    {
        public string name;

        public SpellieElementAttribute(string eName)
        {
            name = eName;
        }
    }
}