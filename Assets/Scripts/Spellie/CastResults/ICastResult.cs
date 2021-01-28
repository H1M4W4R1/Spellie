using Spellie.Tools;

namespace Spellie.CastResults
{
    public interface ICastResult
    {
        void End(IEntity caster);
    }
    
    public class NoCastResult : ICastResult
    {
        public static NoCastResult New() => new NoCastResult();

        /// <inheritdoc />
        public void End(IEntity caster)
        {
            // Nothing
        }
    }
}
