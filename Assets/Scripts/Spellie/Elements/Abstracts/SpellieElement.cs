using System;
using System.Collections.Generic;
using System.Linq;
using Spellie.CastResults;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Abstracts
{

    public interface ISpellieElement
    {
        ICastResult Cast(IEntity caster, ICastSource source, IEntity target);

        ISpellieElement AddEventBase(SpellieEvent ev);

        SpellieEvent GetEvent(string name);
    }
    

    public abstract class SpellieElement<TSelf> : MetaObject, ISpellieElement
    where TSelf:SpellieElement<TSelf>
    {
        public abstract ICastResult Cast(IEntity caster, ICastSource source, IEntity target);

        /// <inheritdoc />
        public ISpellieElement AddEventBase(SpellieEvent ev)
        {
            AddEvent(ev);
            return this;
        }

        /// <summary>
        /// Spellie events
        /// </summary>
        private List<SpellieEvent> _events = new List<SpellieEvent>();

        /// <summary>
        /// Get event from spell, null if not defined
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpellieEvent GetEvent(string name) => _events.FirstOrDefault(e => e.name == name);

        /// <summary>
        /// Adds event to list
        /// </summary>
        /// <param name="ev"></param>
        public TSelf AddEvent(SpellieEvent ev)
        {
            _events.Add(ev);
            return (TSelf) this;
        }

        /// <summary>
        /// Removes event from list
        /// </summary>
        /// <param name="ev"></param>
        public TSelf RemoveEvent(SpellieEvent ev)
        {
            _events.Remove(ev);
            return (TSelf) this;
        }

        /// <summary>
        /// Removes ALL events USING name
        /// </summary>
        /// <param name="ev"></param>
        public TSelf RemoveEvent(string ev)
        {
            _events.RemoveAll(e => e.name == ev);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Initialize this SpellElement, remember to call after building
        /// </summary>
        public TSelf Init()
        {
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
            
            // Events
            var eventFields = type.GetFields().Where(f => f.GetCustomAttributes(typeof(SpellieEventAttribute), true).Length > 0);
            foreach (var fieldInfo in eventFields)
            {
                // Get event attribute
                var propertyAttribute = fieldInfo.GetCustomAttributes(typeof(SpellieEventAttribute), true);

                var property = propertyAttribute.First() as SpellieEventAttribute;
                
                // Find event name
                var eventName = property?.Name;
                if (!string.IsNullOrEmpty(eventName))
                {
                    // Set event if can be done
                    var val = GetEvent(eventName);
                    if (val != null) fieldInfo.SetValue(this, val);
                    else if (property.Required) 
                        throw new MissingMemberException("[Spellie] Missing required event: " + eventName);
                }
            }

            return (TSelf) this;
        }

        /// <summary>
        /// Gets root of this element
        /// </summary>
        /// <returns></returns>
        public T As<T>() where T : SpellieElement<TSelf>
        {
            return this as T;
        }
    }
}