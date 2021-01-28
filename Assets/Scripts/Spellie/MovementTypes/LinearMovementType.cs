using Spellie.MovementTypes.Attributes;
using Spellie.MovementTypes.Ignore;
using Spellie.SpawnedObjects;
using Spellie.Structure.Attributes;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.MovementTypes
{
    /// <summary>
    /// Defines linear movement type
    /// distance = speed*time
    /// </summary>
    [SpellieMovement("linear")]
    public abstract class LinearMovementType : IMovementType, IIgnoreRotation, IIgnoreScale
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
        
        public class Builder : LinearMovementType<Builder> {}
    }
    
    public class LinearMovementType<TSelf> : MovementType<LinearMovementType<TSelf>>
        where TSelf : LinearMovementType<TSelf>
    {
        /// <summary>
        /// Movement speed
        /// </summary>
        [SpellieProperty("speed")]
        public Vector3 speed = Vector3.zero;

        /// <summary>
        /// Defines speed of movement
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public TSelf WithSpeed(Vector3 velocity)
        {
            Set("speed", velocity);
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
            var dx = dtm * speed.x * obj.right;
            var dy = dtm * speed.y * obj.up;
            var dz = dtm * speed.z * obj.forward;

            // Combine position
            pos += dx + dy + dz;
            
            return pos;
        }

      
        
    }
}