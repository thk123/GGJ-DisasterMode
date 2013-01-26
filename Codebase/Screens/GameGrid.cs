using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GGJ_DisasterMode.Codebase.Screens
{   
    class GameGrid : DrawableGameComponent
    {
        private int xPosition;
        private int yPosition;
        private int cellWidth;
        private int cellHeight;
        private int cellCountX;
        private int cellCountY;

        private float lineWidth;
        private Texture2D lineTexture;
        private Color lineColor;

        private SpriteBatch spriteBatch;

        public GameGrid(Game game, int xPosition, int yPosition, int cellWidth, int cellHeight, int xCellCount, int yCellCount, float lineWidth, Color lineColor)
            : base(game)
        {
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;           
            this.cellCountX = xCellCount;
            this.cellCountY = yCellCount;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
        }

        protected override void  LoadContent()
        {
            this.lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData(new[] { Color.White });

            this.spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            /* Do Nothing */
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            for (int i = 0; i < this.cellCountX + 1; i++)
            {
                DrawLine(this.spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.xPosition + (i * cellWidth), this.yPosition),
                    new Vector2(this.xPosition + (i * cellWidth), this.yPosition + (cellCountY * cellHeight)));
            }
            for (int i = 0; i < this.cellCountY + 1; i++)
            {
                DrawLine(this.spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.xPosition, this.yPosition + (i * cellHeight)),
                    new Vector2(this.xPosition + (cellCountY * cellWidth), this.yPosition + (i * cellHeight)) );
            }

            this.spriteBatch.End();
        }

        private void DrawLine(SpriteBatch batch, Texture2D lineTexture,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(lineTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
