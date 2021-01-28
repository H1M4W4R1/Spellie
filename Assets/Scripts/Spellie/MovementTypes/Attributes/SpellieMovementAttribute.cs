using System;

namespace Spellie.MovementTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SpellieMovementAttribute : Attribute
    {
        public string name;

        public SpellieMovementAttribute(string eName)
        {
            name = eName;
        }
    }
}