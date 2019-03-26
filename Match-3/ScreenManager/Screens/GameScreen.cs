using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match_3.GameEntities;
using Match_3.GameEntities.Objects;
using Match_3.StageComponents;
using Match_3.StageComponents.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Match_3.GameEntities.Objects.GameElement;

namespace Match_3
{
    class GameScreen : IScreen
    {
        private Stage stage = new Stage();
        private Grid grid;

        public void Initialize()
        {
            grid = new Grid();
            grid.X = 29;
            grid.Y = 73;
            stage.AddActor(new Image("gameBackground"));
            stage.AddActor(grid);
            
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
