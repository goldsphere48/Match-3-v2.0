using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents
{
    static class TexturePool
    {
        private static Dictionary<string, Texture2D> pool = new Dictionary<string, Texture2D>();

        public static Texture2D Get(string name)
        {
            if (pool.ContainsKey(name))
            {
                Texture2D texture;
                pool.TryGetValue(name, out texture);
                return texture;
            }
            else
            {
                Texture2D texture = Load(name);
                return texture;
            }
        }

        public static void LoadTextures()
        {
            Load("background");
            Load("gameBackground");
            Load("gridBackground");
            Load("playButton");
            Load("Line");
            Load("Bomb");
            Load("Purple");
            Load("Green");
            Load("Blue");
            Load("Gold");
            Load("Brown");
            Load("Destroyer");
        }

        public static void UnloadContent()
        {
            foreach(var pair in pool)
            {
                pair.Value.Dispose();
            }
        }

        private static Texture2D Load(string name)
        {
            Texture2D texture = GameContext.ContentManager.Load<Texture2D>(name);
            pool.Add(name, texture);
            return texture;
        }
    }
}
