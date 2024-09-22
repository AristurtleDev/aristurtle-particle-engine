// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Runtime.InteropServices;
using Aristurtle.ParticleEngine.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aristurtle.ParticleEngine.MonoGame.Sample;

public sealed class ParticleRenderer : IDisposable
{
    public Dictionary<string, Texture2D> Textures;
    public bool IsDisposed { get; private set; }

    public ParticleRenderer()
    {
        Textures = new Dictionary<string, Texture2D>();
    }

    ~ParticleRenderer() => Dispose(false);

    public void Draw(SpriteBatch spriteBatch, ParticleEffect particleEffect)
    {
        ArgumentNullException.ThrowIfNull(particleEffect);
        ObjectDisposedException.ThrowIf(particleEffect.IsDisposed, particleEffect);

        ReadOnlySpan<ParticleEmitter> emitters = CollectionsMarshal.AsSpan(particleEffect.Emitters);
        for (int i = 0; i < emitters.Length; i++)
        {
            Draw(spriteBatch, emitters[i]);
        }
    }

    public void Draw(SpriteBatch spriteBatch, ParticleEmitter emitter)
    {
        ArgumentNullException.ThrowIfNull(emitter);
        UnsafeDraw(spriteBatch, emitter);
    }

    private unsafe void UnsafeDraw(SpriteBatch spriteBatch, ParticleEmitter emitter)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);

        if (string.IsNullOrEmpty(emitter.TextureName))
        {
            return;
        }

        if (!Textures.TryGetValue(emitter.TextureName, out Texture2D texture))
        {
            throw new InvalidOperationException($"{nameof(ParticleRenderer)} does not contain a texture named '{emitter.TextureName}'.  Did you forget to add it?");
        }

        Rectangle sourceRect;
        sourceRect.X = emitter.SourceRectangle?.X ?? texture.Bounds.X;
        sourceRect.Y = emitter.SourceRectangle?.Y ?? texture.Bounds.Y;
        sourceRect.Width = emitter.SourceRectangle?.Width ?? texture.Bounds.Width;
        sourceRect.Height = emitter.SourceRectangle?.Height ?? texture.Bounds.Height;

        Vector2 origin = sourceRect.Center.ToVector2();
        Particle* particle = (Particle*)emitter.Buffer.NativePointer;
        int count = emitter.ActiveParticles;

        IntPtr buffer = Marshal.AllocHGlobal(emitter.Buffer.ActiveSizeInBytes);


        try
        {
            if(emitter.RenderingOrder == ParticleRenderingOrder.FrontToBack)
            {
                emitter.Buffer.CopyToReverse(buffer);
            }
            else
            {
                emitter.Buffer.CopyTo(buffer);
            }

            Particle* particle = (Particle*)buffer;

        while (count-- > 0)
        {
            ColorUtilities.HslToRgb(particle->Color[0], particle->Color[1], particle->Color[2], out int r, out int g, out int b);
            Color color = new Color(r, g, b);

            if (spriteBatch.GraphicsDevice.BlendState == BlendState.AlphaBlend)
            {
                color *= particle->Opacity;
            }
            else
            {
                color.A = (byte)(particle->Opacity * 255);
            }

            Vector2 position = new Vector2(particle->Position[0], particle->Position[1]);
            Vector2 scale = new Vector2(particle->Scale);
            color.A = (byte)MathHelper.Clamp(particle->Opacity * 255, 0, 255);
            float rotation = particle->Rotation;
            float layerDepth = particle->LayerDepth;

            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, layerDepth);

            particle++;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            Textures.Clear();
        }

        IsDisposed = true;
    }
}
