using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3
{
    interface IScreen
    {
        //Инициализвация ресурсов задействованных в экране, вызывается перед установкой экрана как текущего
        void Initialize();

        //Содержит методы отрисовки графических элеметов
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        //Реализует логику взаимодействия элементов на экране
        void Update(GameTime gameTime);

        //Метод вызываемый после смены экрана на новый
        void Shutdown();
    }
}
