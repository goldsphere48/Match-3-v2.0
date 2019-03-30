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
        private Grid grid2;

        public void Initialize()
        {
            grid2 = new Grid();
            grid2.X = 29;
            grid2.Y = 73;
            stage.AddActor(grid2);
            stage.AddActor(new Image("gameBackground"));

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameContext.GraphicsDevice.Clear(new Color(255, 202, 85));
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
