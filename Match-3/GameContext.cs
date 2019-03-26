using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/*
 * Класс для удобного доступа к необходим модулям движка, без предачи их в качестве параметров
 */

namespace Match_3
{
    static class GameContext
    {
        public static ContentManager ContentManager;
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice GraphicsDevice;
    }
}
