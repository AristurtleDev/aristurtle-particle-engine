// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine;

public sealed unsafe class ParticleEmitter : IDisposable
{
    private float _totalSeconds;
    private float _secondsSinceLastReclaim;
    private float _nextAutoTrigger;

    public ParticleBuffer Buffer;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("capacity")]
    public int Capacity => Buffer.Size;

    [JsonPropertyName("lifespan")]
    public float LifeSpan;

    [JsonPropertyName("offset")]
    public Vector2 Offset;

    [JsonPropertyName("layerDepth")]
    [Display(Name = "Layer Depth", Description = "The layer")]
    public float LayerDepth;

    [JsonPropertyName("autoTrigger")]
    public bool AutoTrigger;

    [JsonPropertyName("autoTriggerFrequency")]
    public float AutoTriggerFrequency;

    [JsonPropertyName("reclaimFrequency")]
    public float ReclaimFrequency;

    [JsonPropertyName("parameters")]
    public ParticleReleaseParameters Parameters { get; set; }

    [JsonPropertyName("strategy")]
    public ModifierExecutionStrategy ModifierExecutionStrategy { get; set; }

    [JsonPropertyName("modifiers")]
    public List<Modifier> Modifiers;

    [JsonPropertyName("profile")]
    public Profile Profile;

    [JsonPropertyName("texture")]
    public string TextureName;

    [JsonPropertyName("sourceRectangle")]
    public Rectangle? SourceRectangle { get; set; }

    [JsonIgnore]
    public bool IsDisposed { get; private set; }

    [JsonIgnore]
    public int ActiveParticles => Buffer.Count;

    public ParticleEmitter() : this(1000) { }

    public ParticleEmitter(int initialCapacity)
    {
        LifeSpan = 1.0f;
        Name = nameof(ParticleEmitter);
        TextureName = string.Empty;
        SourceRectangle = null;
        Buffer = new ParticleBuffer(initialCapacity);
        Profile = Profile.Point();
        Modifiers = new List<Modifier>();
        ModifierExecutionStrategy = ModifierExecutionStrategy.Serial;
        Parameters = new ParticleReleaseParameters();
        ReclaimFrequency = 60.0f;
        Offset = Vector2.Zero;
        LayerDepth = 0.0f;
        AutoTrigger = true;
        AutoTriggerFrequency = 1.0f;
    }


    ~ParticleEmitter() => Dispose(false);

    public void ChangeCapacity(int size)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, typeof(ParticleBuffer));

        if (Capacity == size)
        {
            return;
        }

        if (Buffer is ParticleBuffer oldBuffer)
        {
            oldBuffer.Dispose();
        }

        Buffer = new ParticleBuffer(size);
    }

    public void Update(float elapsedSeconds, Vector2 position = default)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, typeof(ParticleBuffer));

        _totalSeconds += elapsedSeconds;
        _secondsSinceLastReclaim += elapsedSeconds;

        if (AutoTrigger)
        {
            _nextAutoTrigger -= elapsedSeconds;

            if (_nextAutoTrigger <= 0)
            {
                Trigger(position, LayerDepth);
                _nextAutoTrigger = AutoTriggerFrequency;
            }
        }

        if (Buffer.Count == 0)
        {
            return;
        }

        if (_secondsSinceLastReclaim > (1.0f / ReclaimFrequency))
        {
            ReclaimExpiredParticles();
            _secondsSinceLastReclaim -= (1.0f / ReclaimFrequency);
        }

        if (Buffer.Count > 0)
        {
            Particle* particle = (Particle*)Buffer.NativePointer;
            int count = Buffer.Count;

            while (count-- > 0)
            {
                particle->Age = (_totalSeconds - particle->Inception) / LifeSpan;

                particle->Position[0] += particle->Velocity[0] * elapsedSeconds;
                particle->Position[1] += particle->Velocity[1] * elapsedSeconds;

                particle++;
            }

            ModifierExecutionStrategy.ExecuteModifiers(Modifiers, elapsedSeconds, (Particle*)Buffer.NativePointer, Buffer.Count);
        }
    }

    public void Trigger(Vector2 position, float layerDepth = 0)
    {
        int numToRelease = Parameters.Quantity.Value;
        Release(position, numToRelease, layerDepth);
    }

    public void Trigger(LineSegment line, float layerDepth = 0)
    {
        int numToRelease = Parameters.Quantity.Value;
        Vector2 lineVector = line.ToVector2();

        for (int i = 0; i < numToRelease; i++)
        {
            Vector2 offset = lineVector * FastRandom.NextSingle();
            Release(line.Origin + offset, 1, layerDepth);
        }
    }

    private void Release(Vector2 position, int numToRelease, float layerDepth)
    {
        int count = Buffer.Release(numToRelease, out Particle* particle);

        while (count-- > 0)
        {
            Profile.GetOffsetAndHeading((Vector2*)particle->Position, (Vector2*)particle->Velocity);

            particle->Age = 0.0f;
            particle->Inception = _totalSeconds;

            particle->Position[0] += position.X;
            particle->Position[1] += position.Y;

            float speed = Parameters.Speed.Value;

            particle->Velocity[0] *= speed;
            particle->Velocity[1] *= speed;

            Vector3 color = Parameters.Color.Value;
            particle->Color[0] = color.X;
            particle->Color[1] = color.Y;
            particle->Color[2] = color.Z;

            particle->Opacity = Parameters.Opacity.Value;
            particle->Scale = Parameters.Scale.Value;
            particle->Rotation = Parameters.Rotation.Value;
            particle->Mass = Parameters.Mass.Value;
            particle->LayerDepth = layerDepth;

            particle++;
        }
    }

    private void ReclaimExpiredParticles()
    {
        Particle* particle = (Particle*)Buffer.NativePointer;
        int count = Buffer.Count;
        int expired = 0;

        while (count-- > 0)
        {
            if ((_totalSeconds - particle->Inception) < LifeSpan)
            {
                break;
            }

            expired++;
            particle++;
        }

        Buffer.Reclaim(expired);
    }

    public override string ToString() => Name;

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
            //  No managed objects
        }

        Buffer.Dispose();
        IsDisposed = true;
    }
}
