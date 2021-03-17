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
    /// Circle projectile element spawns specified amount of projectiles in circle.
    /// Projectiles cannot repeat circles, only one circle can be created.
    /// </summary>
    [SpellieElement("projectile_circle")]
    public abstract class CircleProjectileElement
    {
        public class Builder : CircleProjectileElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class CircleProjectileElement<TSelf> : ProjectileElement<CircleProjectileElement<TSelf>>
    where TSelf : CircleProjectileElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            var sObj = new List<SpellieSpawnedObject>();

            if (target == null) return new NoCastResult();
            
            var positions = Utility.GetCirclePositions(amount, distance, target.GetUpAxis());
            foreach (var projectile in positions)
            {
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
                sObj.Add(obj);

            }

            return ObjectSpawnCastResult.New(sObj);
        }

        /// <summary>
        /// Amount of projectiles spawned
        /// </summary>
        [SpellieProperty("amount")]
        public int amount = 1;
        
        /// <summary>
        /// Amount of projectiles in circle
        /// </summary>
        /// <param name="projectiles"></param>
        /// <returns></returns>
        public TSelf WithAmountOf(int projectiles)
        {
            Set("amount", projectiles);
            return (TSelf) this;
        }

  
    }
}