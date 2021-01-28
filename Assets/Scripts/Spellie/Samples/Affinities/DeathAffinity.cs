using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityDamageMultiplier(typeof(EarthAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(WaterAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(LifeAffinity), 2f)]
    [AffinityCombine(typeof(FireAffinity), typeof(AirAffinity))]
    public class DeathAffinity : SpellieAffinity
    {
        
    }
}