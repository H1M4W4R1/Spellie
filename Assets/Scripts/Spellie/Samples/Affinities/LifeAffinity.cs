using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityDamageMultiplier(typeof(AirAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(FireAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(DeathAffinity), 2f)]
    [AffinityCombine(typeof(WaterAffinity), typeof(EarthAffinity))]
    public class LifeAffinity : SpellieAffinity
    {
        
    }
}