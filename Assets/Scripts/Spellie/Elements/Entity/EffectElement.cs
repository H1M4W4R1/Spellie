using Spellie.CastResults;
using Spellie.Effects;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Entity
{
    /// <summary>
    /// Element applies effect to target. It can be either buff or debuff.
    /// You need to store effect logic in Entity.
    /// </summary>
    [SpellieElement("effect")]
    public abstract class EffectElement
    {
        public class Builder : EffectElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class EffectElement<TSelf> : SpellieElement<EffectElement<TSelf>>
        where TSelf:EffectElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            // Apply effects to targets
            target.ApplyEffect( Effect.New(this, effectName, duration, target));
            return NoCastResult.New();
        }
        
        [SpellieProperty("name", true)] public string effectName;

        [SpellieProperty("duration")] public float duration;

        /// <summary>
        /// Sets effect name/id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TSelf WithEffect(string name)
        {
            Set("name", name);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Sets effect duration
        /// duration lower or equal to 0 leads to permanent effect (unless removed otherwise)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TSelf WithDuration(float time)
        {
            Set("duration", time);
            return (TSelf) this;
        }
        
        [SpellieEvent("onEffectRemoved")]
        public SpellieEvent onEffectRemoved = new SpellieEvent();
        
        [SpellieEvent("onEffectWearOff")]
        public SpellieEvent onEffectWearOff = new SpellieEvent();
        
        [SpellieEvent("onEffectApplied")]
        public SpellieEvent onEffectApplied = new SpellieEvent();
        
    }
}