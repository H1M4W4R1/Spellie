using System;
using System.Collections.Generic;
using System.Linq;
using Spellie.Affiinities.Attributes;
using Spellie.Config;
using Spellie.Samples.Affinities;

namespace Spellie.Affiinities
{
    /// <summary>
    /// Affinity is an damage type - fire, cold, water, chaos etc.
    /// </summary>
    public class SpellieAffinity
    {

        /// <summary>
        /// This affinity damage multipliers
        /// </summary>
        public Dictionary<Type, float> damageMultipliers = new Dictionary<Type, float>();

        /// <summary>
        /// Searches for combination of affinities, returns <see cref="UndefinedAffinity"/> if does not exist.
        /// </summary>
        /// <param name="affinities">Affinities to combine</param>
        /// <returns></returns>
        public static Type GetCombinationOf(params Type[] affinities)
        {
            // Get all known affinities
            var aff = SpellieSingleton.Instance.GetAffinities().Where(t => t.GetType().IsSubclassOf(typeof(SpellieAffinity)));
            
            // For all affinities
            foreach (var type in aff)
            {
                // Get affinity possible combinations
                var attributes = type.GetType().GetCustomAttributes(typeof(AffinityCombineAttribute), true) as AffinityCombineAttribute[];
                if (attributes == null) continue;
                
                // Check if affinities count on attribute is correct
                // Check if all affinities are same as affinities being combined
                if (attributes.Where(affinityCombineAttribute => affinityCombineAttribute.affinities.Length == affinities.Length)
                    .Any(affinityCombineAttribute => affinities.All(a => affinityCombineAttribute.affinities.Contains(a))))
                {
                    // We found proper affinity, return
                    return type.GetType();
                }
            }

            // Unknown affinity
            return typeof(UndefinedAffinity);
        }
        
        /// <summary>
        /// Gets damage multiplier against specified affinity
        /// </summary>
        /// <param name="affinity"></param>
        /// <returns></returns>
        public float GetDamageMultiplier(Type affinity)
        {
            // Check if damage can be multiplied
            if (damageMultipliers.ContainsKey(affinity))
                return damageMultipliers[affinity];

            // Damage cannot be multiplier return neutral element
            return 1f;
        }
        
        /// <summary>
        /// Initializes affinity
        /// </summary>
        public void Initialize()
        {
            // Get all damage multipliers
            var attr = GetType().GetCustomAttributes(typeof(AffinityDamageMultiplierAttribute), true);
            
            // For all dmg multipliers
            foreach (var a in attr)
            {
                // Add highest multiplier to cache
                var b = (AffinityDamageMultiplierAttribute) a;
                if (damageMultipliers.ContainsKey(b.targetAffinity))
                {
                    if(damageMultipliers[b.targetAffinity] < b.multiplier)
                        damageMultipliers[b.targetAffinity] = b.multiplier;
                }
                else
                    damageMultipliers[b.targetAffinity] = b.multiplier;
            }
        }
    }
}