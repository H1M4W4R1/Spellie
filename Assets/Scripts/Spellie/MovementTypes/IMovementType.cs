using Spellie.SpawnedObjects;
using UnityEngine;

namespace Spellie.MovementTypes
{
    public interface IMovementType
    {
        /// <summary>
        /// Evaluate position of object
        /// Processed on non-main thread, so you cannot use Unity functions in here.
        /// You can always cache Unity properties before accessing them.
        /// </summary>
        /// <param name="obj">Object to evaluate</param>
        /// <param name="dt">Time to evaluate for</param>
        /// <returns></returns>
        Vector3 EvaluateMovement(SpellieSpawnedObject obj, float dt);
    }
}