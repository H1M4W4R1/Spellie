using System.Collections.Generic;
using Spellie.Elements.Abstracts;
using Spellie.Structure;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.CastResults
{
    public class ChannelingCastResult : MonoBehaviour, ICastResult, ICastSource
    {
        private float _dt;
        
        /// <summary>
        /// Source casted element
        /// </summary>
        public ISpellieElement sourceElement;

        /// <summary>
        /// Caster that thrown this channel
        /// </summary>
        public IEntity caster;

        /// <summary>
        /// Source that is throwing this channel
        /// </summary>
        public ICastSource source;
        
        /// <summary>
        /// Targets of this channeling spell (usually caster)
        /// </summary>
        public IEntity target;
        
        /// <summary>
        /// If false, then channeling spell will end
        /// </summary>
        public bool stillChanneled = true;
        
        /// <summary>
        /// List of ALL cast results of sub-channeling spells
        /// </summary>
        public List<ICastResult> channeledResults = new List<ICastResult>();

        public static ChannelingCastResult New(ISpellieElement element, IEntity caster, IEntity target, ICastSource src)
        {
            // Create new object that represents this result
            var channelingObject = new GameObject("channelingObject");
            
            // Add ChannelingCastResult to object - it will process channeling events
            var r = channelingObject.AddComponent<ChannelingCastResult>();
            
            // Setup data and initialize object
            r.sourceElement = element;
            r.caster = caster;
            r.target = target;
            r.source = src;
            r.OnChannelStart();
            return r;
        }

        public void Update()
        {
            if (stillChanneled)
            {
                _dt -= Time.deltaTime;
                if (_dt <= 0f)
                {
                    OnChannel();
                    // Set delta to channeling frequency
                    _dt = 1f / ((MetaObject) sourceElement).Get<float>("frequency"); 
                }
            }
            else
            {
                OnChannelEnd();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Executes event on channel start
        /// </summary>
        public void OnChannelStart()
        {
            var ev =  sourceElement.GetEvent("onChannelStarted");
            ev?.InvokeAll(source, caster, target);
        }
        
        /// <summary>
        /// Executes event on channel end
        /// </summary>
        public void OnChannelEnd()
        {
           var ev =  sourceElement.GetEvent("onChannelWearOff");
           ev?.InvokeAll(source, caster, target);

           // End all channeled results
           channeledResults.ForEach(r => r.End(caster));
        }
        
        /// <summary>
        /// Executes event on channel
        /// </summary>
        public void OnChannel()
        {
            var ev =  sourceElement.GetEvent("onChannel");
            var results = ev?.InvokeAll(source, caster, target);
            
            // Add channeled results to list
            if (results != null)
                channeledResults.AddRange(results);
        }

        /// <inheritdoc />
        public Transform GetTransform()
        {
            return transform;
        }

        public Vector3 GetPosition()
        {
            return source.GetPosition();
        }

        /// <summary>
        /// Ends channeling spell
        /// </summary>
        // ReSharper disable once ParameterHidesMember
        public void End(IEntity caster) => stillChanneled = false;

    }
}