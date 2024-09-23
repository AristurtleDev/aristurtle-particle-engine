// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;
using Aristurtle.ParticleEngine.Profiles;
using Aristurtle.ParticleEngine.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Aristurtle.ParticleEngine.MonoGame.Sample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ParticleRenderer _particleRenderer;
        private SpriteFont _font;

        private ParticleEffect[] _particleEffects;
        private ParticleEffect _currentParticleEffect;
        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private KeyboardState _previousKeyboard;
        private KeyboardState _currentKeyboard;

        private Stopwatch _updateWatch;
        private Stopwatch _drawWatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            _updateWatch = new Stopwatch();
            _drawWatch = new Stopwatch();
        }

        protected override void Initialize()
        {
            _particleRenderer = new ParticleRenderer();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            //#########################################################################################################
            //                                  Initialize the Particle Renderer
            //#########################################################################################################
            _particleRenderer = new ParticleRenderer();

            //#########################################################################################################
            //                                  Create a particle effect in place
            //#########################################################################################################

            //  Create the particle effect instance
            ParticleEffect smokeParticleEffect = new ParticleEffect("SmokeParticleEffect");

            //  Create a particle emitter for the effect.
            ParticleEmitter smokeEmitter = new ParticleEmitter(2000);
            smokeEmitter.AutoTrigger = false;
            smokeEmitter.LifeSpan = 3.0f;
            smokeEmitter.ReclaimFrequency = 5.0f;
            smokeEmitter.Profile = Profile.Point();
            smokeEmitter.Parameters.Color = new ParticleColorParameter(new System.Numerics.Vector3(0.0f, 0.0f, 0.6f));
            smokeEmitter.Parameters.Opacity = new ParticleFloatParameter(1.0f);
            smokeEmitter.Parameters.Quantity = new ParticleInt32Parameter(5);
            smokeEmitter.Parameters.Speed = new ParticleFloatParameter(0.0f, 100.0f);
            smokeEmitter.Parameters.Scale = new ParticleFloatParameter(32.0f);
            smokeEmitter.Parameters.Rotation = new ParticleFloatParameter(-MathF.PI, MathF.PI);
            smokeEmitter.Parameters.Mass = new ParticleFloatParameter(8.0f, 12.0f);
            smokeEmitter.TextureKey = "Cloud001";

            //  Create modifiers for the particle emitter
            DragModifier dragModifier = new DragModifier();
            dragModifier.Frequency = 10.0f;
            dragModifier.DragCoefficient = 0.47f;
            dragModifier.Density = 0.125f;

            RotationModifier rotationModifier = new RotationModifier();
            rotationModifier.Frequency = 15.0f;
            rotationModifier.RotationRate = 1.0f;

            AgeModifier ageModifier = new AgeModifier();
            ageModifier.Frequency = 60.0f;

            //  Some modifiers, like the AgeModifier can have interpolators
            ScaleInterpolator scaleInterpolator = new ScaleInterpolator();
            scaleInterpolator.StartValue = 0.5f;
            scaleInterpolator.EndValue = 1.0f;

            OpacityInterpolator opacityInterpolator = new OpacityInterpolator();
            opacityInterpolator.StartValue = 0.3f;
            opacityInterpolator.EndValue = 0.0f;

            ageModifier.Interpolators.Add(scaleInterpolator);
            ageModifier.Interpolators.Add(opacityInterpolator);

            //  Add the modifiers to the emitter
            smokeEmitter.Modifiers.Add(dragModifier);
            smokeEmitter.Modifiers.Add(rotationModifier);
            smokeEmitter.Modifiers.Add(ageModifier);

            //  Add the particle emitter to the particle effect
            smokeParticleEffect.Emitters.Add(smokeEmitter);

            //  Add the texture used by the smoke emitter to the particle renderer
            _particleRenderer.Textures.Add(smokeEmitter.TextureKey, Content.Load<Texture2D>(smokeEmitter.TextureKey));


            //#########################################################################################################
            //                                  Create a particle effect by defining it as a separate class
            //#########################################################################################################
            //  Create the instance
            ParticleEffect sparkParticleEffect = new SparkParticleEffect();

            //  Ensure that the texture for each emitter is loaded into the particle renderer
            foreach (ParticleEmitter emitter in sparkParticleEffect.Emitters)
            {
                _particleRenderer.Textures.Add(emitter.TextureKey, Content.Load<Texture2D>(emitter.TextureKey));
            }


            //#########################################################################################################
            //                                  Create a particle effect by loading from file
            //#########################################################################################################
            //  Create the JsonSerializationOptions with the correct converters.  A default is provided for you
            JsonSerializerOptions jsonOptions = ParticleEffectJsonSerializerOptionsProvider.Default;

            //  Load the json
            ParticleEffect ringParticleEffect;
            using (Stream stream = TitleContainer.OpenStream("Content/ring.particles"))
            {
                ringParticleEffect = JsonSerializer.Deserialize<ParticleEffect>(stream, jsonOptions);
            }

            //  Ensure that the textures for each emitter is loaded into the particle renderer
            foreach (ParticleEmitter emitter in ringParticleEffect.Emitters)
            {
                _particleRenderer.Textures.Add(emitter.TextureKey, Content.Load<Texture2D>(emitter.TextureKey));
            }

            //#########################################################################################################
            //                                  You can have more than one emitter
            //#########################################################################################################
            //  Let's create a particle effect that combines both the smoke and the spark emitters created above
            ParticleEffect smokeAndSparkParticleEffect = new ParticleEffect("SmokeAndSpark");
            smokeAndSparkParticleEffect.Emitters.AddRange(smokeParticleEffect.Emitters);
            smokeAndSparkParticleEffect.Emitters.AddRange(sparkParticleEffect.Emitters);

            _particleEffects = new ParticleEffect[4];
            _particleEffects[0] = smokeParticleEffect;
            _particleEffects[1] = sparkParticleEffect;
            _particleEffects[2] = ringParticleEffect;
            _particleEffects[3] = smokeAndSparkParticleEffect;
            _currentParticleEffect = _particleEffects[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            if (_currentKeyboard.IsKeyDown(Keys.D1) && _previousKeyboard.IsKeyUp(Keys.D1))
            {
                _currentParticleEffect = _particleEffects[0];
            }
            else if (_currentKeyboard.IsKeyDown(Keys.D2) && _previousKeyboard.IsKeyUp(Keys.D2))
            {
                _currentParticleEffect = _particleEffects[1];
            }
            else if (_currentKeyboard.IsKeyDown(Keys.D3) && _previousKeyboard.IsKeyUp(Keys.D3))
            {
                _currentParticleEffect = _particleEffects[2];
            }
            else if (_currentKeyboard.IsKeyDown(Keys.D4) && _previousKeyboard.IsKeyUp(Keys.D4))
            {
                _currentParticleEffect = _particleEffects[3];
            }


            if (_currentMouse.LeftButton == ButtonState.Pressed)
            {
                LineSegment line = new LineSegment(_previousMouse.Position.ToVector2().ToNumerics(), _currentMouse.Position.ToVector2().ToNumerics());
                _currentParticleEffect.Trigger(line, 0.0f);
            }

            _updateWatch.Restart();
            for (int i = 0; i < _particleEffects.Length; i++)
            {
                if (_particleEffects[i] is ParticleEffect effect)
                {
                    effect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                }
            }
            _updateWatch.Stop();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _drawWatch.Restart();
            for (int i = 0; i < _particleEffects.Length; i++)
            {
                if (_particleEffects[i] is ParticleEffect effect)
                {
                    _particleRenderer.Draw(_spriteBatch, effect);

                }
            }
            _drawWatch.Stop();


            _spriteBatch.DrawString(_font, "[1] Smoke Effect  [2] Spark Effect  [3] Ring Effect  [4] Smoke And Spark Effect", Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_font, string.Format(CultureInfo.InvariantCulture, "Time:          {0:n2}", gameTime.TotalGameTime.TotalSeconds), new Vector2(0, 25), Color.White);
            _spriteBatch.DrawString(_font, string.Format(CultureInfo.InvariantCulture, "Particles:     {0:n0}", _currentParticleEffect.ActiveParticles), new Vector2(0, 50), Color.White);
            _spriteBatch.DrawString(_font, string.Format(CultureInfo.InvariantCulture, "Update:        {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 75), Color.White);
            _spriteBatch.DrawString(_font, string.Format(CultureInfo.InvariantCulture, "Render:        {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 100), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
