using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GGJ_DisasterMode.Codebase.Screens
{   
    class GameGrid
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

        public GameGrid(Texture2D lineTexture, int xPosition, int yPosition, int cellWidth, int cellHeight, int xCellCount, int yCellCount, float lineWidth, Color lineColor)

        {
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;           
            this.cellCountX = xCellCount;
            this.cellCountY = yCellCount;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
            this.lineTexture = lineTexture;
        }

        public void Update(GameTime gameTime)
        {
            /* Do Nothing */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.cellCountX + 1; i++)
            {
                DrawLine(spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.xPosition + (i * cellWidth), this.yPosition),
                    new Vector2(this.xPosition + (i * cellWidth), this.yPosition + (cellCountY * cellHeight)));
            }
            for (int i = 0; i < this.cellCountY + 1; i++)
            {
                DrawLine(spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.xPosition, this.yPosition + (i * cellHeight)),
                    new Vector2(this.xPosition + (cellCountY * cellWidth), this.yPosition + (i * cellHeight)) );
            }
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

        public Point? GetGridPointFromMousePosition(Point mousePosition)
        {
            Rectangle gridRectangle = new Rectangle(xPosition, yPosition, cellWidth * cellCountX, cellHeight * cellCountY);
            if (gridRectangle.Contains(mousePosition))
            {
                Point gridPoint = new Point();
                gridPoint.X = mousePosition.X - gridRectangle.X;
                gridPoint.Y = mousePosition.Y - gridRectangle.Y;

                double xPos = gridPoint.X / (float)cellWidth;
                double yPos = gridPoint.Y / (float)cellHeight;

                //Round the floating values
                xPos = Math.Floor(xPos + 0.5f);
                yPos = Math.Floor(yPos + 0.5f);

                gridPoint.X = (int)xPos;
                gridPoint.Y = (int)yPos;

                
                if (gridPoint.X >= cellCountX || gridPoint.X < 0
                    || gridPoint.Y >= cellCountY || gridPoint.Y < 0)
                {
                    return null;
                }

                return gridPoint;
            }
            else
            {
                // We are not inside the grid
                return null;
            }
        }

        public Rectangle GetGridRectangleFromGridPoint(Point p)
        {
            return new Rectangle((p.X * cellWidth) + xPosition, (p.Y * cellHeight) + yPosition, cellWidth, cellHeight);
        }
    }
}
