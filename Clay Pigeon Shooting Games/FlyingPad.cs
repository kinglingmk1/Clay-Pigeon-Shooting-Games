using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Clay_Pigeon_Shooting_Games
{
    class FlyingPad : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Texture2D texture;
        public Vector2 position, center, velocity;
        public float rotateAngle, rotateSpeed;
        public static Random r = new Random();
        SpriteBatch spritebatch;
        public FlyingPad(Game g) : base(g) { }
        public Color[] data;
        public Boolean hit = false;
        MouseState mouseLastState = Mouse.GetState();

        public override void Initialize()
        {
            position.X = 0;
            position.Y = r.Next(GraphicsDevice.Viewport.Height);
            velocity.X = r.Next(40, 80) / 10;
            velocity.Y = 0;
            rotateSpeed = velocity.Y / 10.0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("image\\NewFlyingPad");
            center.X = texture.Width / 2.0f;
            center.Y = texture.Height / 2.0f;

            //load color
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
        }

        public override void Update(GameTime gameTime)
        {
            position.X += velocity.X;
            //rotateAngle = (rotateAngle + rotateSpeed) % MathHelper.TwoPi;
            if (position.X > 1000)
            {
                hit = true;
                Initialize();
                position.X = 0;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            spritebatch.Draw(texture, position, null, Color.White, rotateAngle, center, 1.0f, SpriteEffects.None, 0.5f);
            spritebatch.End();
            base.Draw(gameTime);
        }

    }
}