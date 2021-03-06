﻿using System.Collections;
using System.Collections.Generic;
using Spellie.CastResults;
using Spellie.Config;
using Spellie.Elements.Attributes;
using Spellie.SpawnedObjects;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Elements.Projectiles
{
    /// <summary>
    /// Circle projectile element spawns specified amount of projectiles randomly in circle.
    /// </summary>
    [SpellieElement("random_projectile_circle")]
    public abstract class RandomCircleProjectileElement
    {
        public class Builder : RandomCircleProjectileElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class RandomCircleProjectileElement<TSelf> : ProjectileElement<RandomCircleProjectileElement<TSelf>>
    where TSelf : RandomCircleProjectileElement<TSelf>
    {

        public IEnumerator CastImpl(IEntity caster, ICastSource source, IEntity target, ObjectSpawnCastResult ocr)
        {
            if (target == null) goto end;
            for (var a = 0; a < amount; a++)
            {
                // Randomize projectile position
                var projectile = Utility.RandomCirclePosition(distance, target.GetUpAxis());

                var lookDirection = projectile - Vector3.zero;
                lookDirection /= lookDirection.magnitude;

                // Projectile rotation to set
                var rotation = Quaternion.LookRotation(lookDirection);

                // Spawn object
                var prefab = SpellieSingleton.Instance.GetPrefab(objectId);

                // Analyse direction
                var multiplier = Utility.ParseDirection(direction);

                // Parent
                var parent = Utility.GetSource(attached, source, caster, target);

                // Position parent
                var positionParent = Utility.GetSource(at, source, caster, target);
                
                // Calculate position
                Vector3 cPos;
                cPos = positionParent?.GetPosition() ?? target.GetPosition();


                // Spawn new projectile
                var obj = Utility.Instantiate(prefab, projectile + cPos, lookDirection * multiplier,
                    parent?.GetTransform());

                // Set source element
                obj.sourceElement = this;
                obj.caster = caster;
                var transform = obj.transform;
                transform.rotation = rotation;
                obj.SetDirections(transform);
                ocr.objects.Add(obj);

                if (delay > 0)
                    yield return new WaitForSeconds(delay);

            }
            end: ;
        }

        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            var result = ObjectSpawnCastResult.New(new List<SpellieSpawnedObject>());
            SpellieSingleton.Instance.StartCoroutine(CastImpl(caster, source, target, result));
            return result;
        }

        /// <summary>
        /// Amount of projectiles spawned
        /// </summary>
        [SpellieProperty("amount")]
        public int amount = 1;
        
        /// <summary>
        /// Amount of projectiles spawned
        /// </summary>
        [SpellieProperty("delay")]
        public float delay;
        
        /// <summary>
        /// Amount of projectiles to spawn
        /// </summary>
        /// <param name="projectiles"></param>
        /// <returns></returns>
        public TSelf WithAmountOf(int projectiles)
        {
            Set("amount", projectiles);
            return (TSelf) this;
        }
        
        
        /// <summary>
        /// Delay between random projectile spawns
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TSelf WithDelay(float time)
        {
            Set("delay", time);
            return (TSelf) this;
        }

    }
}