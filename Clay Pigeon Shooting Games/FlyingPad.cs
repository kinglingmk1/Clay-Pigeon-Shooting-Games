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
        public Texture2D texture, mousePoint, mouseMiddlePoint;
        public Vector2 position, center, velocity, targetPosition, targetMiddlllePosition;
        public float rotateAngle, rotateSpeed;
        public static Random r = new Random();
        SpriteBatch spriteBatch;
        public FlyingPad(Game g) : base(g) { }
        public Color[] data;
        public Color[] middleData;
        public Boolean hit = false;
        MouseState mouseLastState = Mouse.GetState();
        public int score = 0;
        public Game1 g;
        public Rectangle flyingPadsRectangle, mouseMiddleRectangle;
        public bool fCollision = false;
        SpriteFont font;
        List<SoundEffect> PadShooted = new List<SoundEffect>();

        public override void Initialize()
        {
            position.X = 0;
            position.Y = r.Next(GraphicsDevice.Viewport.Height);
            velocity.X = r.Next(40, 80) / 10;
            velocity.Y = 0;
            rotateSpeed = 0;
            rotateAngle = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("image\\NewFlyingPad");
            mousePoint = Game.Content.Load<Texture2D>("image\\MousePoint");
            mouseMiddlePoint = Game.Content.Load<Texture2D>("image\\MouseMiddlePoint");
            font = Game.Content.Load<SpriteFont>("font\\MyFont");
            PadShooted.Add(Game.Content.Load<SoundEffect>("soundEffect\\PadDestroySound"));
            center.X = texture.Width / 2.0f;
            center.Y = texture.Height / 2.0f;
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
            middleData = new Color[mouseMiddlePoint.Width * mouseMiddlePoint.Height];
            mouseMiddlePoint.GetData<Color>(middleData);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            position.X += velocity.X;
            MouseState mouseState = Mouse.GetState();
            targetPosition.X = mouseState.X - (mousePoint.Width / 2);
            targetPosition.Y = mouseState.Y - (mousePoint.Height / 2);
            targetMiddlllePosition.X = mouseState.X - (mouseMiddlePoint.Width / 2);
            targetMiddlllePosition.Y = mouseState.Y - (mouseMiddlePoint.Height / 2);

            flyingPadsRectangle = new Rectangle((int)position.X - 50, (int)position.Y -50, texture.Width, texture.Height);
            mouseMiddleRectangle = new Rectangle((int)targetMiddlllePosition.X, (int)targetMiddlllePosition.Y, mouseMiddlePoint.Width, mouseMiddlePoint.Height);

            if (mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) // Left Click 當按下左鍵并且當前上一階段左鍵是放開
            {
                if(IntersectPixels(flyingPadsRectangle, data, mouseMiddleRectangle, middleData))
                {
                    fCollision = true;
                    PadShooted[0].CreateInstance().Play();
                    position.X = 0;
                    position.Y = r.Next(GraphicsDevice.Viewport.Height);
                    velocity.X = r.Next(40, 80) / 10;
                    score++;
                }
            }
            mouseLastState = mouseState; //Record the last statement of mouse 記錄最後滑鼠狀態
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(texture, position, null, Color.White, rotateAngle, center, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(texture, flyingPadsRectangle, Color.White);
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