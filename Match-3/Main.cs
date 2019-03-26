using Match_3.StageComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match_3
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 692;
            graphics.PreferredBackBufferHeight = 734;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameContext.ContentManager = Content;
            GameContext.Graphics = graphics;
            GameContext.GraphicsDevice = GraphicsDevice;

            TexturePool.LoadTextures();

            ScreenManager.AddScreen("Menu", new MainMenuScreen());
            ScreenManager.AddScreen("Game", new GameScreen());
            ScreenManager.SetScreen("Menu");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            ScreenManager.UpdateCurrentScreen(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            ScreenManager.DrawCurrentScreen(gameTime, spriteBatch);
        }
    }
}
