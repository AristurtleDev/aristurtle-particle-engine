// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public class HueInterpolator : Interpolator<float>
{
    public override unsafe void Update(float amount, Particle* particle)
    {
        float h = StartValue + (EndValue - StartValue) * amount;
        particle->Color[0] = h;
    }
}
