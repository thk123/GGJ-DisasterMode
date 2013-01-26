using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ_DisasterMode.Codebase.Characters
{
    public class ChildCharacter : Civilian
    {
        public ChildCharacter(int xStart, int yStart, Texture2D texture)
            : base(GetProperties(), xStart, yStart, texture)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 1.0f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 1.0f;

            properties.healthDecay = 10.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 15.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 5.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 5.0f;

            return properties;
        }
    }
}
