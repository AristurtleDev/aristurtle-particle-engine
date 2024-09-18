// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/..

using System.Numerics;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Containers;

public class CircleContainerModifier : Modifier
{
    public float Radius;
    public bool Inside = true;
    public float RestitutionCoefficient = 1;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        float radiusSq = Radius * Radius;

        while (count-- > 0)
        {
            Vector2 localPos;
            localPos.X = particle->Position[0] - particle->TriggerPos[0];
            localPos.Y = particle->Position[1] - particle->TriggerPos[1];


            float distSq = localPos.LengthSquared();
            Vector2 normal = Vector2.Normalize(localPos);

            if (Inside)
            {
                if (distSq < radiusSq) { continue; }
                SetReflected(distSq, particle, normal);
            }
            else
            {
                if (distSq > radiusSq) { continue; }
                SetReflected(distSq, particle, -normal);
            }

            particle++;
        }
    }

    private unsafe void SetReflected(float distSq, Particle* particle, Vector2 normal)
    {
        float dist = (float)Math.Sqrt(distSq);
        float d = dist - Radius; // how far outside the circle is the particle

        float twoRestDot = 2 * RestitutionCoefficient *
                           Vector2.Dot(new Vector2(particle->Velocity[0], particle->Velocity[1]), normal);

        particle->Velocity[0] -= twoRestDot * normal.X;
        particle->Velocity[1] -= twoRestDot * normal.Y;

        // exact computation requires sqrt or goniometrics
        particle->Position[0] -= normal.X * d;
        particle->Position[1] -= normal.Y * d;
    }
}
