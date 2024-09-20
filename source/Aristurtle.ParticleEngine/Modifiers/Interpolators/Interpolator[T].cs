// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

namespace Aristurtle.ParticleEngine.Modifiers.Interpolators;

public abstract class Interpolator<T> : Interpolator where T : struct
{
    public T StartValue;
    public T EndValue;
}
