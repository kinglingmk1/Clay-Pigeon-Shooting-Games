using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Clay_Pigeon_Shooting_Games
{
    class FlyingPad : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Texture2D FlyingPads, mousePoint, mouseMiddlePoint, FlyingPads2;
        public Vector2 position, center, velocity, targetPosition, targetMiddlllePosition;
        public Vector2 position2, center2, velocity2, targetPosition2, targetMiddlllePositio2;
        public float rotateAngle, rotateSpeed;
        public static Random r = new Random();
        SpriteBatch spriteBatch;
        public FlyingPad(Game g) : base(g) { }
        public Color[] FlyingPadsData;
        public Color[] middleData;
        public Color[] FlyingPads2Data;
        public Boolean hit = false;
        MouseState mouseLastState = Mouse.GetState();
        public int score = 0;
        public Game1 g;
        public Rectangle flyingPadsRectangle, mouseMiddleRectangle, flyingPads2Rectangle;
        public bool fCollision = false;
        SpriteFont font;
        List<SoundEffect> PadShooted = new List<SoundEffect>();

        public override void Initialize()
        {
            position.X = 0;
            position.Y = r.Next(50,GraphicsDevice.Viewport.Height * 1/2);
            velocity.X = r.Next(40, 80) / 10;
            velocity.Y = r.Next(1, 20) / 10;
            position2.X = 0;
            position2.Y = r.Next(50,GraphicsDevice.Viewport.Height * 1 / 2 );
            velocity2.X = r.Next(40, 80) / 10;
            velocity2.Y = r.Next(1, 20) / 10;
            rotateSpeed = 0;
            rotateAngle = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            FlyingPads = Game.Content.Load<Texture2D>("image\\NewFlyingPad");
            FlyingPads2 = Game.Content.Load<Texture2D>("image\\NewFlyingPad2");
            mousePoint = Game.Content.Load<Texture2D>("image\\MousePoint");
            mouseMiddlePoint = Game.Content.Load<Texture2D>("image\\MouseMiddlePoint");
            font = Game.Content.Load<SpriteFont>("font\\MyFont");
            PadShooted.Add(Game.Content.Load<SoundEffect>("soundEffect\\PadDestroySound"));
            center.X = FlyingPads.Width / 2.0f;
            center.Y = FlyingPads.Height / 2.0f;
            FlyingPadsData = new Color[FlyingPads.Width * FlyingPads.Height];
            FlyingPads.GetData<Color>(FlyingPadsData);
            FlyingPads2Data = new Color[FlyingPads2.Width * FlyingPads2.Height];
            FlyingPads2.GetData<Color>(FlyingPads2Data);
            middleData = new Color[mouseMiddlePoint.Width * mouseMiddlePoint.Height];
            mouseMiddlePoint.GetData<Color>(middleData);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            position.X += velocity.X;
            position.Y += velocity.Y;
            position2.X += velocity2.X;
            position2.Y += velocity2.Y;
            MouseState mouseState = Mouse.GetState();
            targetPosition.X = mouseState.X - (mousePoint.Width / 2);
            targetPosition.Y = mouseState.Y - (mousePoint.Height / 2);
            targetMiddlllePosition.X = mouseState.X - (mouseMiddlePoint.Width / 2);
            targetMiddlllePosition.Y = mouseState.Y - (mouseMiddlePoint.Height / 2);

            if(mouseState.Y > GraphicsDevice.Viewport.Height)
            {
                targetPosition.Y = GraphicsDevice.Viewport.Height - (mousePoint.Height / 2);
                targetMiddlllePosition.Y = GraphicsDevice.Viewport.Height - (mousePoint.Height / 2);
            }
            if (mouseState.X > GraphicsDevice.Viewport.Width)
            {
                targetPosition.X = GraphicsDevice.Viewport.Width - (mousePoint.Width / 2);
                targetMiddlllePosition.X = GraphicsDevice.Viewport.Width - (mousePoint.Width / 2);
            }
            if(mouseState.Y < 0)
            {
                targetPosition.Y = 0 - (mousePoint.Height / 2);
                targetMiddlllePosition.Y = 0 - (mousePoint.Height / 2);
            }
            if (mouseState.X < 0)
            {
                targetPosition.X = 0 - (mousePoint.Width / 2);
                targetMiddlllePosition.X = 0 - (mousePoint.Width / 2);
            }

            flyingPadsRectangle = new Rectangle((int)position.X - 50, (int)position.Y -50, FlyingPads.Width, FlyingPads.Height);
            flyingPads2Rectangle = new Rectangle((int)position2.X - 50, (int)position2.Y - 50, FlyingPads2.Width, FlyingPads2.Height);
            mouseMiddleRectangle = new Rectangle((int)targetMiddlllePosition.X, (int)targetMiddlllePosition.Y, mouseMiddlePoint.Width, mouseMiddlePoint.Height);
            if (mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) // Left Click 當按下左鍵并且當前上一階段左鍵是放開
            {
                if(IntersectPixels(flyingPadsRectangle, FlyingPadsData, mouseMiddleRectangle, middleData))
                {
                    fCollision = true;
                    PadShooted[0].CreateInstance().Play();
                    position.X = 0;
                    velocity.X = r.Next(40, 80) / 10;
                    velocity.Y = r.Next(1, 20) / 10;
                    position.Y = r.Next(50,GraphicsDevice.Viewport.Height * 1 / 2);
                    score++;
                }
                if (IntersectPixels(flyingPads2Rectangle, FlyingPads2Data, mouseMiddleRectangle, middleData))
                {
                    fCollision = true;
                    PadShooted[0].CreateInstance().Play();
                    position2.X = 0;
                    velocity2.X = r.Next(40, 80) / 10;
                    velocity2.Y = r.Next(1, 20) / 10;
                    position2.Y = r.Next(50,GraphicsDevice.Viewport.Height * 1 / 2);
                    score++;
                }
            }
            mouseLastState = mouseState; //Record the last statement of mouse 記錄最後滑鼠狀態
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(FlyingPads, position, null, Color.White, rotateAngle, center, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(FlyingPads, flyingPadsRectangle, Color.White);
            spriteBatch.Draw(FlyingPads2, flyingPads2Rectangle, Color.White);
            spriteBatch.Draw(mouseMiddlePoint, mouseMiddleRectangle, Color.White);
            spriteBatch.Draw(mousePoint, targetPosition, Color.White); //Generate sight bead 準心生成
            spriteBatch.Draw(mouseMiddlePoint, targetPosition, Color.White); //Generate sight bead 準心生成
            //spriteBatch.DrawString(font, "Score: " + score, new Vector2(20, GraphicsDevice.Viewport.Height - 30), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false; // No intersection found
        }
    }
}