using Spellie.Elements.Abstracts;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.CastResults
{
    public class SwitchCastResult : ICastResult, ICastSource
    {
        /// <summary>
        /// Source element of cast result
        /// </summary>
        public ISpellieElement sourceElement;

        /// <summary>
        /// Targets of cast result
        /// </summary>
        public IEntity target;

        /// <summary>
        /// Caster
        /// </summary>
        public IEntity caster;

        public static SwitchCastResult New(ISpellieElement src, IEntity caster, IEntity target)
        {
            var e = new SwitchCastResult {sourceElement = src, caster = caster, target = target};
            e.Enable();
            return e;
        }

        
        /// <summary>
        /// Executed when the switch begins (eg. enables aura)
        /// </summary>
        public void Enable()
        {
            sourceElement.GetEvent("onBegin")?.InvokeAll(this, caster, target);
            sourceElement.GetEvent("onSwitch")?.InvokeAll(this, caster, target);
        }
        
        /// <summary>
        /// Ends this Cast Result through ending entire switchable spell (eg. disables aura)
        /// </summary>
        public void Disable()
        {
            End(caster);
        }
        
        /// <inheritdoc />
        public void End(IEntity fCaster)
        {
            sourceElement.GetEvent("onSwitch")?.InvokeAll(this, fCaster, target);
            sourceElement.GetEvent("onEnd")?.InvokeAll(this, fCaster, target);
        }

        /// <inheritdoc />
        public Transform GetTransform()
        {
            return caster.GetTransform();
        }

        public Vector3 GetPosition()
        {
            return caster.GetPosition();
        }
    }
}