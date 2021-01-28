using Spellie.MovementTypes.Attributes;
using Spellie.MovementTypes.Ignore;
using Spellie.SpawnedObjects;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.MovementTypes
{
    /// <summary>
    /// Defines linear accelerated movement
    /// s = (startSpeed)*time + (time*time*acceleration)/2
    /// </summary>
    [SpellieMovement("acceleratedLinear")]
    public abstract class AcceleratedLinearMovementType : IMovementType, IIgnoreRotation, IIgnoreScale
    {
        private Builder _instance;

        private Builder Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    _instance = New();
                    return _instance;
                }
            }
        }
        
        /// <inheritdoc />
        public Vector3 EvaluateMovement(SpellieSpawnedObject obj, float dt)
        {
            return Instance.EvaluateMovement(obj, dt);
        }
        
        public static Builder New() => new Builder();
        
        public class Builder : AcceleratedLinearMovementType<Builder> {}
    }
    
    public class AcceleratedLinearMovementType<TSelf> : MovementType<AcceleratedLinearMovementType<TSelf>>
        where TSelf : AcceleratedLinearMovementType<TSelf>
    {
        [SpellieProperty("speed")]
        public Vector3 speed = Vector3.zero;

        [SpellieProperty("acceleration")]
        public Vector3 acceleration = Vector3.zero;

        /// <summary>
        /// Defines initial speed of the movement
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public TSelf WithInitialSpeed(Vector3 velocity)
        {
            Set("speed", velocity);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Defines acceleration of movement
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public TSelf WithAcceleration(Vector3 a)
        {
            Set("acceleration", a);
            return (TSelf) this;
        }

        /// <inheritdoc />
        public override Vector3 EvaluateMovement(SpellieSpawnedObject obj, float dt)
        {
            // Analyse direction
            var mult = Utility.ParseDirection(obj.Direction);

            // Calculate position s = v*t + initial position
            var pos = obj.spawnPos;

            // Calculate time multiplier
            var dtm = mult * dt;

            // Calculate unit positions
            var dx = (0.5f * acceleration.x * dtm*dtm + dtm * speed.x) * obj.right;
            var dy = (0.5f * acceleration.y * dtm*dtm + dtm * speed.y)* obj.up;
            var dz = (0.5f * acceleration.z * dtm*dtm + dtm * speed.z) * obj.forward;

            // Combine position
            pos += dx + dy + dz;
            
            return pos;
        }

      
        
    }
}