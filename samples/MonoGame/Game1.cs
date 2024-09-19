// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;
using Aristurtle.ParticleEngine.Profiles;
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
        private ParticleEffect _smokeEffect;
        private ParticleEffect _sparkEffect;
        private ParticleEffect _ringEffect;
        private ParticleEffect _loadTestEffect;
        private SpriteFont _font;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            Texture2D cloudTexture = Content.Load<Texture2D>("Cloud001");
            Texture2D particleTexture = Content.Load<Texture2D>("Particle");
            Texture2D pixelTexture = Content.Load<Texture2D>("Pixel");
            Texture2D ringTexture = Content.Load<Texture2D>("Ring001");

            _particleRenderer.Textures.Add(cloudTexture.Name, cloudTexture);
            _particleRenderer.Textures.Add(particleTexture.Name, particleTexture);
            _particleRenderer.Textures.Add(pixelTexture.Name, pixelTexture);
            _particleRenderer.Textures.Add(ringTexture.Name, ringTexture);

            _smokeEffect = new ParticleEffect("Smoke")
            {
                Position = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight).ToNumerics() * 0.5f,
                Emitters = new List<ParticleEmitter>()
                {
                    new ParticleEmitter(2000)
                    {
                        LifeSpan = 3.0f,
                        Profile = Profile.Point(),
                        AutoTrigger = false,
                        Parameters = new ParticleReleaseParameters()
                        {
                            Color = new ParticleColorParameter(new Vector3(0.0f, 0.0f, 0.6f).ToNumerics()),
                            Opacity = new ParticleFloatParameter(1.0f),
                            Quantity = new ParticleInt32Parameter(5),
                            Speed = new ParticleFloatParameter(0.0f, 100.0f),
                            Scale = new ParticleFloatParameter(32.0f),
                            Rotation = new ParticleFloatParameter(-MathF.PI, MathF.PI),
                            Mass = new ParticleFloatParameter(8.0f, 12.0f)
                        },
                        ReclaimFrequency = 5.0f,
                        TextureName = "Cloud001",
                        Modifiers = new List<Modifiers.Modifier>()
                        {
                            new DragModifier()
                            {
                                Frequency = 10.0f,
                                DragCoefficient = 0.47f,
                                Density = 0.125f
                            },
                            new RotationModifier()
                            {
                                Frequency = 15.0f,
                                RotationRate = 1.0f
                            },
                            new AgeModifier()
                            {
                                Frequency = 60.0f,
                                Interpolators = new List<Interpolator>()
                                {
                                    new ScaleInterpolator()
                                    {
                                        StartValue = 0.5f,
                                        EndValue = 1.0f
                                    },
                                    new OpacityInterpolator()
                                    {
                                        StartValue = 0.3f,
                                        EndValue = 0.0f
                                    }
                                }
                            }
                        }
                        
                    }
                }
            };
        }

        private MouseState _previousMouse;
        private MouseState _currentMouse;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            if(_currentMouse.LeftButton == ButtonState.Pressed)
            {
                LineSegment line = new LineSegment(_previousMouse.Position.ToVector2().ToNumerics(), _currentMouse.Position.ToVector2().ToNumerics());
                _smokeEffect.Trigger(line, 0.0f);
            }

            _smokeEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _particleRenderer.Draw(_spriteBatch, _smokeEffect);

            _spriteBatch.DrawString(_font, string.Format("Time:        {0}", gameTime.TotalGameTime.TotalSeconds), Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_font, string.Format("Particles:   {0:n0}", _smokeEffect.ActiveParticles), new Vector2(0, 18), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Update:      {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 36), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Render:      {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 52), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
