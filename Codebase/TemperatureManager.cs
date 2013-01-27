using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GGJ_DisasterMode.Codebase
{
    static class TemperatureManager
    {
        const float ambientTemperatureRange = 10.0f;
        const float chanceOfIntenseHeat = 0.05f;
        const float chanceOfIntenseCold = 0.05f;

        const int standardTemperatureFlux = 5;
        const int extremeFlux = 15;

        static Random weather = new Random();

        static Texture2D diamondTexture;

        public static float Temperature
        {
            get;
            private set;

        }

        public static void LoadTempManager(ContentManager content)
        {
            diamondTexture = content.Load<Texture2D>("Graphics//GUIElements//temperatureGauge");
        }

        public static void ProcessDay()
        {
            double weatherOutcome = weather.NextDouble();

            if (weatherOutcome < chanceOfIntenseCold)
            {
                //Intense cold!!
                Temperature -= extremeFlux;
            }
            else if (weatherOutcome + chanceOfIntenseCold < chanceOfIntenseHeat)
            {
                //Intense heat!!
                Temperature += extremeFlux;
            }
            else
            {
                Temperature += weather.Next(-standardTemperatureFlux, standardTemperatureFlux);
            }

            Console.WriteLine(Temperature);
        }

        public static float GetAmbientTemperatureRange()
        {
            return 10.0f;
        }

        public static void DrawDiamond(SpriteBatch spriteBatch, float uiOffset)
        {
            spriteBatch.Draw(diamondTexture, new Vector2(uiOffset + 392 + 9 - (diamondTexture.Width / 2.0f), 59 + 200 - (Temperature*4)), Color.White);
        }
    }
}
