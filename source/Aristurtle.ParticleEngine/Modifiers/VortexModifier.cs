// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers;

public unsafe class VortexModifier : Modifier
{
    private const float GRAVITY = 100000f;

    public Vector2 Position;
    public float Mass;
    public float MaxSpeed;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {
            var distx = Position.X - particle->Position[0];
            var disty = Position.Y - particle->Position[1];

            var distance2 = (distx * distx) + (disty * disty);
            var distance = (float)Math.Sqrt(distance2);

            var m = (GRAVITY * Mass * particle->Mass) / distance2;
            m = Math.Max(Math.Min(m, MaxSpeed), -MaxSpeed) * elapsedSeconds;

            distx = (distx / distance) * m;
            disty = (disty / distance) * m;

            particle->Velocity[0] += distx;
            particle->Velocity[1] += disty;

            particle++;
        }
    }
}
