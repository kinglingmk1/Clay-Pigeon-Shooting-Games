using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            MouseState mouseState = Mouse.GetState();
            position.X = mouseState.X; //Move guns right left
            //position.Y = mouseState.Y; //Move guns up down but I think not good
            if ((mouseState.LeftButton == ButtonState.Pressed && mouseLastState.LeftButton == ButtonState.Released) || currentFrame != 0)
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

            spriteBatch.Begin();
            
            spriteBatch.Draw(texture, position, frameRect, Color.White, 0.0f, Vector2.Zero, 1.0f, direction, 0.5f);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

