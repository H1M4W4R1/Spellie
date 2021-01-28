using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    [AffinityCombine(typeof(WaterAffinity), typeof(FireAffinity))]
    [AffinityCombine(typeof(EarthAffinity), typeof(AirAffinity))]
    [AffinityCombine(typeof(DeathAffinity), typeof(LifeAffinity))]
    [AffinityCombine(typeof(ChaosAffinity), typeof(HolyAffinity))]
    public class NoneAffinity : SpellieAffinity
    {
        
    }
}