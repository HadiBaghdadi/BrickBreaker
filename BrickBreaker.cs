using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.Classes;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Linq;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// version1.0;
    public class BrickBreaker : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ball ball;
        Paddle paddle;
        List<Brick> lstBricks;
        List<BrickLevel2> lstBrickLevel2;
        List<BrickLevel3> lstBrickLevel3;
        Texture2D pixel;
        Texture2D background;
        Texture2D helpScreen;
        Texture2D aboutScreen;
        SpriteFont HUD;
        Vector2 posLives;
        SoundEffect effect;
        SoundEffect paddleBounce;
        Song song;
        Fire fire;
        Random rndX = new Random();
        Random rndY = new Random();
        int rndPosX;
        int rndPosY;
        private LevelImage L2;
        private LevelImage L3;

        public enum GameState
        {
            MainMenu,
            Playing,
            About,
            Help,
            Level2,
            Level3
        };

        GameState CurrentGameState = GameState.MainMenu;
        PlayButton btnPlay;
        AboutButton btnAbout;
        HelpButton btnHelp;
        int ballSize = 10;
        int paddleWidth = 50;
        int paddleHeight = 10;
        int brickHeight = 20;
        int brickWidth = 50;
        int brickRows = 6;
        int score;
        int score2;
        int score3;
        int gameWidth = 500;
        int gameHeight = 600;

        public BrickBreaker()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            score = 0;
            posLives.X = 437;
            posLives.Y = 10;
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            btnPlay = new PlayButton(Content.Load<Texture2D>("PlayButton"), graphics.GraphicsDevice);
            btnAbout = new AboutButton(Content.Load<Texture2D>("AboutButton"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(150, 200));
            btnAbout.setPosition(new Vector2(150, 300));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Texture2D(GraphicsDevice, 1, 1);
            background = Content.Load<Texture2D>("background");
            Texture2D fireTex = Content.Load<Texture2D>("fireSpriteSheet");
            fire = new Fire(this, spriteBatch, fireTex, Vector2.Zero, 3);
            this.Components.Add(fire);
            Texture2D level2 = this.Content.Load<Texture2D>("LVL2");
            Vector2 levelPos = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            L2 = new LevelImage(this, spriteBatch, level2, levelPos);
            Texture2D level3 = this.Content.Load<Texture2D>("LVL3");
            L3 = new LevelImage(this, spriteBatch, level3, levelPos);

            effect = Content.Load<SoundEffect>("bruh");
            paddleBounce = Content.Load<SoundEffect>("paddleBounce");
            song = Content.Load<Song>("song");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.03f;

            ball = new Ball(this, GraphicsDevice, spriteBatch, ballSize);

            paddle = new Paddle(this, GraphicsDevice, spriteBatch, paddleWidth, paddleHeight);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            aboutScreen = new Texture2D(GraphicsDevice, gameWidth, gameHeight);
            lstBricks = new List<Brick>();
            lstBrickLevel2 = new List<BrickLevel2>();
            lstBrickLevel3 = new List<BrickLevel3>();
            HUD = Content.Load<SpriteFont>("HUD");
            aboutScreen = Content.Load<Texture2D>("AboutScreen");
            helpScreen = Content.Load<Texture2D>("HelpScreen");

            btnHelp = new HelpButton(Content.Load<Texture2D>("HelpButton"), graphics.GraphicsDevice);
            btnHelp.setPosition(new Vector2(150, 400));

            ball.lives = 3;

            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();
        }
        public void LoadObjects()
        {
            Components.Add(paddle);
            Components.Add(ball);
            CreateBricks();
        }
        public void LoadObjectsLevel2()
        {
            CreateBricksLevel2();
        }
        public void LoadObjectsLevel3()
        {
            CreateBricksLevel3();
        }

        public void RemoveObject()
        {
            Components.Remove(paddle);
            Components.Remove(ball);
            lstBricks.Clear();
            lstBrickLevel2.Clear();
            lstBrickLevel3.Clear();
        }
        public void CreateBricks()
        {
            //Generate brick columns, then rows, then add bricks to each slot on grid
            for (int i = 0; i < gameWidth / brickWidth; i++)
            {
                for (int j = 2; j < brickRows + 2; j++)
                {
                    lstBricks.Add(new Brick(this, GraphicsDevice, spriteBatch, brickWidth, brickHeight, i * brickWidth + i, j * brickHeight + j));
                }
            }

            foreach (var brick in lstBricks)
            {
                Components.Add(brick);
            }
        }
        public void CreateBricksLevel2()
        {
            ball.lives = 3;
            for (int i = 0; i < gameWidth / brickWidth; i++)
            {
                for (int j = 2; j < (brickRows + 1) + 2; j++)
                {
                    lstBrickLevel2.Add(new BrickLevel2(this, GraphicsDevice, spriteBatch, brickWidth, brickHeight, i * brickWidth + i, j * brickHeight + j));
                }
            }

            foreach (var bricks in lstBrickLevel2)
            {
                Components.Add(bricks);
            }
        }
        public void CreateBricksLevel3()
        {
            ball.lives = 3;
            for (int i = 0; i < gameWidth / brickWidth; i++)
            {
                for (int j = 2; j < (brickRows + 2) + 2; j++)
                {
                    lstBrickLevel3.Add(new BrickLevel3(this, GraphicsDevice, spriteBatch, brickWidth, brickHeight, i * brickWidth + i, j * brickHeight + j));
                }
            }

            foreach (var bricks in lstBrickLevel3)
            {
                Components.Add(bricks);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            //Switch screens and what should be updated / displayed based on what the GameState is set to
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    RemoveObject();
                    ball.lives = 3;
                    if (btnPlay.isClicked == true)
                    {
                        LoadObjects();
                        CurrentGameState = GameState.Playing;
                        score = 0;
                    }
                    btnPlay.Update(mouse);

                    if (btnAbout.isClicked == true) CurrentGameState = GameState.About;
                    btnAbout.Update(mouse);

                    if (btnHelp.isClicked == true) CurrentGameState = GameState.Help;
                    btnHelp.Update(mouse);
                    break;

                case GameState.Playing:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = GameState.MainMenu;

                    if (ball.lives == 0)
                    {
                        goto case GameState.MainMenu;
                    }

                    break;

                case GameState.About:
                    CurrentGameState = GameState.About;
                    RemoveObject();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = GameState.MainMenu;
                   

                    break;

                case GameState.Help:
                    CurrentGameState = GameState.Help;
                    RemoveObject();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = GameState.MainMenu;
                  

                    break;

                case GameState.Level2:
                    CurrentGameState = GameState.Level2;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = GameState.MainMenu;

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        this.Components.Remove(L2);
                    }
                    break;

                case GameState.Level3:
                    CurrentGameState = GameState.Level3;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        CurrentGameState = GameState.MainMenu;

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        this.Components.Remove(L3);
                    }
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws all components it is told to draw. Includes if statements to determine what GameState the game is in, which helps it draw what it required for each screen / state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //If gamestate is mainmenu, draw menu buttons, background, ensure no bricks are shown, reset lives, etc
            if (CurrentGameState == GameState.MainMenu)
            {
                RemoveObject();
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                ball.lives = 3;
                btnPlay.Draw(spriteBatch);
                btnAbout.Draw(spriteBatch);
                btnHelp.Draw(spriteBatch);
            }

            //Otherwise, if gamestate is playing, use mouse for paddle, call checkballcollision method (self explanatory) the paddle and for each brick, and if hit, add to score and play sound effect, etc
            else if (CurrentGameState == GameState.Playing)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    CurrentGameState = GameState.MainMenu;

                else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !ball.run)
                    ball.run = true;

                paddle.posX = Mouse.GetState().X;
                paddle.CheckPaddleBallCollision(ball);

                if (paddle.isHit)
                {
                    paddleBounce.Play();
                }

                foreach (var item in lstBricks)
                {
                    if (item.CheckBallCollision(ball))
                    {
                        effect.Play();
                        score += 25;
                        rndPosX = rndX.Next(50, 425);
                        rndPosY = rndY.Next(180, 350);
                        fire.Position = new Vector2(rndPosX, rndPosY);
                        fire.start();
                        item.Active = false;
                    }
                }

                for (int i = 0; i < lstBricks.Count; i++)
                {
                    if (lstBricks[i].Active == false)
                    {
                        lstBricks.Remove(lstBricks[i]);
                    }
                }

                if (lstBricks.Count == 0)
                {
                    CurrentGameState = GameState.Level2;
                    ball.run = false;
                    ball.ResetBallPosition();
                    this.Components.Add(L2);
                    LoadObjectsLevel2();
                }

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(HUD, "Score:" + score.ToString(), new Vector2(10, 10), Color.Green);
                spriteBatch.Draw(pixel, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, GraphicsDevice.Viewport.Width, 1), Color.White);
                spriteBatch.DrawString(HUD, "Lives:" + (ball.lives).ToString(), posLives, Color.Green);

                if (ball.lives == 0)
                {
                    RemoveObject();
                    CurrentGameState = GameState.MainMenu;
                }
            }

            //Otherwise, if it is next level, show next level bricks, rest of the code is the same from level one. Only difference from this and level 3 is the level 3 brick generating
            else if (CurrentGameState == GameState.Level2)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    CurrentGameState = GameState.MainMenu;

                else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !ball.run)
                    ball.run = true;

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                paddle.ResetPaddlePosition();
                paddle.posX = Mouse.GetState().X;
                paddle.CheckPaddleBallCollision(ball);

                if (paddle.isHit)
                {
                    paddleBounce.Play();
                }

                foreach (var item in lstBrickLevel2)
                {
                    if (item.CheckBallCollisionLevel2(ball))
                    {
                        effect.Play();
                        score2 += 25;
                        rndPosX = rndX.Next(50, 425);
                        rndPosY = rndY.Next(180, 350);
                        fire.Position = new Vector2(rndPosX, rndPosY);
                        fire.start();
                        item.Active = false;
                    }

                }

                for (int i = 0; i < lstBrickLevel2.Count; i++)
                {
                    if (lstBrickLevel2[i].Active == false)
                    {
                        lstBrickLevel2.Remove(lstBrickLevel2[i]);
                    }
                }

                if (lstBrickLevel2.Count == 0)
                {
                    CurrentGameState = GameState.Level3;
                    ball.run = false;
                    ball.ResetBallPosition();
                    this.Components.Add(L3);
                    LoadObjectsLevel3();
                }

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(HUD, "Score:" + (score + score2).ToString(), new Vector2(10, 10), Color.Green);
                spriteBatch.Draw(pixel, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, GraphicsDevice.Viewport.Width, 1), Color.White);
                spriteBatch.DrawString(HUD, "Lives:" + (ball.lives).ToString(), posLives, Color.Green);

                if (ball.lives == 0)
                {
                    CurrentGameState = GameState.MainMenu;
                }
            }

            else if (CurrentGameState == GameState.Level3)
            {
                ball.lives = 3;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    CurrentGameState = GameState.MainMenu;

                else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !ball.run)
                    ball.run = true;

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                paddle.ResetPaddlePosition();
                paddle.posX = Mouse.GetState().X;
                paddle.CheckPaddleBallCollision(ball);

                if (paddle.isHit)
                {
                    paddleBounce.Play();
                }

                foreach (var item in lstBrickLevel3)
                {
                    if (item.CheckBallCollisionLevel3(ball))
                    {
                        effect.Play();
                        score3 += 25;
                        rndPosX = rndX.Next(50, 425);
                        rndPosY = rndY.Next(180, 350);
                        fire.Position = new Vector2(rndPosX, rndPosY);
                        fire.start();
                        item.Active = false;
                    }
                }

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(HUD, "Score:" + (score + score2 + score3).ToString(), new Vector2(10, 10), Color.Green);
                spriteBatch.Draw(pixel, new Rectangle(0, GraphicsDevice.Viewport.Height - 20, GraphicsDevice.Viewport.Width, 1), Color.White);
                spriteBatch.DrawString(HUD, "Lives:" + (ball.lives).ToString(), posLives, Color.Green);

                if (ball.lives == 0)
                {
                    CurrentGameState = GameState.MainMenu;
                }
            }

            else if (CurrentGameState == GameState.About)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(aboutScreen, new Vector2(0, 300), Color.White);


            }

            else if (CurrentGameState == GameState.Help)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(helpScreen, new Vector2(0, 200), Color.White);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}