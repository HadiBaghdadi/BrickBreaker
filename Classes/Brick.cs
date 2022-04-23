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
    class Brick : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D pixel;
        GraphicsDevice graphics;

        public bool Active { get; set; } = true;
        int height;
        int width;
        int posX;
        int posY;

        public Brick(Game game, GraphicsDevice graphics, SpriteBatch spriteBatch, int width, int height, int posX, int posY) : base(game)
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
        public bool CheckBallCollision(Ball ball)
        {
            if (Active && ball.posX >= posX && ball.posX <= posX + width && ball.posY <= posY + height && ball.posY >= posY)
            {
                ball.dirX = ball.dirX;
                ball.dirY = -ball.dirY;
                return true;
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
                spriteBatch.Begin();
                spriteBatch.Draw(pixel, new Rectangle(posX, posY, width, height), Color.Green);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
