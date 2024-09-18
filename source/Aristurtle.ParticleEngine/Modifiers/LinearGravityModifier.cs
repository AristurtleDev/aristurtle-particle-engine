// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers;

public class LinearGravityModifier : Modifier
{
    public Vector2 Direction;
    public float Strength;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        Vector2 vector = Direction * (Strength * elapsedSeconds);

        while (count-- > 0)
        {
            particle->Velocity[0] = particle->Velocity[0] + vector.X * particle->Mass;
            particle->Velocity[1] = particle->Velocity[1] + vector.Y * particle->Mass;

            particle++;
        }
    }
}
