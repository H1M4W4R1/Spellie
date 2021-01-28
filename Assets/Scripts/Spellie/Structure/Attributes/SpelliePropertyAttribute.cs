using System;

namespace Spellie.Structure.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpelliePropertyAttribute : Attribute
    {

        /// <summary>
        /// Property to select
        /// </summary>
        public string Name;

        /// <summary>
        /// True if property is required for spell element
        /// </summary>
        public bool Required;

        public SpelliePropertyAttribute(string name, bool required = false)
        {
            Name = name;
            Required = required;
        }

    }
}