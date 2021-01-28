using System.Linq;
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
    public class SpellieSpawnedProjectile : SpellieSpawnedObject
    {
        /// <summary>
        /// Amount of targets pierced by this projectile
        /// </summary>
        public int piercedTargets;
        
        /// <summary>
        /// If true then will cast onHit when collision detected
        /// </summary>
        public bool callEventOnCollision;
        
        /// <summary>
        /// If true then will cast onHit when trigger detected
        /// </summary>
        public bool callEventOnTrigger;

        private void OnTriggerEnter(Collider other)
        {
            
            if (!callEventOnTrigger) return;
            
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                var param = MetaObject.Get<string>("target");
                if (entity.GetEntityTypes().Any(t => t == param) || param == "any")
                {
                    if (SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onHit (Trigger): " + other.name);
                    sourceElement.GetEvent("onHit").InvokeAll(this, caster, entity);
                    Pierce();
                }
            }
            else
            { 
                if (!((MetaObject) sourceElement).Get<bool>("ghost"))
                {
                    Destroy(gameObject);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!callEventOnTrigger) return;
            
            var entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                var param = MetaObject.Get<string>("target");
                if (entity.GetEntityTypes().Any(t => t == param) || param == "any")
                {
                    if (SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onHit (Trigger2D): " + other.name);
                    sourceElement.GetEvent("onHit").InvokeAll(this, caster, entity);
                    Pierce();
                }
            }
            else
            {
                if (!((MetaObject) sourceElement).Get<bool>("ghost"))
                {
                    Destroy(gameObject);
                }
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!callEventOnCollision) return;
            
            var entity = other.collider.GetComponent<IEntity>();
            if (entity != null)
            {
                var param = MetaObject.Get<string>("target");
                if (entity.GetEntityTypes().Any(t => t == param) || param == "any")
                {
                    if (SpellieEvent.logEnabled) Debug.Log("[SpellieEvent] onHit (Collision): " + other.collider.name);
                    sourceElement.GetEvent("onHit").InvokeAll(this, caster, entity);
                    Pierce();
                }
            }
            else
            {
                if (!((MetaObject) sourceElement).Get<bool>("ghost"))
                {
                    Destroy(gameObject);
                }
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!callEventOnCollision) return;

            var entity = other.collider.GetComponent<IEntity>();
            if (entity != null)
            {
                var param = MetaObject.Get<string>("target");
                
                if (entity.GetEntityTypes().Any(t => t == param) || param == "any")
                {
                    if (SpellieEvent.logEnabled)
                        Debug.Log("[SpellieEvent] onHit (Collision2D): " + other.collider.name);
                    sourceElement.GetEvent("onHit").InvokeAll(this, caster, entity);
                    Pierce();
                }
            }
            else
            {
                if (!((MetaObject) sourceElement).Get<bool>("ghost"))
                {
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// Processes piercing behaviour
        /// </summary>
        public void Pierce()
        {
            // Increase amount of pierced targets
            piercedTargets++;
            
            // If pierced amount equal to possible pierces, then destroy
            if (piercedTargets > ((MetaObject) sourceElement).Get<int>("piercing"))
            {
                Destroy(gameObject);
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