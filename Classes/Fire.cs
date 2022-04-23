using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Classes
{
    public class Fire : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 dimension;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay;
        private int delayCounter;
        private const int ROW = 4;
        private const int COL = 4;

        public Vector2 Position { get => position; set => position = value; }

        public Fire(Game game, SpriteBatch spriteBatch,
            Texture2D tex, Vector2 position, int delay) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;
            dimension = new Vector2(tex.Width / COL, tex.Height / ROW);
            hide();
            createFrames();
        }

        private void createFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;
                    Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(r);
                }
            }
        }


        public void start()
        {
            frameIndex = -1;
            this.Enabled = true;
            this.Visible = true;
        }

        public void hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (frameIndex >= 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(tex, Position, frames[frameIndex], Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIndex++;
                if (frameIndex > ROW * COL - 1)
                {
                    frameIndex = -1;
                    hide();
                }
                delayCounter = 0;
            }
            base.Update(gameTime);
        }
    }
}
