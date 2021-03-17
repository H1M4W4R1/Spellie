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
    /// Spawns projectiles in code in front of target.
    /// </summary>
    [SpellieElement("projectile_cone")]
    public abstract class ConeProjectileElement
    {
        public class Builder : ConeProjectileElement<Builder>{}

        public static Builder New() => new Builder();
    }
    
    public class ConeProjectileElement<TSelf> : ProjectileElement<ConeProjectileElement<TSelf>>
    where TSelf : ConeProjectileElement<TSelf>
    {
        /// <inheritdoc />
        public override ICastResult Cast(IEntity caster, ICastSource source, IEntity target)
        {
            var sObj = new List<SpellieSpawnedObject>();

            if(target == null) return new NoCastResult();
            // U1ed to make forward a primary direction for cone
            var vAngle = startAngle + 90;
            var positions = Utility.GetCircleConePositions(amount, angle,
                centered ? vAngle - 0.5f * angle : vAngle, distance, target.GetUpAxis());
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
        /// If true, then angle 0 will mean that middle projectile will be in centre
        /// </summary>
        [SpellieProperty("centered")]
        public bool centered;

        [SpellieProperty("startAngle")]
        public float startAngle;
        
        [SpellieProperty("angle", true)]
        public float angle;
        
        /// <summary>
        /// If true, then projectiles will be centered around look direction
        /// </summary>
        /// <param name="centre"></param>
        /// <returns></returns>
        public TSelf IsCentered(bool centre)
        {
            Set("centered", centre);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Defines angle of the cone, where 360 is full circle.
        /// </summary>
        /// <param name="sAngle"></param>
        /// <returns></returns>
        public TSelf WithAngle(float sAngle)
        {
            Set("angle", sAngle);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Defines start angle of the cone. Allows to move cone around the circle.
        /// Described in degrees.
        /// </summary>
        /// <param name="sAngle"></param>
        /// <returns></returns>
        public TSelf WithStartAngle(float sAngle)
        {
            Set("startAngle", sAngle);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Amount of projectiles in cone
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