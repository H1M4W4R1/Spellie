using Spellie.Elements.Projectiles;
using Spellie.Structure;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.SpawnedObjects
{
    /// <summary>
    /// Represents world-space spawned projectile, processes collisions, ghosting, piercing etc.
    /// Also represents projectiles in sub-projectile Elements like <see cref="CircleProjectileElement"/>
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class SpellieSpawnedZone : SpellieSpawnedObject
    {
        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onEnter (Trigger): " + other.name);
                sourceElement.GetEvent("onEnter")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onEnter (Trigger): " + other.name);
                sourceElement.GetEvent("onEnter")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onExit (Trigger): " + other.name);
                sourceElement.GetEvent("onExit")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onExit (Trigger): " + other.name);
                sourceElement.GetEvent("onExit")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onStay (Trigger): " + other.name);
                sourceElement.GetEvent("onStay")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                if(SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onStay (Trigger): " + other.name);
                sourceElement.GetEvent("onStay")?.InvokeAll(this, caster, entity);
            }
        }
        
        private void Start()
        {
            // Invoke spawn effect
            var b = sourceElement.GetEvent("onSpawned");
            b?.InvokeAll(this, caster, caster);
        }
    }
}