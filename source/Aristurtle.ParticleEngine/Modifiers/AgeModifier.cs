// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Modifiers;

public class AgeModifier : Modifier
{
    public List<Interpolator> Interpolators { get; set; } = new List<Interpolator>();

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {
            for (var i = 0; i < Interpolators.Count; i++)
            {
                Interpolator interpolator = Interpolators[i];
                interpolator.Update(particle->Age, particle);
            }

            particle++;
        }
    }
}
