using Spellie.CastResults;
using Spellie.Elements.Abstracts;
using Spellie.Tools;

namespace Spellie
{
    /// <summary>
    /// Represents spell
    /// </summary>
    public class Spell
    {

        /// <summary>
        /// Name of spell
        /// </summary>
        public string spellName;
        
        private ISpellieElement _onSpellCasted;

        /// <summary>
        /// Defines spell name
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public Spell WithName(string sName)
        {
            spellName = sName;
            return this;
        }

        /// <summary>
        /// Defines spell cast element (root element that is casted first)
        /// </summary>
        /// <param name="castElement"></param>
        /// <returns></returns>
        public Spell WithCastElement(ISpellieElement castElement)
        {
            _onSpellCasted = castElement;
            return this;
        }
        
        public ICastResult Cast(ICastSource source, IEntity caster, IEntity target)
        {
            return _onSpellCasted?.Cast(caster, source, target);
        }
        
    }
}