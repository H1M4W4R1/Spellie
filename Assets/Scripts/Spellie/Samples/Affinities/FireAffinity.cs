using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityDamageMultiplier(typeof(WaterAffinity), 2f)]
    public class FireAffinity : SpellieAffinity
    {
        
    }
}