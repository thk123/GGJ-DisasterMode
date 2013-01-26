using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GGJ_DisasterMode.Codebase.Dropoffs
{
    enum clockTypes
    {
        oneThree,
        twoThree,
        threeThree,
        oneTwo,
        twoTwo,
        oneOne,
        zero,
    }
    class ClockDrawer
    {
        static Dictionary<clockTypes, Texture2D> clocks;

        public static void LoadContent(ContentManager content)
        {
            clocks = new Dictionary<clockTypes, Texture2D>();
            foreach (clockTypes clockType in Enum.GetValues(typeof(clockTypes)))
            {
                string path = string.Format("Graphics//Clocks//{0}", GetPath(clockType));
                clocks.Add(clockType, content.Load<Texture2D>(path));
            }
        }

        public static Texture2D DrawClock(clockTypes clockType)
        {
            return clocks[clockType];
        }

        public static clockTypes GetClockType(int daysRemaining, int daysTotal)
        {
            //int deysElapsed = daysTotal - daysRemaining;
            switch (daysRemaining)
            {
                case 0:
                    return clockTypes.zero;

                case 3:
                    return clockTypes.threeThree;

                case 2:
                    switch (daysTotal)
                    {
                        case 2:
                            return clockTypes.twoTwo;

                        case 3:
                            return clockTypes.twoThree;
                    }
                    break;

                case 1:
                    switch (daysTotal)
                    {
                        case 1:
                            return clockTypes.oneOne;

                        case 2:
                            return clockTypes.oneTwo;

                        case 3:
                            return clockTypes.oneThree;
                    }
                    break;

            }

            throw new Exception("Unknown clock face");
        }

        private static string GetPath(clockTypes clockTypes)
        {
            switch (clockTypes)
            {
                case clockTypes.oneThree:
                    return "3_1";
                case clockTypes.twoThree:
                    return "3_2";
                case clockTypes.threeThree:
                    return "3_3";
                case clockTypes.oneTwo:
                    return "1_2";
                case clockTypes.twoTwo:
                    return "2_2";
                case clockTypes.oneOne:
                    return "1_1";
                case clockTypes.zero:
                    return "0";
                default:
                    throw new Exception("Unknown Clock");
            }
        }
    }
}
