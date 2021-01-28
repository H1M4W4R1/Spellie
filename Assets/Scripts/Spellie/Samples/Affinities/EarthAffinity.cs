using Spellie.Affiinities;
using Spellie.Affiinities.Attributes;

namespace Spellie.Samples.Affinities
{
    
    [AffinityDamageMultiplier(typeof(AirAffinity), 2f)]
    public class EarthAffinity : SpellieAffinity
    {
        
    }
}