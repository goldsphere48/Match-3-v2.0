using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Match_3
{
    static class ScreenManager
    {
        private static Dictionary<string, IScreen> lstScreens = new Dictionary<string, IScreen>();
        private static IScreen activeScreen;

        public static void AddScreen(string screenName, IScreen screen)
        {
            if (!Contains(screenName))
            {
                lstScreens.Add(screenName, screen);
            }
        }

        public static void DeleteScreen(string screenName)
        {
            if (Contains(screenName))
            {
                lstScreens[screenName].Shutdown();
                lstScreens.Remove(screenName);
            }
        }

        public static IScreen Get(string screenName)
        {
            IScreen screen;
            lstScreens.TryGetValue(screenName, out screen);
            return screen;
        }

        public static bool Contains(string screenName)
        {
            return lstScreens.ContainsKey(screenName);
        }

        public static void SetScreen(string screenName)
        {
            if (Contains(screenName))
            {
                if(activeScreen != null) activeScreen.Shutdown(); //Если уже есть установленный экран, закрыть его
                lstScreens.TryGetValue(screenName, out activeScreen);
                activeScreen.Initialize();
            }
        }

        public static void DrawCurrentScreen(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (activeScreen != null)
                activeScreen.Draw(gameTime, spriteBatch);
        }

        public static void UpdateCurrentScreen(GameTime gameTime)
        {
            if (activeScreen != null)
                activeScreen.Update(gameTime);
        }

    }
}
