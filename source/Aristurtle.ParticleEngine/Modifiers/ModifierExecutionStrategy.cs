// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using TPL = System.Threading.Tasks.Parallel;

namespace Aristurtle.ParticleEngine.Modifiers;

public abstract class ModifierExecutionStrategy
{
    static public ModifierExecutionStrategy Serial = new SerialModifierExecutionStrategy();
    static public ModifierExecutionStrategy Parallel = new ParallelModifierExecutionStrategy();

    internal abstract unsafe void ExecuteModifiers(IEnumerable<Modifier> modifiers, float elapsedSeconds, Particle* particle, int count);

    internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
    {
        internal override unsafe void ExecuteModifiers(IEnumerable<Modifier> modifiers, float elapsedSeconds, Particle* particle, int count)
        {
            foreach (var modifier in modifiers)
            {
                modifier.InternalUpdate(elapsedSeconds, particle, count);
            }
        }

        public override string ToString() => nameof(Serial);
    }

    internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
    {
        internal override unsafe void ExecuteModifiers(IEnumerable<Modifier> modifiers, float elapsedSeconds, Particle* particle, int count)
        {
            TPL.ForEach(modifiers, modifier => modifier.InternalUpdate(elapsedSeconds, particle, count));
        }

        public override string ToString() => nameof(Parallel);
    }
}

