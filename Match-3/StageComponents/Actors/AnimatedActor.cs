using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.StageComponents.Actors
{
    class AnimatedActor : Image
    {
        private int rows;
        private int columns;
        private double currentFrameIndex;
        private int framesCount;
        private float animSpeed;
        private bool isAnimated;
        public bool IsAnimated
        {
            get => isAnimated;
        }

        public AnimatedActor(string textureName, int frameWidth, int frameHeight, float animSpeed) : base(textureName)
        {
            Init(frameWidth, frameHeight, animSpeed);
        }

        public AnimatedActor(Texture2D texture, int frameWidth, int frameHeight, float animSpeed) : base(texture)
        {
            Init(frameWidth, frameHeight, animSpeed);
        }

        protected void Init(int frameWidth, int frameHeight, float animSpeed)
        {
            columns = texture.Width / frameWidth;
            rows = texture.Height / frameHeight;
            Width = texture.Width / columns;
            Height = texture.Height / rows;
            this.animSpeed = animSpeed;
            framesCount = rows * columns;
        }

        protected AnimatedActor()
        {
            
        }

        //Остановка анимации
        public void StopAnimation()
        {
            isAnimated = false;
        }

        //Возобновление анимации
        public void StartAnimation()
        {
            isAnimated = true;
        }

        public void InvertAnimationState()
        {
            isAnimated = !IsAnimated;
            BackToFirstfame();
        }

        //Установка первого кадра как текущего
        public void BackToFirstfame()
        {
            currentFrameIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsAnimated)
            {
                currentFrameIndex += animSpeed * gameTime.ElapsedGameTime.TotalSeconds;
                if (currentFrameIndex >= framesCount)
                    currentFrameIndex = 0;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            int row = (int)(currentFrameIndex / columns);
            int column = (int)currentFrameIndex % columns;
            Rectangle sourceRectangle = new Rectangle(Width * column, Height * row, Width, Height);
            Draw(batch, sourceRectangle);
        }
    }
}
