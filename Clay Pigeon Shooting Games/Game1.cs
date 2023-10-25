﻿using Microsoft.Xna.Framework;
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
        Texture2D gameBackground, mousePoint, mouseMiddlePoint;
        public Vector2 position, center, velocity;
        //Song GunSound;
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
            FlyingPads = new FlyingPad[4];
            for (int i = 0; i < 4; i++)
            {
                FlyingPads[i] = new FlyingPad(this);
                Components.Add(FlyingPads[i]);
            }

            GFire = new GunFire(this);
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
            mousePoint = Content.Load<Texture2D>("image\\MousePoint");
            mouseMiddlePoint = Content.Load<Texture2D>("image\\MouseMiddlePoint");


            bgm = Content.Load<Song>("soundEffect\\bgm");
            MediaPlayer.Play(bgm);

            //GunSound = Content.Load<Song>("soundEffect\\GunSound");
            GunSound.Add(Content.Load<SoundEffect>("soundEffect\\GunSound"));
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
            
            //Gun Sound
            MouseState mouseState = Mouse.GetState();
            position.X = mouseState.X - (mousePoint.Width/2);
            position.Y = mouseState.Y - (mouseMiddlePoint.Width / 2);
            if (mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) // Left Click
            {
                //Limit Gun short
                if(GFire.currentFrame == 0)
                {
                    GunSound[0].CreateInstance().Play();
                }
                
                
            }
            mouseLastState = mouseState;

            //Mouse Move

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
            spriteBatch.Draw(gameBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(mousePoint, position, Color.White);
            spriteBatch.Draw(mouseMiddlePoint, position, Color.White);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
