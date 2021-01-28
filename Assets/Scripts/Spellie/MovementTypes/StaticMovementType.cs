using Spellie.MovementTypes.Attributes;
using Spellie.MovementTypes.Ignore;
using Spellie.SpawnedObjects;
using UnityEngine;

namespace Spellie.MovementTypes
{
    /// <summary>
    /// Defines static object - object that does not move at all
    /// </summary>
    [SpellieMovement("static")]
    public abstract class StaticMovementType : IMovementType, IIgnoreRotation, IIgnoreScale, IIgnorePosition
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
        
        public class Builder : StaticMovementType<Builder> {}
    }
    
    public class StaticMovementType<TSelf> : MovementType<StaticMovementType<TSelf>>
        where TSelf : StaticMovementType<TSelf>
    {

        /// <inheritdoc />
        public override Vector3 EvaluateMovement(SpellieSpawnedObject obj, float dt)
        {
            return obj.spawnPos;
        }
        
    }
}