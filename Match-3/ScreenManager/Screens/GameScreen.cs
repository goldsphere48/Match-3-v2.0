using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private SpriteFont font;
        private Grid grid;
        private Timer timer;
        private int timeLeft = 60;

        public void Initialize()
        {
            Image gridBackground = new Image("gridBackground");
            gridBackground.X = 19;
            gridBackground.Y = 63;
            stage.AddActor(gridBackground);
            font = GameContext.ContentManager.Load<SpriteFont>("Font");
            grid = new Grid();
            grid.X = 29;
            grid.Y = 73;
            stage.AddActor(grid);
            stage.AddActor(new Image("gameBackground"));
            timer = new Timer(1000);
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
            }
            else
            {
                timer.Stop();
                ScreenManager.SetScreen("GameOver");
            }
        }

        public void Update(GameTime gameTime)
        {
            stage.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameContext.GraphicsDevice.Clear(new Color(255, 202, 85));
            stage.Draw(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + User.Score, new Vector2(30, 10), Color.White);
            spriteBatch.DrawString(font, "Timer: " + timeLeft, new Vector2(450, 10), Color.White);

            spriteBatch.End();
        }

        public void Shutdown()
        {
            grid.Dispose();
            stage.Childrens.Clear();
            timeLeft = 60;
            timer.Close();
        }
    }
}
