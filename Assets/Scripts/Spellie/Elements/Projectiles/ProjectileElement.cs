using System.Collections.Generic;
using Spellie.CastResults;
using Spellie.Config;
using Spellie.Elements.Abstracts;
using Spellie.Elements.Attributes;
using Spellie.SpawnedObjects;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Elements.Projectiles
{
    /// <summary>
    /// Projectile represents... projectile - arrow, fireball, ice shot or anything that flies...
    /// Okay maybe not anything, because it goes more complex when it comes to zones attached to projectiles.
    /// For this case you would need to use attached property.
    /// </summary>
    [SpellieElement("projectile")]
    public abstract class ProjectileElement
    {
        public class Builder : ProjectileElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class ProjectileElement<TSelf> : SpawningSpellElement<ProjectileElement<TSelf>>
        where TSelf:ProjectileElement<TSelf>
    {

        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            var sObj = new List<SpellieSpawnedObject>();

            // Spawn object
            var prefab = SpellieSingleton.Instance.GetPrefab(objectId);

            // Analyse direction
            var multiplier = Utility.ParseDirection(direction);

            // Get direction for casted spell
            var spellCastDirection = multiplier * target.GetLookDirection();

            // Recalculate position
            var diff = distance * spellCastDirection;
            Vector3 calPos;

            // Parent
            var parent = Utility.GetSource(attached, source, caster, target);
            
            // Position parent
            var positionParent = Utility.GetSource(at, source, caster, target);
            
            // Calculate position
            if (positionParent != null)
                calPos = diff + positionParent.GetTransform().position;
            else
                calPos = target.GetPosition() + diff;

            // Spawn new projectile
            var obj = Utility.Instantiate(prefab, calPos, spellCastDirection, parent?.GetTransform());

            // Set source element
            obj.sourceElement = this;
            obj.caster = caster;
            if (source == null)
                source = caster;
            obj.transform.rotation = Quaternion.Euler(source.GetTransform().eulerAngles);
            obj.SetDirections(obj.GetTransform());
            sObj.Add(obj);


            // It's done!
            return ObjectSpawnCastResult.New(sObj);
        }

        /// <summary>
        /// Object to be instantiated
        /// </summary>
        [SpellieProperty("object", true)]
        public string objectId;

        [SpellieProperty("duration")] 
        public float duration;
        
        /// <summary>
        /// If true, then projectile will be attached to ICastSource
        /// </summary>
        [SpellieProperty("attached")]
        public string attached = Utility.Src.NONE;

        /// <summary>
        /// If true, then projectile travels through world objects like walls (non-pierce-able targets)
        /// </summary>
        [SpellieProperty("ghost")]
        public bool ghost = false;

        /// <summary>
        /// Amount of targets to pierce, target must be pierce-able
        /// </summary>
        [SpellieProperty("piercing")]
        public int piercing = 0;

        /// <summary>
        /// Target of this projectile (any, world, enemy, caster, friendly, etc.)
        /// separated by comma
        /// </summary>
        [SpellieProperty("target", true)]
        public string targetType = "any";
        
      
        
        /// <summary>
        /// Direction to send projectile - forward or reverse
        /// forward - projectile is being sent into look direction
        /// reverse - projectile is being sent in reverse (toward caster)
        /// </summary>
        [SpellieProperty("direction", true)]
        public string direction = "forward";
        
        /// <summary>
        /// Distance from location to spawn projectiles
        /// </summary>
        [SpellieProperty("distance", true)]
        public float distance = 0;

        /// <summary>
        /// Executed when projectile hits something (according to target option),
        /// world collision always causes projectile death unless ghost is active
        /// </summary>
        [SpellieEvent("onHit", true)]
        public SpellieEvent onHit = new SpellieEvent();
        
        /// <summary>
        /// Executed when projectile is spawned
        /// </summary>
        [SpellieEvent("onSpawned")]
        public SpellieEvent onSpawned = new SpellieEvent();

        public TSelf WithDuration(float lifeSpan)
        {
            Set("duration", lifeSpan);
            return (TSelf) this;
        }
        
        public TSelf WithDirection(string dir)
        {
            Set("direction", dir);
            return (TSelf) this;
        }
        
        public TSelf WithDistanceFromSource(float dist)
        {
            Set("distance", dist);
            return (TSelf) this;
        }
        
        public TSelf IsGhost(bool @is = true)
        {
            Set("ghost", @is);
            return (TSelf) this;
        }
        
        public TSelf CanPierce(int targets)
        {
            Set("piercing", targets);
            return (TSelf) this;
        }
        

        public TSelf WithPrefab(string prefabId)
        {
            Set("object", prefabId);
            return (TSelf) this;
        }

        public TSelf Build()
        {
            this.Init();
            return (TSelf) this;
        }
        
        public TSelf WithTarget(string target)
        {
            Set("target", target);
            return (TSelf) this;
        }

    }
}