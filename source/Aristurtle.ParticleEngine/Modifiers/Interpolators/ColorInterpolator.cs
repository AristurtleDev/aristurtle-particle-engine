// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/..

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public sealed class ColorInterpolator : Interpolator<Vector3>
{
    public override unsafe void Update(float amount, Particle* particle)
    {
        float h = StartValue.X + (EndValue.X - StartValue.X) * amount;
        float s = StartValue.Y + (EndValue.Y - StartValue.Y) * amount;
        float l = StartValue.Z + (EndValue.Z - StartValue.Z) * amount;

        particle->Color[0] = h;
        particle->Color[1] = s;
        particle->Color[2] = l;
    }
}
