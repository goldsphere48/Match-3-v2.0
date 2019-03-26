using Match_3.GameEntities;
using Match_3.GameEntities.Objects;
using Match_3.StageComponents;
using Match_3.StageComponents.Actions;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;
using static Match_3.GameEntities.Objects.GameElement;

namespace Match_3
{
    class MainMenuScreen : IScreen
    {
        private Stage stage = new Stage();
        private Texture2D texture;
       
        public void Initialize()
        {
            Image background = new Image("background");
            Image playButton = new Image("playButton");
            playButton.OnMouseClick = (button, x, y, self) => ScreenManager.SetScreen("Game");
            stage.AddActor(background);
            stage.AddActor(playButton);

            GameElement element = GameElementFactory.CreateGameElement(ElementColor.Blue, ElementType.Regular);
            element.AddAction(new MoveAction(new Vector2(100, 100), 4));
            element.AddAction(new RunableAction((self) => {
                Console.WriteLine("Hello");
            }));
            stage.AddActor(element);
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
