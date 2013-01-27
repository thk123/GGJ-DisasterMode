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
    public class ChildMaleCharacter : Civilian
    {
        public ChildMaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 1.1f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 1.3f;

            properties.healthVulnerability = 12.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 24.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 8.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 2.0f;

            properties.dotTexturePath = "Graphics//People//child"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//child_0";

            return properties;
        }
    }
    public class ChildFemaleCharacter : Civilian
    {
        public ChildFemaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 1.1f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 1.3f;

            properties.healthVulnerability = 12.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 24.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 8.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 2.0f;

            properties.dotTexturePath = "Graphics//People//child"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//child_1";

            return properties;
        }
    }

    public class AdultMaleCharacter : Civilian
    {
        public AdultMaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 0.75f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 0.6f;

            properties.healthVulnerability = 6.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 15.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 4.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 7.0f;

            properties.dotTexturePath = "Graphics//People//adult"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//adult_0";

            return properties;
        }
    }
    public class AdultFemaleCharacter : Civilian
    {
        public AdultFemaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 0.7f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 0.6f;

            properties.healthVulnerability = 6.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 15.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 4.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 7.0f;

            properties.dotTexturePath = "Graphics//People//adult"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//adult_1";

            return properties;
        }
    }

    public class OldMaleCharacter : Civilian
    {
        public OldMaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 1.5f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 1.0f;

            properties.healthVulnerability = 10.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 13.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 5.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 4.0f;

            properties.dotTexturePath = "Graphics//People//old"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//old_0";

            return properties;
        }
    }
    public class OldFemaleCharacter : Civilian
    {
        public OldFemaleCharacter(int xStart, int yStart)
            : base(GetProperties(), xStart, yStart)
        {

        }

        public static CivilianClassProperties GetProperties()
        {
            CivilianClassProperties properties = new CivilianClassProperties();
            properties.coldTempLevel = 100.0f;
            properties.coldTempMultiplier = 1.5f;

            properties.hotTempLevel = 100.0f;
            properties.hotTempMultiplier = 1.0f;

            properties.healthVulnerability = 10.0f;
            properties.healthLevel = 100.0f;

            properties.hungerDecay = 13.0f;
            properties.hungerLevel = 100.0f;

            properties.thirstDecay = 5.0f;
            properties.thirstLevel = 100.0f;

            properties.trustLevel = 100.0f;
            properties.trustMultiplier = 4.0f;

            properties.dotTexturePath = "Graphics//People//old"; //this is non-rebelious, maybe combine 3 pngs into one then just scroll along?
            properties.graphTexturePath = "Graphics//ListenPeople//old_1";

            return properties;
        }
    }
}
