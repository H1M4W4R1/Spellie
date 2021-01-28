using System;
using System.Collections.Generic;
using System.Linq;
using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Structure
{
    
    public class SpellieEvent : MetaObject
    {
        private bool _initialized;

        public static bool logEnabled = true;
        
        public static SpellieEvent New() => new SpellieEvent();
        
        /// <summary>
        /// Target of this event (any, world, enemy, caster, friendly, etc.)
        /// separated by comma
        /// </summary>
        [SpellieProperty("target")]
        public string targetType = "any";
        
        /// <summary>
        /// Separate with comma
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public SpellieEvent WithTargetType(string types)
        {
            Set("target", types);
            return this;
        }

        /// <summary>
        /// Checks if this event has specified target type
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool HasTargetType(string target)
        {
            if (targetType == "any") return true;
            return targetType.ToLower().Replace(" ", "").Contains(target.ToLower());
        }

        /// <summary>
        /// True if can be executed on entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CanExecuteOnEntity(IEntity entity)
        {
            return entity.GetEntityTypes().Any(HasTargetType);
        }
        
        /// <summary>
        /// Event name
        /// </summary>
        public string name;
        
        private readonly List<ISpellieElement> _onInvoke = new List<ISpellieElement>();

        /// <summary>
        /// Add Element to cast
        /// </summary>
        /// <param name="element"></param>
        public SpellieEvent AddElement(ISpellieElement element)
        {
            _onInvoke.Add(element);
            return this;
        }

        /// <summary>
        /// Remove Element from casting
        /// </summary>
        /// <param name="element"></param>
        public SpellieEvent RemoveElement(ISpellieElement element)
        {
            _onInvoke.Remove(element);
            return this;
        }

        /// <summary>
        /// Cast spell for all targets
        /// </summary>
        /// <param name="source"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        public List<ICastResult> InvokeAll(ICastSource source, IEntity caster, IEntity target)
        {
            var results = new List<ICastResult>();
            if(CanExecuteOnEntity(target))
                _onInvoke.ForEach(element => results.Add(element.Cast(caster, source, target)));
            
            return results;
        }

        /// <summary>
        /// Defines event name eg. onHit
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public SpellieEvent WithName(string newName)
        {
            name = newName;
            return this;
        }

        /// <summary>
        /// Initializes the event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MissingMemberException"></exception>
        public SpellieEvent Init()
        {
            if (_initialized) return this;
            var type = GetType();
            
            // Properties
            var propertyFields = type.GetFields().Where(f => f.GetCustomAttributes(typeof(SpelliePropertyAttribute), true).Length > 0);
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

            _initialized = true;
            return this;
        }
    }
}