// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.MonoGame.Sample.ParticleEffects;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
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

            _particleEffects = new ParticleEffect[4];
            _particleEffects[0] = new SmokeParticleEffect();
            _particleEffects[1] = new SparkParticleEffect();
            _particleEffects[2] = new RingParticleEffect();
            _particleEffects[3] = new LoadTestParticleEffect();
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

            for (int i = 0; i < _particleEffects.Length; i++)
            {
                if (_particleEffects[i] is ParticleEffect effect)
                {
                    effect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            for (int i = 0; i < _particleEffects.Length; i++)
            {
                if (_particleEffects[i] is ParticleEffect effect)
                {
                    _particleRenderer.Draw(_spriteBatch, effect);

                }
            }

            _spriteBatch.DrawString(_font, string.Format("Time:        {0}", gameTime.TotalGameTime.TotalSeconds), Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_font, string.Format("Particles:   {0:n0}", _currentParticleEffect.ActiveParticles), new Vector2(0, 18), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Update:      {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 36), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Render:      {0:n4} ({1,8:P2})", gameTime.ElapsedGameTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds / 0.01666666f), new Vector2(0, 52), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
