// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/..

using System.Numerics;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Maths;

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public sealed class ColorInterpolator : Interpolator<Vector3>
{
    public override unsafe void Update(float amount, Particle* particle)
    {
        float h = EndValue.X - StartValue.X * amount + StartValue.X;
        float s = EndValue.Y - StartValue.Y * amount + StartValue.Y;
        float l = EndValue.Z - StartValue.Z * amount + StartValue.Z;

        particle->Color[0] = ColorUtilities.NormalizeHue(h);
        particle->Color[1] = s;
        particle->Color[2] = l;
    }
}
