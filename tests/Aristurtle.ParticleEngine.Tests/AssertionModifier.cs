// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;

namespace Aristurtle.ParticleEngine.Tests;

internal sealed class AssertionModifier : Modifier
{
    private readonly Predicate<Particle> _predicate;

    public AssertionModifier(Predicate<Particle> predicate)
    {
        _predicate = predicate;
    }

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {
            Assert.True(_predicate(*particle));
            particle++;
        }
    }
}
