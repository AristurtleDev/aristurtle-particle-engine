// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers;

public sealed class OpacityFastFadeModifier : Modifier
{
    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {
            particle->Opacity = 1.0f - particle->Age;
            particle++;
        }
    }
}
