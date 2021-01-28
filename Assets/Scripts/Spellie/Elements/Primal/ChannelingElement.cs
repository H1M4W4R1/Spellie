using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Primal
{
    /// <summary>
    /// Channeling element is responsible for channeling spells.
    /// It's useful to create projectiles that explode on channeling end, rays or beams.
    /// </summary>
    [SpellieElement("channeling")]
    public abstract class ChannelingElement
    {
        public class Builder : ChannelingElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class ChannelingElement<TSelf> : SpellieElement<ChannelingElement<TSelf>>
        where TSelf:ChannelingElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            return ChannelingCastResult.New(this, caster, target, source);
        }
        
        /// <summary>
        /// Amount of channels per second
        /// </summary>
        [SpellieProperty("frequency", true)]
        public float frequency = 1;

        /// <summary>
        /// Defines amount of channeling events per second
        /// eg. 2 = 2 channels per second
        /// 0.5 = 1 channel each 2 seconds
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public TSelf WithFrequency(int freq)
        {
            Set("frequency", freq);
            return (TSelf) this;
        }

        [SpellieEvent("onChannel", true)]
        public SpellieEvent onChannel;

        [SpellieEvent("onChannelStarted")]
        public SpellieEvent onChannelStarted;
        
        [SpellieEvent("onChannelWearOff")]
        public SpellieEvent onChannelWearOff;
    }
}