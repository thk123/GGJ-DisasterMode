using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GGJ_DisasterMode.Codebase
{
    class Grid
    {
        Rectangle gridRectangle;
        float gridSize;

        public Grid(Rectangle drawRectangle, float individualGridSize)
        {
            gridRectangle = drawRectangle;
            gridSize = individualGridSize;
        }

        public Point? GetGridPointFromMousePosition(Point mousePosition)
        {
            if (gridRectangle.Contains(mousePosition))
            {
                Point gridPoint = new Point();
                gridPoint.X = mousePosition.X - gridRectangle.X;
                gridPoint.Y = mousePosition.Y - gridRectangle.Y;

                double xPos = gridPoint.X / (float)gridSize;
                double yPos = gridPoint.Y / (float)gridSize;

                //Round the floating values
                xPos = Math.Floor(xPos + 0.5f);
                yPos = Math.Floor(yPos + 0.5f);

                gridPoint.X = (int)xPos;
                gridPoint.Y = (int)yPos;

                return gridPoint;
            }
            else
            {
                return null;
            }
        }

    }
}
