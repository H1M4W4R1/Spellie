using Spellie.Elements.Abstracts;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Effects
{
    public class Effect : ICastSource
    {

        /// <summary>
        /// Source of this effect
        /// </summary>
        public ISpellieElement sourceElement;
        
        /// <summary>
        /// Effect name also known as ID
        /// </summary>
        public string name;
        
        /// <summary>
        /// Effect duration
        /// </summary>
        public float duration;

        /// <summary>
        /// Caster
        /// </summary>
        public IEntity effectTarget;

        /// <summary>
        /// Time left
        /// </summary>
        private float _lifeTime;

        public static Effect New(ISpellieElement element, string name, float duration, IEntity target)
        {
            // Create new effect
            var e = new Effect()
            {
                sourceElement = element,
                name = name,
                duration = duration,
                effectTarget = target
            };
            
            // Apply effect to target
            e.OnEffectApplied();
            return e;
        }

        /// <summary>
        /// Executed when the effect is created
        /// </summary>
        private void OnEffectApplied()
        {
            var ev = sourceElement?.GetEvent("onEffectApplied");
            ev?.InvokeAll(this, effectTarget, effectTarget);
        }

        /// <summary>
        /// Removes effect from entity
        /// </summary>
        public void RemoveEffect()
        {
            var ev = sourceElement?.GetEvent("onEffectRemoved");
            ev?.InvokeAll(this, effectTarget, effectTarget);
            effectTarget.RemoveEffect(this);
        }
        
        /// <summary>
        /// Processes Effect LifeTime Tick
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns>false if effect should wear off</returns>
        public bool OnLifetimeTick(float deltaTime)
        {
            _lifeTime += deltaTime;
            if (duration > 0 && _lifeTime > duration)
            {
                // Effect wears off
                var ev = sourceElement?.GetEvent("onEffectWearOff");
                ev?.InvokeAll(this, effectTarget, effectTarget);
                effectTarget.RemoveEffect(this);
                return false;
            }

            return true;
        }


        /// <inheritdoc />
        public Transform GetTransform()
        {
            return effectTarget.GetTransform();
        }

        public Vector3 GetPosition()
        {
            return effectTarget.GetPosition();
        }
    }
}