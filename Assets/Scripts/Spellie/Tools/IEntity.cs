using System;
using System.Collections.Generic;
using Spellie.Effects;
using UnityEngine;

namespace Spellie.Tools
{
    /// <summary>
    /// Entity is an world damage-able object like player, enemy, destroyable world objects like barrels etc.
    /// </summary>
    public interface IEntity : ICastSource
    {
        bool ApplyEffect(Effect effect);

        /// <summary>
        /// Gets position of entity, in 3D games remember to place this at level of hand
        /// </summary>
        /// <returns></returns>
        Vector3 GetPosition();
        
        /// <summary>
        /// Gets look direction of entity
        /// in 2D games remember to make y an 0-valued
        /// in HnS games recommended is to use cursor position ray-cast plus base height of hand as y value
        /// </summary>
        /// <returns></returns>
        Vector3 GetLookDirection();
        
        /// <summary>
        /// Damages the entity
        /// </summary>
        void Damage(int amount, IDamageSource source);
        
        /// <summary>
        /// Heals the entity
        /// </summary>
        void Heal(int amount, IHealSource source);
        
        /// <summary>
        /// Should return subset of types available in <see cref="Utility.TargetType"/>
        /// </summary>
        /// <returns></returns>
        List<string> GetEntityTypes();

        /// <summary>
        /// Gets vertical axis of look (look upward against current look direction)
        /// </summary>
        /// <returns></returns>
        Vector3 GetUpAxis();

        Type GetAffinityType();
        void RemoveEffect(string effect);
        void RemoveEffect(Effect effect);
    }

   
}