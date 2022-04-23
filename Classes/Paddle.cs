using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
namespace Game1.Classes
{
    class Paddle : DrawableGameComponent
    {
        int height;
        int width;
        public bool isHit;
        public int posX { get; set; }
        public int posY { get; set; }

        SpriteBatch spriteBatch;
        Texture2D pixel;
        GraphicsDevice graphics;

        public Paddle(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int width, int height) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.width = width;
            this.height = height;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            ResetPaddlePosition();
        }
        public void ResetPaddlePosition()
        {
            posX = graphics.Viewport.Width / 2 - width / 2;
            posY = graphics.Viewport.Height - 20;
        }

        public void CheckPaddleBallCollision(Ball ball)
        {
            //int third = width / 3;
            isHit = false;

            if (ball.posY + ball.size >= posY && ball.posX >= posX && ball.posX + ball.size <= posX + width)
            {
                isHit = true;
                ball.dirY = -ball.dirY;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, new Rectangle(posX, posY, width, height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
