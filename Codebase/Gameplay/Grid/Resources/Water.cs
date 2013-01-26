using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGJ_DisasterMode.Codebase.Characters;
using GGJ_DisasterMode.Codebase.Dropoffs;
using GGJ_DisasterMode.Codebase.Gameplay.Grid.Resources;
using GGJ_DisasterMode.Codebase.Screens;
using GGJ_DisasterMode.Codebase.Actions;

namespace GGJ_DisasterMode.Codebase.Gameplay.Grid.Resources
{
    class Water
    {
        public Point Position
        {
            set;
            get;
        }

        public Water(int x, int y)
        {
            this.Position = new Point(x, y);
        }
        
        public bool IsClean()
        {
            return true;
        }

    }
}
