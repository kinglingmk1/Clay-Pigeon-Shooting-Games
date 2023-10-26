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
        Texture2D gameBackground;
        public Vector2 position, center, velocity;
        Song bgm;
        List<SoundEffect> GunSound;
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
        public int padNumner = 3;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GunSound = new List<SoundEffect>();
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

            FlyingPads = new FlyingPad[padNumner + stage]; //Flying Pads numbers 飛碟數量
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
            GunSound.Add(Content.Load<SoundEffect>("soundEffect\\GunSound"));
            bgm = Content.Load<Song>("soundEffect\\bgm");
            MediaPlayer.Play(bgm); //Play bgm 播放背景音樂
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
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) // Left Click 當按下左鍵并且當前上一階段左鍵是放開
            {
                if(GFire.currentFrame == 0) //Limit Gun short 限制瘋狂射擊
                {
                    GunSound[0].CreateInstance().Play(); //Gun Sound Play 播放槍聲
                    //if collision score = 1 * stage + scores
                }
            }
            mouseLastState = mouseState; //Record the last statement of mouse 記錄最後滑鼠狀態

            if(Keyboard.GetState().IsKeyDown(Keys.R) && (miss >= misslimit || stage>3)) //Press R reset game
            {
                miss = 0;
                padNumner = 3;
                stage = 1;
                FP.score = 0;
            }
            
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
                        score ++;
                        FlyingPads[i].fCollision = false;
                        FlyingPads[i].score = 0;
                        FlyingPads[i].Initialize();
                    }
                    
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
            spriteBatch.DrawString(font, "Miss: " + miss + "/" + misslimit, new Vector2(20, GraphicsDevice.Viewport.Height - 50), Color.White);
            spriteBatch.DrawString(font, "Stage: " + stage + "/3", new Vector2(20, GraphicsDevice.Viewport.Height - 70), Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(20, GraphicsDevice.Viewport.Height - 30), Color.White);

            if (miss >= misslimit)
            {
                spriteBatch.DrawString(font, "Game Over Press R to replay", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height /2), Color.Black);
                for(int i=0; i<FlyingPads.Length;i++)
                {
                    miss = 10;
                    FlyingPads[i].position.X = -100;
                }
            }
            if (stage > 3)
            {
                spriteBatch.DrawString(font, "You win. Press R to Play Again", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Black);
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
