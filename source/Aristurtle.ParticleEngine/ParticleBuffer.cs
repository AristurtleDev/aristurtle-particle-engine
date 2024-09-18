// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Runtime.InteropServices;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine;

public sealed class ParticleBuffer : IDisposable
{
    private int _tail;

    public readonly IntPtr NativePointer;
    public readonly int Size;
    public int Available => Size - _tail;
    public int Count => _tail;
    public int SizeInBytes => Particle.SizeInBytes * Size;
    public int ActiveSizeInBytes => Particle.SizeInBytes * _tail;
    public bool IsDisposed { get; private set; }

    public ParticleBuffer(int size)
    {
        Size = size;
        NativePointer = Marshal.AllocCoTaskMem(SizeInBytes);
        GC.AddMemoryPressure(SizeInBytes);
    }

    ~ParticleBuffer() => Dispose(false);

    public unsafe int Release(int releaseQuantity, out Particle* first)
    {
        int numToRelease = Math.Min(releaseQuantity, Available);
        int oldTail = _tail;
        _tail += numToRelease;
        first = (Particle*)IntPtr.Add(NativePointer, oldTail * Particle.SizeInBytes);
        return numToRelease;
    }

    public unsafe void Reclaim(int number)
    {
        _tail -= number;
        MemCpy(NativePointer, IntPtr.Add(NativePointer, number * Particle.SizeInBytes), ActiveSizeInBytes);
    }

    public unsafe void CopyTo(IntPtr destination) => MemCpy(destination, NativePointer, ActiveSizeInBytes);

    public unsafe void CopyToReverse(IntPtr destination)
    {
        int offset = 0;

        for (var i = ActiveSizeInBytes - Particle.SizeInBytes; i >= 0; i -= Particle.SizeInBytes)
        {
            MemCpy(IntPtr.Add(destination, offset), IntPtr.Add(NativePointer, i), Particle.SizeInBytes);
            offset += Particle.SizeInBytes;
        }
    }

    private static unsafe void MemCpy(IntPtr dest, IntPtr src, int count)
    {
        Buffer.MemoryCopy(
           source: (void*)src,
           destination: (void*)dest,
           destinationSizeInBytes: count,
           sourceBytesToCopy: count);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed) { return; }

        if (disposing)
        {
            //  No managed resources to free.
        }

        Marshal.FreeHGlobal(NativePointer);
        GC.RemoveMemoryPressure(Particle.SizeInBytes * Size);

        IsDisposed = true;
    }
}
