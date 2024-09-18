// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Modifiers;

public class VelocityModifier : Modifier
{
    public List<Interpolator> Interpolators { get; set; } = new List<Interpolator>();
    public float VelocityThreshold;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        float velocityThreshold2 = VelocityThreshold * VelocityThreshold;

        while (count-- > 0)
        {

            float velocitySquared = particle->Velocity[0] * particle->Velocity[0] +
                                    particle->Velocity[1] * particle->Velocity[1];

            if (velocitySquared >= velocityThreshold2)
            {
                for (int i = 0; i < Interpolators.Count; i++)
                {
                    Interpolator interpolator = Interpolators[i];
                    interpolator.Update(1, particle);
                }
            }
            else
            {
                float t = (float)Math.Sqrt(velocitySquared) / VelocityThreshold;

                for (int i = 0; i < Interpolators.Count; i++)
                {
                    Interpolator interpolator = Interpolators[i];
                    interpolator.Update(t, particle);
                }
            }

            particle++;
        }
    }
}
