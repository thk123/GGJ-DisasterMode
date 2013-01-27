using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase
{
    static class ColorAdapter
    {
        public static Color getTransparentColor(Color colour, float a)
        {
            return new Color(colour,a);
        }
    }
}
