using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityDamageMultiplier(typeof(AirAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(WaterAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(HolyAffinity), 2f)]
    [AffinityCombine(typeof(FireAffinity), typeof(EarthAffinity))]
    public class ChaosAffinity : SpellieAffinity
    {
        
    }
}