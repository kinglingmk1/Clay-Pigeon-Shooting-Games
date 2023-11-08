using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Clay_Pigeon_Shooting_Games
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D gameBackground ,bulletHole;
        public Vector2 position, center, velocity;
        public Vector2[] BulletHolePosition;
        Song bgm;
        List<SoundEffect> GunSound, round1,round2,finalRound, victory, lose;
        MouseState mouseLastState = Mouse.GetState();
        FlyingPad[] FlyingPads;
        FlyingPad FP;
        GunFire GFire;
        SpriteFont font;
        public static Random F = new Random();
        public int score = 0;
        public int miss = 0;
        public int misslimit = 10;
        public int stage = 1;
        public int padNumner = 1;
        public int scoreMax = 10;
        public bool playonce = true;
        public int bulletHoleCount = 0;
        public int MaxBulletHole = 1000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GunSound = new List<SoundEffect>();
            round1 = new List<SoundEffect>();
            round2 = new List<SoundEffect>();
            finalRound = new List<SoundEffect>();
            victory = new List<SoundEffect>();
            lose = new List<SoundEffect>();
            BulletHolePosition = new Vector2[MaxBulletHole];
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            FlyingPads = new FlyingPad[padNumner]; //Flying Pads numbers 飛碟數量
            for (int i = 0; i < FlyingPads.Length; i++)
            {
                FlyingPads[i] = new FlyingPad(this);
                Components.Add(FlyingPads[i]);
            }
            GFire = new GunFire(this); //Gun's animation and Crosshairs 槍支動畫及準星
            Components.Add(GFire);
            FP = new FlyingPad(this);
            Components.Add(FP);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font\\MyFont");
            gameBackground = Content.Load<Texture2D>("image\\WallPaper");
            bulletHole = Content.Load<Texture2D>("image\\BulletHole");
            GunSound.Add(Content.Load<SoundEffect>("soundEffect\\GunSound"));            
            bgm = Content.Load<Song>("soundEffect\\bgm");
            MediaPlayer.Play(bgm); //Play bgm 播放背景音樂
            round1.Add(Content.Load<SoundEffect>("soundEffect\\Round1"));
            round2.Add(Content.Load<SoundEffect>("soundEffect\\Round2"));
            finalRound.Add(Content.Load<SoundEffect>("soundEffect\\Round3"));
            victory.Add(Content.Load<SoundEffect>("soundEffect\\Victory"));
            lose.Add(Content.Load<SoundEffect>("soundEffect\\Lost"));
            round1[0].CreateInstance().Play();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.R) && (miss >= misslimit || score>=30)) //Press R reset game
            {
                BulletHolePosition = new Vector2[MaxBulletHole];
                playonce = true;
                round1[0].CreateInstance().Play();
                miss = 0;
                padNumner = 3;
                stage = 1;
                FP.score = 0;
                scoreMax = 10;
                score = 0;
                misslimit = 10;
            }
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) // Left Click 當按下左鍵并且當前上一階段左鍵是放開
            {
                GFire.currentFrame = 0;
                if (GFire.currentFrame == 0) //Limit Gun short 限制瘋狂射擊
                {
                    BulletHolePosition[bulletHoleCount].X = mouseState.Position.X - bulletHole.Width / 2;
                    BulletHolePosition[bulletHoleCount].Y = mouseState.Position.Y - bulletHole.Height / 2;
                    bulletHoleCount++;
                    if (bulletHoleCount >= MaxBulletHole)
                    {
                        bulletHoleCount = 0;
                    }
                    GunSound[0].CreateInstance().Play(); //Gun Sound Play 播放槍聲
                    //if collision score = 1 * stage + scores
                }
            }
            mouseLastState = mouseState; //Record the last statement of mouse 記錄最後滑鼠狀態
            for (int i=0; i< FlyingPads.Length; i++)
            {
                if (FlyingPads[i].position.X > 1000) //Out of range then count to miss
                {
                    miss++;
                    FlyingPads[i].Initialize();
                }
                if (FlyingPads[i].fCollision == true)
                {
                    if (GFire.currentFrame == 0) //Limit Gun short 限制瘋狂射擊
                    {
                        if (miss < 10 || (stage == 3 && miss < 15)) 
                        {
                            score++;
                        }
                        FlyingPads[i].fCollision = false;
                        FlyingPads[i].score = 0;
                        //FlyingPads[i].Initialize();
                    }
                }
            }
            if (score >= scoreMax && stage == 3)
            {
                victory[0].CreateInstance().Play();
                score = 30;
                stage++;
                FlyingPads = new FlyingPad[padNumner + stage]; //Flying Pads numbers 飛碟數量
                for (int i = 0; i < FlyingPads.Length; i++)
                {
                    FlyingPads[i] = new FlyingPad(this);
                    Components.Add(FlyingPads[i]);
                }
            }
            if (miss >= misslimit && playonce == true)
            {
                playonce = false;
                lose[0].CreateInstance().Play();
            }
            if (score>= scoreMax && stage == 2)
            {
                round2[0].CreateInstance().Play();
                //MediaPlayer.Play(finalRound);
                miss = 0;
                score = 0;
                scoreMax = 30;
                misslimit = 15;
                stage++;
                FlyingPads = new FlyingPad[padNumner + stage]; //Flying Pads numbers 飛碟數量
                for (int i = 0; i < FlyingPads.Length; i++)
                {
                    FlyingPads[i] = new FlyingPad(this);
                    Components.Add(FlyingPads[i]);
                }
            }
            if (score >= scoreMax && stage == 1)
            {
                finalRound[0].CreateInstance().Play();
                //MediaPlayer.Play(round2);
                miss = 0;
                score = 0;
                scoreMax = 20;
                stage++;
                FlyingPads = new FlyingPad[padNumner + stage]; //Flying Pads numbers 飛碟數量
                for (int i = 0; i < FlyingPads.Length; i++)
                {
                    FlyingPads[i] = new FlyingPad(this);
                    Components.Add(FlyingPads[i]);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(gameBackground, Vector2.Zero, Color.White); //Generate background 背景生產
            for(int i=0; i< MaxBulletHole; i++)
            {
                spriteBatch.Draw(bulletHole, BulletHolePosition[i], Color.White);
            }
            
            spriteBatch.DrawString(font, "Miss: " + miss + "/" + misslimit, new Vector2(20, GraphicsDevice.Viewport.Height - 50), Color.White);
            if(stage<=3)
            {
                spriteBatch.DrawString(font, "Stage: " + stage + "/3", new Vector2(20, GraphicsDevice.Viewport.Height - 70), Color.White);
            } else
            {
                spriteBatch.DrawString(font, "Stage: 3/3", new Vector2(20, GraphicsDevice.Viewport.Height - 70), Color.White);
            }
            spriteBatch.DrawString(font, "Score: " + score +"/" + scoreMax, new Vector2(20, GraphicsDevice.Viewport.Height - 30), Color.White);
            if (miss >= misslimit)
            {
                if(stage == 3)
                {
                    miss = 15;
                }else
                {
                    miss = 10;
                }
                spriteBatch.DrawString(font, "Game Over Press R to replay or ESC to Exit", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height /2), Color.Black);
                for(int i=0; i<FlyingPads.Length;i++)
                {
                    
                    FlyingPads[i].position.X = -100;
                }
            }
            if (stage > 3 || score >= 30)
            {
                score = 30;
                spriteBatch.DrawString(font, "You win. Press R to Play Again or ESC to Exit", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Black);
                for (int i = 0; i < FlyingPads.Length; i++)
                {
                    FlyingPads[i].position.X = -100;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
