using System;
using System.Collections.Generic;
using Spellie.Effects;
using Spellie.Samples.Affinities;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Samples
{
    public class SampleEntityEnemy : MonoBehaviour, IEntity
    {
        public int health = 100;
        public int maxHealth = 100;

        /// <inheritdoc />
        public bool ApplyEffect(Effect effect)
        {
            Debug.Log("Effect Applied: " + effect.name);
            return false;
        }

        /// <inheritdoc />
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        /// <inheritdoc />
        public Vector3 GetLookDirection()
        {
            return transform.forward;
        }

        /// <inheritdoc />
        public void Damage(int amount, IDamageSource source)
        {
            health -= amount;
            if (health <= 0)
                Destroy(gameObject);
        }

        /// <inheritdoc />
        public void Heal(int amount, IHealSource source)
        {
            health += amount;
            if (health > maxHealth)
                health = maxHealth;
        }

        /// <inheritdoc />
        public List<string> GetEntityTypes()
        {
            return new List<string>(){Utility.TargetType.ENEMY, Utility.TargetType.NPC};
        }

        /// <inheritdoc />
        public Vector3 GetUpAxis()
        {
            var transform1 = transform;
            return transform1 ? transform1.up : Vector3.up;
        }

        /// <inheritdoc />
        public Type GetAffinityType()
        {
            return typeof(NoneAffinity);
        }

        /// <inheritdoc />
        public void RemoveEffect(string effect)
        {
            Debug.Log("Removed effect: " + effect);
        }

        /// <inheritdoc />
        public Transform GetTransform()
        {
            return transform;
        }
        
        /// <inheritdoc />
        public void RemoveEffect(Effect effect)
        {
            Debug.Log("Removed effect: " + effect.name);
        }
    }
}