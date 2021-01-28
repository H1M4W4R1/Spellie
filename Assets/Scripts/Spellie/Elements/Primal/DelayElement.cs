using System.Collections;
using System.Collections.Generic;
using Spellie.CastResults;
using Spellie.Config;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Elements.Primal
{
    [SpellieElement("delay")]
    public abstract class DelayElement
    {
        public class Builder : DelayElement<Builder>{}

        public static Builder New() => new Builder();
    }

    public class DelayElement<TSelf> : SpellieElement<TSelf>
        where TSelf : DelayElement<TSelf>
    {
        public IEnumerator CastImpl(IEntity caster, ICastSource source, IEntity target)
        {
            yield return new WaitForSeconds(time);
            try
            {
                target.GetUpAxis();
                onDelayPassed.InvokeAll(source, caster, target);
            }
            catch
            {
                // Do nothing, target died
            }
        }
        
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            SpellieSingleton.Instance.StartCoroutine(CastImpl(caster, source, target));
            return new NoCastResult();
        }

        [SpellieProperty("time", true)]
        public float time;

        /// <summary>
        /// Sets time of delay
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public TSelf WithTime(float t)
        {
            time = t;
            return (TSelf) this;
        }

        [SpellieEvent("onDelayPassed", true)]
        public SpellieEvent onDelayPassed;
    }
}
