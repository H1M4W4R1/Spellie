using System.Collections.Generic;
using Spellie.SpawnedObjects;
using Spellie.Tools;

namespace Spellie.CastResults
{
    public class ObjectSpawnCastResult : ICastResult
    {
        public List<SpellieSpawnedObject> objects = new List<SpellieSpawnedObject>();

        public static ObjectSpawnCastResult New( List<SpellieSpawnedObject> obj)
         => new ObjectSpawnCastResult() {objects = obj};
        
        
        /// <inheritdoc />
        public void End(IEntity caster)
        {
            // Get all objects spawned this spell
            foreach (var spellieSpawnedObject in objects)
            {
                // If object still exists
                if (spellieSpawnedObject)
                {
                    // Execute end event on the object
                    var ev = spellieSpawnedObject.sourceElement.GetEvent("onEnd");
                    ev?
                        .InvokeAll(spellieSpawnedObject, caster, caster);
                }
            }
        }
    }
}