using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityDamageMultiplier(typeof(FireAffinity), 2f)]
    public class WaterAffinity : SpellieAffinity
    {
        
    }
}