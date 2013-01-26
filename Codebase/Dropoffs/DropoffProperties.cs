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
        public static DropoffProperties GetPropertiesFoodLow(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Food_Low;

            properties.delay = 1;
            properties.duration = 1;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_0");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_0");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesFoodMedium(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Food_Medium;

            properties.delay = 2;
            properties.duration = 2;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_1");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_1");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesFoodHigh(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Food_High;

            properties.delay = 3;
            properties.duration = 3;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_2");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_2");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//food_2");

            return properties;
        }

        public static DropoffProperties GetPropertiesHealthLow(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Health_Low;

            properties.delay = 1;
            properties.duration = 1;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_0");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_0");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_1");

            return properties;
        }

        public static DropoffProperties GetPropertiesHealthMedium(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Health_Medium;

            properties.delay = 2;
            properties.duration = 2;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_1");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_1");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesHealthHigh(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Health_High;

            properties.delay = 3;
            properties.duration = 3;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_2");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//health_2");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesTempLow(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Temperature_Low;

            properties.delay = 1;
            properties.duration = 1;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_0");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_0");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesTempMedium(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Temperature_Medium;

            properties.delay = 2;
            properties.duration = 2;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_1");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_1");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesTempHigh(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Temperature_High;

            properties.delay = 3;
            properties.duration = 3;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_2");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//temperature_2");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesWaterLow(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Water_Low;

            properties.delay = 1;
            properties.duration = 1;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//water_0");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//water_0");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesWaterMedium(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Water_Medium;

            properties.delay = 2;
            properties.duration = 2;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//Water_1");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//Water_1");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }

        public static DropoffProperties GetPropertiesWaterHigh(ContentManager content)
        {
            DropoffProperties properties = new DropoffProperties();

            properties.type = DropoffType.Dropoff_Water_High;

            properties.delay = 3;
            properties.duration = 3;
            properties.useCount = 1;//?

            properties.shopTexture = content.Load<Texture2D>("Graphics//Dropoffs//Water_2");
            properties.draggingTexture = content.Load<Texture2D>("Graphics//Dropoffs//Water_2");
            properties.placedTexture = content.Load<Texture2D>("Graphics//Dropoffs//Dropoff_FirstAid_Basic");

            return properties;
        }
    }
}
