// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Modifiers.Containers;

public class RectangleLoopContainerModifier : Modifier
{
    public int Width;
    public int Height;

    public override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
    {
        while (count-- > 0)
        {
            var left = particle->TriggerPos[0] + Width * -0.5f;
            var right = particle->TriggerPos[0] + Width * 0.5f;
            var top = particle->TriggerPos[1] + Height * -0.5f;
            var bottom = particle->TriggerPos[1] + Height * 0.5f;

            var xPos = particle->Position[0];
            var yPos = particle->Position[1];

            if ((int)particle->Position[0] < left)
            {
                xPos = particle->Position[0] + Width;
            }
            else
            {
                if ((int)particle->Position[0] > right)
                {
                    xPos = particle->Position[0] - Width;
                }
            }

            if ((int)particle->Position[1] < top)
            {
                yPos = particle->Position[1] + Height;
            }
            else
            {
                if ((int)particle->Position[1] > bottom)
                {
                    yPos = particle->Position[1] - Height;
                }
            }

            particle->Position[0] = xPos;
            particle->Position[1] = yPos;

            particle++;
        }
    }
}
