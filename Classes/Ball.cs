using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Classes
{
    class Ball : DrawableGameComponent
    {
        public bool run { get; set; } = false;
        public int size;
        public int dirX { get; set; }
        public int dirY { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        int speed = 5;
        public int lives;

        SpriteBatch spriteBatch;
        Texture2D pixel;
        GraphicsDevice graphics;
        Random rnd;

        public Ball(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int size) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.size = size;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            rnd = new Random();

            ResetBallPosition();
            ResetBallDirection();
        }
        public void ResetBallDirection()
        {
            do
            {
                dirX = rnd.Next(-1, 1);
            } while (dirX == 0);
            do
            {
                dirY = rnd.Next(-1, 1);
            } while (dirY == 0);
        }

        public void ResetBallPosition()
        {
            posX = graphics.Viewport.Width / 2 - size / 2;
            posY = graphics.Viewport.Height - graphics.Viewport.Height / 3;
        }

        public override void Update(GameTime gameTime)
        {
            if (run)
            {
                posX += dirX * speed;
                posY += dirY * speed;
                CheckWallCollision();
            }
            base.Update(gameTime);
        }

        public void CheckWallCollision()
        {
            if (posX <= graphics.Viewport.X || posX + size >= graphics.Viewport.Width)
            {
                dirX *= -1;
            }
            if (posY <= 0)
            {
                dirY *= -1;
            }
            if (posY >= graphics.Viewport.Height - 20)
            {
                run = false;
                lives -= 1;

                ResetBallDirection();
                ResetBallPosition();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, new Rectangle(posX, posY, size, size), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
