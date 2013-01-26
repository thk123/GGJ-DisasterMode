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
        public int X
        {
            get;
            private set;
        }
        public int Y
        {
            get;
            private set;
        }
        public int CellWidth
        {
            get;
            private set;
        }
        public int CellHeight
        {
            get;
            private set;
        }
        public int CellCountX
        {
            get;
            private set;
        }
        public int CellCountY
        {
            get;
            private set;
        }

        private float lineWidth;
        private Texture2D lineTexture;
        private Color lineColor;

        public GameGrid(Texture2D lineTexture, int xPosition, int yPosition, int cellWidth, int cellHeight, int xCellCount, int yCellCount, float lineWidth, Color lineColor)

        {
            this.X = xPosition;
            this.Y = yPosition;
            this.CellWidth = cellWidth;
            this.CellHeight = cellHeight;           
            this.CellCountX = xCellCount;
            this.CellCountY = yCellCount;
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
            for (int i = 0; i < this.CellCountX + 1; i++)
            {
                DrawLine(spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.X + (i * CellWidth), this.Y),
                    new Vector2(this.X + (i * CellWidth), this.Y + (CellCountY * CellHeight)));
            }
            for (int i = 0; i < this.CellCountY + 1; i++)
            {
                DrawLine(spriteBatch, this.lineTexture, this.lineWidth, this.lineColor,
                    new Vector2(this.X, this.Y + (i * CellHeight)),
                    new Vector2(this.X + (CellCountY * CellWidth), this.Y + (i * CellHeight)) );
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
            Rectangle gridRectangle = new Rectangle(X, Y, CellWidth * CellCountX, CellHeight * CellCountY);
            if (gridRectangle.Contains(mousePosition))
            {
                Point gridPoint = new Point();
                gridPoint.X = mousePosition.X - gridRectangle.X;
                gridPoint.Y = mousePosition.Y - gridRectangle.Y;

                double xPos = gridPoint.X / (float)CellWidth;
                double yPos = gridPoint.Y / (float)CellHeight;

                //Round the floating values
                xPos = Math.Floor(xPos + 0.5f);
                yPos = Math.Floor(yPos + 0.5f);

                gridPoint.X = (int)xPos;
                gridPoint.Y = (int)yPos;

                
                if (gridPoint.X >= CellCountX || gridPoint.X < 0
                    || gridPoint.Y >= CellCountY || gridPoint.Y < 0)
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
            return new Rectangle((p.X * CellWidth) + X, (p.Y * CellHeight) + Y, CellWidth, CellHeight);
        }
    }
}
