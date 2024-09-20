// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public class VelocityInterpolator : Interpolator<Vector2>
{
    public override unsafe void Update(float amount, Particle* particle)
    {
        particle->Velocity[0] = (EndValue.X - StartValue.X) * amount + StartValue.X;
        particle->Velocity[1] = (EndValue.Y - StartValue.Y) * amount + StartValue.Y;
    }
}
