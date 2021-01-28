using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Primal
{
    /// <summary>
    /// Switching element is an element that represents switchable spell.
    /// Example of that spell type may be aura - it's switched on and off.
    /// You can use ICastResult to switch the spell off on your demand.
    /// </summary>
    [SpellieElement("switching")]
    public abstract class SwitchingElement
    {
        public class Builder : SwitchingElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class SwitchingElement<TSelf> : SpellieElement<SwitchingElement<TSelf>>
        where TSelf:SwitchingElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            return SwitchCastResult.New(this, caster, target);
        }
        
        [SpellieEvent("onBegin", true)]
        public SpellieEvent onBegin;
        
        [SpellieEvent("onSwitch", true)]
        public SpellieEvent onSwitch;
        
        [SpellieEvent("onEnd", true)]
        public SpellieEvent onEnd;
    }
}