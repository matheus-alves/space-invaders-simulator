using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace T1_SE_SpaceInvaders
{
    class EnemyGenerator
    {
        // The enemy textures
        Texture2D enemyTexture1, enemyTexture2, enemyTexture3, enemyTexture4;
        List<Texture2D> textures;

        // The threshold of time it takes for the enemies to make a move
        private int minEnemyMovementTime;
        private int maxEnemyMovementTime;

        // The threshold of time it takes for the enemies to shoot
        private int minEnemyShootTime;
        private int maxEnemyShootTime;

        // The threshold of speed values the enemies can have
        private int minEnemySpeed;
        private int maxEnemySpeed;

        // The speed of the enemies shots
        private int enemyShotSpeed;

        // A Random number generator
        Random rnd = new Random();

        /// <summary>
        /// A class that is responsible for generating the enemies of the game
        /// </summary>
        /// <param name="minEnemyMovementTime">
        /// The minimum time it takes for the enemies to make a move
        /// </param>
        /// <param name="maxEnemyMovementTime">
        /// The maximun time it takes for the enemies to make a move
        /// </param>
        /// <param name="minEnemyShootTime">
        /// The minimum time it takes for a enemy to shoot
        /// </param>
        /// <param name="maxEnemyShootTime">
        /// The maximum time it takes for a enemy to shoot
        /// </param>
        /// <param name="minEnemySpeed">
        /// The minimum enemy speed
        /// </param>
        /// /// <param name="maxEnemySpeed">
        /// The maximum enemy speed
        /// </param>
        /// <param name="enemyShotSpeed">
        /// The speed of the enemy shots
        /// </param>
        public EnemyGenerator(
            int minEnemyMovementTime, int maxEnemyMovementTime, int minEnemyShootTime, 
            int maxEnemyShootTime, int minEnemySpeed, int maxEnemySpeed, int enemyShotSpeed)
        {
            this.minEnemyMovementTime = minEnemyMovementTime;
            this.maxEnemyMovementTime = maxEnemyMovementTime;
            this.minEnemyShootTime = minEnemyShootTime;
            this.maxEnemyShootTime = maxEnemyShootTime;
            this.minEnemySpeed = minEnemySpeed;
            this.maxEnemySpeed = maxEnemySpeed;
            this.enemyShotSpeed = enemyShotSpeed;
        }

        /// <summary>
        /// This method isolates enemy related content loading
        /// </summary>
        /// <param name="content">
        /// The game's content manager
        /// </param>
        public void LoadContent(ContentManager content)
        {
            enemyTexture1 = content.Load<Texture2D>("enemy1");
            enemyTexture2 = content.Load<Texture2D>("enemy2");
            enemyTexture3 = content.Load<Texture2D>("enemy3");
            enemyTexture4 = content.Load<Texture2D>("enemy4");

            textures = new List<Texture2D>();

            textures.Add(enemyTexture1);
            textures.Add(enemyTexture2);
            textures.Add(enemyTexture3);
            textures.Add(enemyTexture4);
        }

        /// <summary>
        /// This method generates the next wave of enemies
        /// </summary>
        /// <returns> A list with the generated enemies </returns>
        public List<Enemy> GenerateNextEnemyWave()
        {
            List<Enemy> enemies = new List<Enemy>();

            // generate three enemies with each texture
            for (int i = 0; i < textures.Count; i++)
            {
                // randomizes the speed, the movement time and the shoot time
                int speed = rnd.Next(minEnemySpeed, maxEnemySpeed);
                int movementTime = rnd.Next(minEnemyMovementTime, maxEnemyMovementTime);

                for (int j = 0; j < 5; j++)
                {
                    int shootTime = rnd.Next(minEnemyShootTime, maxEnemyShootTime);

                    Enemy e = new Enemy(textures[i], new Vector2(j * 200, i * 85), 
                        speed, movementTime, shootTime, enemyShotSpeed);

                    enemies.Add(e);
                }
            }

            return enemies;
        }
    }
}
