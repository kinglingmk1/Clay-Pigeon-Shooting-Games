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
        GunFire GFire;


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

            FlyingPads = new FlyingPad[4]; //Flying Pads numbers 飛碟數量
            for (int i = 0; i < 4; i++)
            {
                FlyingPads[i] = new FlyingPad(this);
                Components.Add(FlyingPads[i]);
            }
            GFire = new GunFire(this); //Gun's animation and Crosshairs 槍支動畫及準星
            Components.Add(GFire);
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
                }
            }
            mouseLastState = mouseState; //Record the last statement of mouse 記錄最後滑鼠狀態
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
