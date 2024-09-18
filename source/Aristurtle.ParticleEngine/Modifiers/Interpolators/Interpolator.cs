// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public abstract class Interpolator
{
    public string Name;

    protected Interpolator()
    {
        Name = GetType().Name;
    }

    public abstract unsafe void Update(float amount, Particle* particle);
}
