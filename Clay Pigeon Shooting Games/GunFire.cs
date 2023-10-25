﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace Clay_Pigeon_Shooting_Games
{
    class GunFire : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        public Vector2 position, velocity;
        public Texture2D texture;
        public int frameCount, currentFrame;
        double frameElapsedTime, frameTimeStep;
        public Rectangle frameRect;
        public Color[] data;
        public GunFire(Game g) : base(g) { }
        SpriteEffects direction = SpriteEffects.None;
        MouseState mouseLastState = Mouse.GetState();

        public bool firstPositionAndFrame = true;
        public Vector2 firstPosition;
        public Rectangle firstFrameRect;
        public double timer = 0f;


        public override void Initialize()
        {
            position.X = Game.GraphicsDevice.Viewport.Width / 2;
            position.Y = 320;
            velocity.X = 4.5f;
            velocity.Y = 0;
            frameCount = 5;
            frameTimeStep = 1000/25;
            currentFrame = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("image\\GunShortMovement");

            frameRect = new Rectangle(0, 0, texture.Width / frameCount, texture.Height);
            //load color
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
        }


        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

                    // is it time to move on to the next frame?
                    
            MouseState mouseState = Mouse.GetState();
            if ((mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) || currentFrame !=0)
            {
                    frameElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds / frameTimeStep;
                    if (frameElapsedTime >= currentFrame)
                    {
                        currentFrame = (currentFrame + 1) % frameCount;
                        frameRect.X = currentFrame * frameRect.Width;
                        frameElapsedTime = 0;
                        // checking for screen edge
                    }
                // Left Click
                
                 // reset the elapsed counter
                
            }
            mouseLastState = mouseState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (velocity.X > 0)
            {
                direction = SpriteEffects.None;
            }
            else if (velocity.X < 0)
            {
                direction = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Begin();
            
            spriteBatch.Draw(texture, position, frameRect, Color.White, 0.0f, Vector2.Zero, 1.0f, direction, 0.5f);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

