using Spellie.MovementTypes;
using Spellie.Structure;
using Spellie.Structure.Attributes;
using Spellie.Tools;

namespace Spellie.Elements.Abstracts
{
    public abstract class SpawningSpellElement<TSelf> : SpellieElement<SpawningSpellElement<TSelf>>
        where TSelf:SpawningSpellElement<TSelf>
    {
        
        [SpellieProperty("at")] public string at = Utility.Src.HIT;

        
        [SpellieProperty("use_global")] public bool useGlobal;

        /// <summary>
        /// Executed on spawned objects when channeling ends
        /// targets always aims at caster
        /// </summary>
        [SpellieEvent("onEnd")]
        public SpellieEvent onEnd = new SpellieEvent();
        
                
        /// <summary>
        /// Describes movement of this projectile
        /// </summary>
        [SpellieProperty("movement", true)]
        public IMovementType movement;

        /// <summary>
        /// Defines object movement data
        /// </summary>
        /// <param name="mType"></param>
        /// <returns></returns>
        public TSelf WithMovement(IMovementType mType)
        {
            Set("movement", mType);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Defines object spawn location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public TSelf At(string location)
        {
            Set("at", location);
            return (TSelf) this;
        }
        
        /// <summary>
        /// Will use global position instead local for spawned objects, useful when it comes to movement.
        /// Beware that movement will be global position based instead of local position.
        /// If you want to make it working properly, then you need to create custom movement scripts that
        /// are not deltaTime based. Primary scripts are deltaTime based, so it means that if you change
        /// rotation, the movement will recalculate using new position, so for example movement using local
        /// forward with linear speed 5 when rotated by 90 degrees will also rotate the object around the
        /// spawn point.
        /// This setting is recommended only for making objects that move around global positions (eg. world horizontal
        /// axis)
        /// As of build_2020_06_01_00004 it stores directional vectors in memory, so that problem may be considered solved.
        /// Beware that rotation won't modify position calculations at all and is completely independent. 
        /// </summary>
        /// <param name="use"></param>
        /// <returns></returns>
        public TSelf UseGlobalPositionForSpawnedObjects(bool use)
        {
            Set("use_global", use);
            return (TSelf) this;
        }
    }
}