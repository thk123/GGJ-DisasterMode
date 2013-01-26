using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static float Temperature
        {
            get;
            private set;

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

        static float GetAmbientTemperatureRange()
        {
            return 10.0f;
        }
    }
}
