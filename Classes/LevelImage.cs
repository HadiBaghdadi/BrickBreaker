using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Classes
{
    public class LevelImage : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D imageTex;
        private Rectangle destRect;
        private Vector2 origin;
        private float scale = 1.0f;
        private Rectangle srcRect;
        private const float maxScale = 3.0F;
        private const float minScale = 0.1F;
        private float scaleChange = 0.03f;
        private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }
        public float Scale { get => scale; set => scale = value; }

        public LevelImage(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.imageTex = tex;
            this.position = position;
            destRect = new Rectangle((int)position.X, (int)position.Y, tex.Width * 2, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.TotalSeconds <= 3)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(imageTex, Position, srcRect, Color.White, 0, origin, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            scale += scaleChange;
            if (scale > maxScale || scale < minScale)
            {
                scaleChange = -scaleChange;
            }
            base.Update(gameTime);
        }
    }
}
