// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Containers;

public sealed class RectangleContainerModifier : Modifier
{
    public int Width;
    public int Height;
    public float RestitutionCoefficient = 1.0f;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {

            float left = particle->TriggerPos[0] + Width * -0.5f;
            float right = particle->TriggerPos[0] + Width * 0.5f;
            float top = particle->TriggerPos[1] + Height * -0.5f;
            float bottom = particle->TriggerPos[1] + Height * 0.5f;

            float xPos = particle->Position[0];
            float xVel = particle->Velocity[0];
            float yPos = particle->Position[1];
            float yVel = particle->Velocity[1];

            if ((int)particle->Position[0] < left)
            {
                xPos = left + (left - xPos);
                xVel = -xVel * RestitutionCoefficient;
            }
            else
            {
                if (particle->Position[0] > right)
                {
                    xPos = right - (xPos - right);
                    xVel = -xVel * RestitutionCoefficient;
                }
            }

            if (particle->Position[1] < top)
            {
                yPos = top + (top - yPos);
                yVel = -yVel * RestitutionCoefficient;
            }
            else
            {
                if ((int)particle->Position[1] > bottom)
                {
                    yPos = bottom - (yPos - bottom);
                    yVel = -yVel * RestitutionCoefficient;
                }
            }

            particle->Position[0] = xPos;
            particle->Position[1] = yPos;
            particle->Velocity[0] = xVel;
            particle->Velocity[1] = yVel;

            particle++;
        }
    }
}
