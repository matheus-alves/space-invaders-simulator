using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace T1_SE_SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceInvadersSimulator : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // The game textures
        Texture2D titleScreen, shipTexture, playerShotTexture, enemyShotTexture;

        // The player ship
        GameObject2D ship;

        // The game objects
        List<GameObject2D> objects = new List<GameObject2D>();

        // The enemy generator
        EnemyGenerator enemyGenerator;

        // The game over font
        SpriteFont font;

        #endregion

        #region ControlVariables

        // A flag used to define if the game is running or not
        private bool gameStarted = false;

        // A flag to define if the game has ended
        private bool gameEnded = false;

        // The score
        private int score = 0;

        // The speed of the shots
        private int playerShotSpeed = 5;
        private int enemyShotSpeed = 3;

        // The speed of the ship
        private int shipSpeed = 3;

        // The number of lives
        private int playerLives = 3;

        // The time it takes for the player to be able to shoot again
        private const float playerShootTime = 0.5f;
        private float elapsedPlayerShootTime = 0.0f;

        // The threshold of time it takes for the enemies to make a move
        private const int minEnemyMovementTime = 1;
        private const int maxEnemyMovementTime = 2;
        
        // The threshold of time it takes for the enemies to shoot
        private const int minEnemyShootTime = 2;
        private const int maxEnemyShootTime = 5;

        // The threshold of speed values the enemies can have
        private const int minEnemySpeed = 25;
        private const int maxEnemySpeed = 50;

        #endregion

        public SpaceInvadersSimulator()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            enemyGenerator = new EnemyGenerator(
                minEnemyMovementTime, maxEnemyMovementTime, minEnemyShootTime,
                maxEnemyShootTime, minEnemySpeed, maxEnemySpeed, enemyShotSpeed);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads the sprites/textures
            titleScreen = Content.Load<Texture2D>("titleScreen");
            
            shipTexture = Content.Load<Texture2D>("ship");
            playerShotTexture = Content.Load<Texture2D>("playerShot");
            enemyShotTexture = Content.Load<Texture2D>("enemyShot");

            font = Content.Load<SpriteFont>("MessageFont_40");

            enemyGenerator.LoadContent(Content);

            // After loading all the content, sets the game up for playing
            SetupGame();
        }

        /// <summary>
        /// This method is called once to set the initial positions for the game objects
        /// </summary>
        private void SetupGame()
        {
            // Sets the ship initial position
            ship = new PlayerShip(shipTexture, new Vector2(460, 700), playerLives, shipSpeed);
            objects.Add(ship);

            // Creates the first enemy wave
            objects.AddRange(enemyGenerator.GenerateNextEnemyWave());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (!gameStarted)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    gameStarted = true;
                }
            }
            else // the game has started
            {
                ManagePlayer((float)gameTime.ElapsedGameTime.TotalSeconds);

                // Manages enemy behavior
                ManageEnemies((float)gameTime.ElapsedGameTime.TotalSeconds);

                // Checks for collisions
                CheckForCollisions();

                // at the end of each Update loop, 
                // checks for destroyed objects and removes then from the game screen
                RemoveDestroyedObjects();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This method is responsible for handling player input
        /// </summary>
        /// <param name="fromLastUpdateTillNow">How much time has passed since the last update call</param>
        private void ManagePlayer(float fromLastUpdateTillNow)
        {
            elapsedPlayerShootTime += fromLastUpdateTillNow;

            if ((ship as PlayerShip).IsAlive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.B) && elapsedPlayerShootTime > playerShootTime)
                {
                    // Generates a shot from the player ship
                    GameObject2D shot = new Shot(playerShotTexture,
                        (ship as PlayerShip).GetShotPosition(), Direction.UP, playerShotSpeed, Shot.ShotType.Player);

                    objects.Add(shot);

                    elapsedPlayerShootTime = 0.0f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    (ship as PlayerShip).Move(Direction.LEFT);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    (ship as PlayerShip).Move(Direction.RIGHT);
                }
            }
        }

        /// <summary>
        /// This method is responsible for handling the enemies behavior
        /// </summary>
        /// <param name="fromLastUpdateTillNow">How much time has passed since the last update call</param>
        private void ManageEnemies(float fromLastUpdateTillNow)
        {
            // The number of remaining enemies in the current loop
            int enemyCounter = 0;

            // A list of enemy shots to add
            List<GameObject2D> enemyShots = new List<GameObject2D>();

            // Updates all objects
            foreach (GameObject2D obj in objects)
            {
                obj.Update(fromLastUpdateTillNow);

                // generate enemy shots
                if (obj is Enemy)
                {
                    enemyCounter++;

                    if ((obj as Enemy).HasShoot)
                    {
                        GameObject2D shot = new Shot(enemyShotTexture,
                            (obj as Enemy).GetShotPosition(), Direction.DOWN,
                            (obj as Enemy).ShotSpeed, Shot.ShotType.Enemy);

                        enemyShots.Add(shot);

                        (obj as Enemy).HasShoot = false;
                    }
                }
            }

            // adds the enemy shots to the game
            objects.AddRange(enemyShots);

            if (enemyCounter == 0) objects.AddRange(enemyGenerator.GenerateNextEnemyWave());
        }

        /// <summary>
        /// This method checks if any objects have collided and informs them if they did
        /// </summary>
        private void CheckForCollisions()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject2D obj = objects[i];

                for (int j = i + 1; j < objects.Count; j++)
                {
                    GameObject2D other = objects[j];

                    if (obj.BoundingBox.Intersects(other.BoundingBox))
                    {
                        obj.OnCollision(other);
                        other.OnCollision(obj);
                    }
                }
            }
        }

        /// <summary>
        /// This method is responsible for removing any destroyed objects from the screen
        /// </summary>
        private void RemoveDestroyedObjects()
        {
            List<GameObject2D> destroyedOnes = new List<GameObject2D>();

            foreach (GameObject2D obj in objects)
            {
                if (!obj.IsAlive) destroyedOnes.Add(obj);
            }

            foreach (GameObject2D dead in destroyedOnes)
            {
                objects.Remove(dead);

                if (dead is Enemy) score++;

                if (dead is PlayerShip) gameEnded = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

                // If the game is not started yet, then draw only the title screen
                if (!gameStarted)
                {
                    spriteBatch.Draw(titleScreen, titleScreen.Bounds, Color.White);
                }
                else if (!gameEnded) // Draws the game in action
                {
                    foreach (GameObject2D obj in objects)
                    {
                        obj.Draw(spriteBatch);
                    }
                }
                else // game has ended
                {
                    // Draw Hello World
                    string output = "Game Over\nScore = " + score;

                    // Find the center of the string
                    Vector2 FontOrigin = font.MeasureString(output) / 2;
                    // Draw the string
                    spriteBatch.DrawString(font, output, new Vector2(512, 384), Color.White,
                        0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
