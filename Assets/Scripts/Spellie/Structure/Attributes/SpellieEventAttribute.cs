using System;

namespace Spellie.Structure.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpellieEventAttribute : Attribute
    {

        /// <summary>
        /// Property to select
        /// </summary>
        public string Name;

        /// <summary>
        /// True if property is required for spell element
        /// </summary>
        public bool Required;

        public SpellieEventAttribute(string name, bool required = false)
        {
            Name = name;
            Required = required;
        }

    }
}