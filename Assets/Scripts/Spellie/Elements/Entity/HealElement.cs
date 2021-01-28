using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Entity
{
    /// <summary>
    /// Heal Element is responsible for healing entities.
    /// </summary>
    [SpellieElement("heal")]
    public abstract class HealElement
    {
        public class Builder : HealElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class HealElement<TSelf> : SpellieElement<HealElement<TSelf>>, IHealSource
        where TSelf:HealElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            target.Heal(amount, this);
            return NoCastResult.New();
        }
        
        /// <summary>
        /// Amount of hp to heal
        /// </summary>
        [SpellieProperty("amount", true)]
        public int amount = 1;

        /// <summary>
        /// Defines amount of hp to heal
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public TSelf Heals(int hp)
        {
            Set("amount", hp);
            return (TSelf) this;
        }
        
    }
}