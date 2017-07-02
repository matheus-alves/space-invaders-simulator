using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace T1_SE_SpaceInvaders
{
    class Shot : GameObject2D
    {
        public enum ShotType { Player, Enemy };

        // This shot direction
        private Direction direction;

        // Who generated this shot
        private ShotType generatedBy;
        public ShotType WasGeneratedBy { get { return generatedBy; } }

        /// <summary>
        /// Creates a shot object
        /// </summary>
        /// <param name="texture">
        /// The object's texture
        /// </param>
        /// <param name="position">
        /// The 2D position of the object on the game screen
        /// </param>
        /// <param name="direction">
        /// The direction in which the shot will move
        /// </param>
        /// /// <param name="speed">
        /// The speed of the object
        /// </param>
        public Shot(Texture2D texture, Vector2 position, Direction direction, int speed, ShotType generatedBy) :
            base(texture, position, speed)
        {
            this.direction = direction;
            this.speed = speed;
            this.generatedBy = generatedBy;
        }

        public override void Update(float fromLastUpdateTillNow)
        {
            if (direction == Direction.UP)
            {
                this.position.Y -= this.speed;
            }
            else
            {
                this.position.Y += this.speed;
            }

            // Removes shots that "flew" out of the game window
            if ((this.position.Y > 768) || (this.position.Y < 0)) this.Destroy();
        }

        public override void OnCollision(GameObject2D other)
        {
            this.Destroy();
        }
    }
}
