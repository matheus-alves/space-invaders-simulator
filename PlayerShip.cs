﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace T1_SE_SpaceInvaders
{
    class PlayerShip : GameObject2D
    {
        // The player number of lives
        private int lives;

        /// <summary>
        /// Returns the number of remaining lives
        /// </summary>
        public int Lives { get { return lives; } }

        /// <summary>
        /// Creates the Player Ship object
        /// </summary>
        /// <param name="texture">
        /// The object's texture
        /// </param>
        /// <param name="position">
        /// The 2D position of the object on the game screen
        /// </param>
        /// <param name="lives">
        /// The number of lives the player will have
        /// </param>
        /// /// /// <param name="speed">
        /// The speed of the object
        /// </param>
        public PlayerShip(Texture2D texture, Vector2 position, int lives, int speed) :
            base(texture, position, speed)
        {
            this.lives = lives;
        }

        public override void Update(float fromLastUpdateTillNow)
        {
            if (lives == 0) this.Destroy();
        }

        public override void OnCollision(GameObject2D other)
        {
            if (other is Shot)
            {
                if ((other as Shot).WasGeneratedBy == Shot.ShotType.Enemy)
                {
                    // TODO play sound
                    lives--;
                }
            }
        }

        /// <summary>
        /// Helper method used to return the position of the shots generated by this ship
        /// </summary>
        /// <returns></returns>
        public Vector2 GetShotPosition()
        {
            return new Vector2(this.Position.X + 44, this.Position.Y - this.Texture.Height - 1);
        }

        /// <summary>
        /// This method is responsible for handling the movements of the ship
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Direction direction)
        {
            if (direction == Direction.LEFT)
            {
                if (this.position.X - this.speed > 0) this.position.X -= this.speed;
            }
            else
            {
                if (this.position.X + this.speed + this.texture.Width < 1024) this.position.X += this.speed;
            }
        }
    }
}