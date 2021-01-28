using Spellie.Affiinities;
using Spellie.CastResults;
using Spellie.Config;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Entity
{
    /// <summary>
    /// Damage element is responsible for damaging entities, and possibly killing them.
    /// Beware that damage can be afflicted by affinity. To know more about affinity read: //TODO: DOCS
    /// </summary>
    [SpellieElement("damage")]
    public abstract class DamageElement
    {
        public class Builder : DamageElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class DamageElement<TSelf> : SpellieElement<DamageElement<TSelf>>, IDamageSource
        where TSelf:DamageElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            // Check if damage has affinity
            var aff = SpellieSingleton.Instance.Get(affinity);
            
            // If true
            if (aff != null)
            {
                // Damage with multiplier
                target.Damage((int)(aff.GetDamageMultiplier(target.GetAffinityType())) * amount, this);
            }
            else
            {   
                // Deal neutral damage
                target.Damage(amount, this);
            }
            
            // Damage is not based on targets, because primary skills are based on targets,
            // Effects are only applied onto processed data
            return NoCastResult.New();
        }
        
        [SpellieProperty("affinity")] public string affinity;

        /// <summary>
        /// Sets affinity of this Damage
        /// </summary>
        /// <param name="aff"></param>
        /// <returns></returns>
        public TSelf WithAffinity(string aff)
        {
            Set("affinity", aff);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Sets affinity of this Damage
        /// </summary>
        /// <param name="aff"></param>
        /// <returns></returns>
        public TSelf WithAffinity(SpellieAffinity aff)
        {
            var dat = aff?.ToString().Split('.');
            var a = dat?[dat.Length - 1].ToLower().Replace("affinity", "");
            
            Set("affinity", a);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Amount of damage to deal
        /// </summary>
        [SpellieProperty("amount", true)]
        public int amount = 1;

        /// <summary>
        /// Defines amount of damage to deal
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public TSelf Deals(int damage)
        {
            Set("amount", damage);
            return (TSelf) this;
        }
        
    }
}