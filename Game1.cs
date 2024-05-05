using System.Collections.Generic;
using System.Linq;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Space
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch ;
        private SpriteFont _font;
        private Model _ringModel;
        private Model _spaceShipModel;
        private Simulation _spaceSimulation;
        private Box _spaceShip;
        private List<Vector3> _ringPositions;
        private readonly int _currentRingIndex = 0;
        private bool _raceStarted = false;
        private float _raceTime = 0f;
        private Vector3 _spaceShipPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _spaceSimulation = new Simulation();

            _spaceShip = new Box(0.7f, 3, 10);
        

            _ringPositions = new List<Vector3>
            {
                new(10, 0, 0),
                new(20, 0, 0),
                new(30, 0, 0)
            };

            base.Initialize();
        }

      protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);
    _ringModel = Content.Load<Model>("Ring");
    _spaceShipModel = Content.Load<Model>("SpaceShip");
    _font = Content.Load<SpriteFont>("Font");
}



protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    if (!_raceStarted && (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed))
        _raceStarted = true;

    if (_raceStarted)
    {
        _raceTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update physics simulation by a fixed time step
        _spaceSimulation.Timestep((float)gameTime.ElapsedGameTime.TotalSeconds);

        // Check collision between ship and rings
        if (_currentRingIndex < _ringPositions.Count)
        {
            
        }
        else
        {
            _raceStarted = false; // Race completed
        }
    }

    base.Update(gameTime);
}


 protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw rings
            foreach (var position in _ringPositions)
            {
                DrawModel(_ringModel, position);
            }

            if (_raceStarted)
            {
                // Draw spaceship using the stored position
                DrawModel(_spaceShipModel, _spaceShipPosition);

                // Draw race time
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, "Race Time: " + _raceTime.ToString("F2"), new Vector2(10, 10), Color.White);
                _spriteBatch.End();
            }
            else
            {
                // Draw start message
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, "Press SPACE to start the race", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2), Color.White);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private static void DrawModel(Model model, Vector3 position)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects.Cast<BasicEffect>())
                {
                    effect.World = Matrix.CreateTranslation(position);
                   // Assuming you have a camera instance named 'camera'

// Set the view matrix for the effect
effect.View = Camera.ViewMatrix;

// Set the projection matrix for the effect
effect.Projection = Camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }
        }
    }

    internal class Camera
    {
        public static Matrix ViewMatrix { get; internal set; }
        public static Matrix ProjectionMatrix { get; internal set; }
    }
}
