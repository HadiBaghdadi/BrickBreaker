using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Classes
{
    class BrickLevel3 : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D pixel;
        GraphicsDevice graphics;

        public bool Active { get; set; } = true;
        int height;
        int width;
        int posX;
        int posY;
        int x = 3;

        public BrickLevel3(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int width, int height, int posX, int posY) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.width = width;
            this.height = height;
            this.posX = posX;
            this.posY = posY;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public bool CheckBallCollisionLevel3(Ball ball)
        {
            if (x >= 0)
            {
                if (Active && ball.posX >= posX && ball.posX <= posX + width && ball.posY <= posY + height && ball.posY >= posY)
                {
                    ball.dirX = ball.dirX;
                    ball.dirY = -ball.dirY;
                    x -= 1;
                }

                if (x == 0)
                {
                    return true;

                }
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (Active)
            {
                if (x == 1)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(pixel, new Rectangle(posX, posY, width, height), Color.Orange);
                    spriteBatch.End();
                }
                else if (x == 2)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(pixel, new Rectangle(posX, posY, width, height), Color.Red);
                    spriteBatch.End();
                }
                else if (x == 3)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(pixel, new Rectangle(posX, posY, width, height), Color.Green);
                    spriteBatch.End();
                }
            }
            base.Draw(gameTime);
        }
    }
}
