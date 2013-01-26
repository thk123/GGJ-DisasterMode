using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase.Dropoffs
{
    class DropoffPropertiesFile
    {
        public static DropoffProperties GetPropertiesHealthLow(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Food_Low;

            properties.delay = 1;
            properties.duration = 1;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesHealthMedium(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Food_Medium;

            properties.delay = 2;
            properties.duration = 2;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }
    }
}
