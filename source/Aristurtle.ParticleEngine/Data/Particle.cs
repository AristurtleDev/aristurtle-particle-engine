// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Runtime.InteropServices;

namespace Aristurtle.ParticleEngine.Data;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Particle
{
    public float Inception;
    public float Age;
    public fixed float Position[2];
    public fixed float Velocity[2];
    public fixed float Color[3];
    public float Scale;
    public fixed float TriggerPos[2];
    public float Opacity;
    public float Rotation;
    public float Mass;
    public float LayerDepth;

    public static readonly int SizeInBytes = Marshal.SizeOf<Particle>();
}
