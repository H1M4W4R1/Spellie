using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Entity
{
    /// <summary>
    /// Element applies effect to target. It can be either buff or debuff.
    /// You need to store effect logic in Entity.
    /// </summary>
    [SpellieElement("remove_effect")]
    public abstract class RemoveEffectElement
    {
        public class Builder : RemoveEffectElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class RemoveEffectElement<TSelf> : SpellieElement<RemoveEffectElement<TSelf>>
        where TSelf:RemoveEffectElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            // Apply effects to targets
            target.RemoveEffect(effectName);
            return NoCastResult.New();
        }
        
        [SpellieProperty("name", true)] public string effectName;

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

    }
}