using System;
using System.Linq;
using Spellie.SpawnedObjects;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using UnityEngine;

namespace Spellie.MovementTypes
{
    public abstract class MovementType<TSelf> : MetaObject, IMovementType
    where TSelf: MovementType<TSelf>
    {
        /// <inheritdoc />
        public abstract Vector3 EvaluateMovement(SpellieSpawnedObject obj, float dt);
        
        public TSelf Build()
        {
            var type = GetType();
            
            // Properties
            var propertyFields = type.GetFields().Where(f => 
                f.GetCustomAttributes(typeof(SpelliePropertyAttribute), true).Length > 0);
            foreach (var fieldInfo in propertyFields)
            {
                // Get property attribute
                var propertyAttribute = fieldInfo.GetCustomAttributes(typeof(SpelliePropertyAttribute), true);

                var property = propertyAttribute.First() as SpelliePropertyAttribute;
                
                // Find property name
                var propertyName = property?.Name;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    // Set property if can be done
                    var val = Get(propertyName);
                    if (val != null) fieldInfo.SetValue(this, val);
                    else if (property.Required) 
                        throw new MissingMemberException("[Spellie] Missing required property: " + propertyName);
                }
            }

            return (TSelf) this;
        }
    }
}