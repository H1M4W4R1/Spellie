using System.Collections;
using System.Collections.Generic;
using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie
{
    [SpellieElement("debug")]
    public abstract class DebugElement
    {
        public class Builder : DebugElement<Builder>{}
    
        public static Builder New() => new Builder();
    }

    public class DebugElement<TSelf> : SpellieElement<TSelf>
        where TSelf : DebugElement<TSelf>
    {
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            Debug.Log("My text is: " + text.Replace("/y", " "));
            ev.InvokeAll(source, caster, target);
            return NoCastResult.New();
        }

        [SpellieProperty("text")]
        public string text;

        public TSelf WithText(string txt)
        {
            Set("text", txt);
            return (TSelf) this;
        }

        [SpellieEvent("onEvent")]
        public SpellieEvent ev;
    }
}