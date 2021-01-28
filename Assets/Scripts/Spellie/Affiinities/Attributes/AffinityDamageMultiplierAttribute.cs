using System;

namespace Spellie.Affiinities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AffinityDamageMultiplierAttribute : Attribute
    {

        /// <summary>
        /// Target Affinity
        /// </summary>
        public readonly Type targetAffinity;

        /// <summary>
        /// Damage multiplier
        /// </summary>
        public readonly float multiplier;

        public AffinityDamageMultiplierAttribute(Type target, float value = 1f)
        {
            targetAffinity = target;
            multiplier = value;
        }

    }
}