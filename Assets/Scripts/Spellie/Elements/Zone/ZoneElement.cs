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

namespace Spellie.Elements.Zone
{
    /// <summary>
    /// Zone represents spell that creates zone - eg. casts fire on the ground.
    /// </summary>
    [SpellieElement("zone")]
    public abstract class ZoneElement
    {
        public class Builder : ZoneElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class ZoneElement<TSelf> : SpawningSpellElement<ZoneElement<TSelf>>
        where TSelf:ZoneElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            var sObj = new List<SpellieSpawnedObject>();

            var parent = Utility.GetSource(attached, source, caster, target);

            // Spawn object
            var prefab = SpellieSingleton.Instance.GetPrefab(objectId);

            Vector3 calPos;

            // Calculate position
            if (parent != null)
                calPos = parent.GetPosition();
            else
                calPos = target.GetPosition();

            // Spawn zone
            var obj = Utility.Instantiate(prefab, calPos, parent?.GetTransform());

            // Setup data
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
        /// If true, then zone will be attached to ICastSource
        /// </summary>
        [SpellieProperty("attached")]
        public string attached = Utility.Src.NONE;

        
        /// <summary>
        /// Executed when entity enters zone.
        /// </summary>
        [SpellieEvent("onEnter")]
        public SpellieEvent onEnter = new SpellieEvent();
        
        /// <summary>
        /// Executed when entity enters zone.
        /// </summary>
        [SpellieEvent("onExit")]
        public SpellieEvent onExit = new SpellieEvent();
        
        /// <summary>
        /// Executed when entity stays in zone.
        /// </summary>
        [SpellieEvent("onStay")]
        public SpellieEvent onStay = new SpellieEvent();
        
        /// <summary>
        /// Executed when zone is spawned
        /// </summary>
        [SpellieEvent("onSpawned")]
        public SpellieEvent onSpawned = new SpellieEvent();

        public TSelf WithDuration(float lifeSpan)
        {
            Set("duration", lifeSpan);
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

    }
}