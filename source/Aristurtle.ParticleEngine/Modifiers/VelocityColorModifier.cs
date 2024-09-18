// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers;

public class VelocityColorModifier : Modifier
{
    public Vector3 StationaryColor;
    public Vector3 VelocityColor;
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
                particle->Color[0] = VelocityColor.X;
                particle->Color[1] = VelocityColor.Y;
                particle->Color[2] = VelocityColor.Z;
            }
            else
            {
                Vector3 deltaColor = VelocityColor - StationaryColor;
                float t = MathF.Sqrt(velocitySquared) / VelocityThreshold;

                float h = deltaColor.X * t + StationaryColor.X;
                float s = deltaColor.Y * t + StationaryColor.Y;
                float l = deltaColor.Z * t + StationaryColor.Z;

                particle->Color[0] = ColorUtilities.NormalizeHue(h);
                particle->Color[1] = s;
                particle->Color[2] = l;
            }

            particle++;
        }
    }
}
