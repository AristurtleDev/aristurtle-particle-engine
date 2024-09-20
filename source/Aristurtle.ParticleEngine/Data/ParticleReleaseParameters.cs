// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine;

public class ParticleReleaseParameters
{
    public ParticleInt32Parameter Quantity;
    public ParticleFloatParameter Speed;
    public ParticleColorParameter Color;
    public ParticleFloatParameter Opacity;
    public ParticleFloatParameter Scale;
    public ParticleFloatParameter Rotation;
    public ParticleFloatParameter Mass;

    public ParticleReleaseParameters()
    {
        Quantity = new ParticleInt32Parameter(5, 5);
        Speed = new ParticleFloatParameter(50.0f, 100.0f);
        Color = new ParticleColorParameter(new Vector3(0.0f, 0.0f, 1.0f));
        Opacity = new ParticleFloatParameter(0.0f, 1.0f);
        Scale = new ParticleFloatParameter(1.0f, 1.0f);
        Rotation = new ParticleFloatParameter(-MathF.PI, MathF.PI);
        Mass = new ParticleFloatParameter(1.0f);
    }
}
