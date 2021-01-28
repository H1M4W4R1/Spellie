using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Elements.Other
{
    /// <summary>
    /// Responsible for destroying source of the SpellElement eg. projectile, zone etc.
    /// Beware not to destroy your player :)
    /// </summary>
    [SpellieElement("destroy_source")]
    public abstract class DestroySourceElement
    {
        public class Builder : DestroySourceElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class DestroySourceElement<TSelf> : SpellieElement<DestroySourceElement<TSelf>>
        where TSelf:DestroySourceElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            // Source can be destroyed only if can be found.
            if (source != null) 
                Object.Destroy(source.GetTransform().gameObject);
            
            // Damage is not based on targets, because primary skills are based on targets,
            // Effects are only applied onto processed data
            return NoCastResult.New();
        }
        
    }
}