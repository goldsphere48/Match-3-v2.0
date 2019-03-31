using System;
using Match_3.StageComponents;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match_3
{
    class GameOverScreen : IScreen
    {
        private Stage stage = new Stage();

        public void Initialize()
        { 
            Image background = new Image("background");
            stage.AddActor(background);

            Image okButton = new Image("okButton");
            okButton.OnMouseClick = (button, x, y, self) =>
            {
                ScreenManager.SetScreen("Menu");
            };
            okButton.Origin = new Vector2(okButton.X / 2, okButton.Y / 2);
            okButton.X = GameContext.Graphics.PreferredBackBufferWidth / 2 - okButton.Width / 2;
            okButton.Y = GameContext.Graphics.PreferredBackBufferHeight / 2 - okButton.Height / 2 + 200;
            stage.AddActor(okButton);

            Image gameOver = new Image("gameOver");
            gameOver.X = GameContext.Graphics.PreferredBackBufferWidth / 2 - gameOver.Width / 2;
            gameOver.Y = GameContext.Graphics.PreferredBackBufferHeight / 2 - gameOver.Height / 2 - 100;
            gameOver.Origin = new Vector2(gameOver.X / 2, gameOver.Y / 2);
            stage.AddActor(gameOver);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            stage.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            stage.Update(gameTime);
        }

        public void Shutdown()
        {

        }
    }
}
