using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    
    [AffinityDamageMultiplier(typeof(EarthAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(FireAffinity), 1.5f)]
    [AffinityDamageMultiplier(typeof(ChaosAffinity), 2f)]
    [AffinityCombine(typeof(WaterAffinity), typeof(AirAffinity))]
    public class HolyAffinity : SpellieAffinity
    {
        
    }
}