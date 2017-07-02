using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace T1_SE_SpaceInvaders
{
    class GameObject2D
    {
        /// <summary>
        /// Returns the BoundingBox of this object, used for collision checking
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        // This object speed
        protected int speed;

        /// <summary>
        /// Returns the speed of this object
        /// </summary>
        public int Speed { get { return speed; } }

        // A flag that indicates if the object is alive
        private bool alive = true;

        /// <summary>
        /// Returns whether the object is alive or not
        /// </summary>
        public bool IsAlive { get { return alive; } }

        // This object texture
        protected Texture2D texture;

        /// <summary>
        /// Returns the Texture of this object, used for drawing the object
        /// </summary>
        public Texture2D Texture { get { return texture; } }

        // The object current position
        protected Vector2 position;

        /// <summary>
        /// Returns the current Position of this object, used for the drawing the object
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// Creates a Game Object
        /// </summary>
        /// <param name="texture">
        /// The object's texture
        /// </param>
        /// <param name="position">
        /// The 2D position of the object on the game screen
        /// </param>
        /// /// <param name="speed">
        /// The speed of the object
        /// </param>
        public GameObject2D(Texture2D texture, Vector2 position, int speed)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
        }

        /// <summary>
        /// Abstract method that will define the behavior of the object.
        /// Must be implemented by each subclass.
        /// </summary>
        public virtual void Update(float fromLastUpdateTillNow) {}

        /// <summary>
        /// Abstract method that will define the behavior of the object upon collision.
        /// Must be implemented by each subclass.
        /// </summary>
        /// <param name="other">The object that has collided with this one</param>
        public virtual void OnCollision(GameObject2D other) {}

        /// <summary>
        /// Draws this object using the given sprite batch
        /// </summary>
        /// <param name="spriteBatch">The game's sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, BoundingBox, Color.White);
        }

        /// <summary>
        /// This method should be called whenever we want the object to be removed from the game
        /// </summary>
        public void Destroy()
        {
            alive = false;
        }
    }
}
