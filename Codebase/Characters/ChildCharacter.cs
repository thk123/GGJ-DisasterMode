using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGJ_DisasterMode.Codebase.Characters
{
    class ChildCharacter : CharacterClass
    {
        public ChildCharacter()
            : base(GetProperties())
        {

        }

        private static CharacterClassProperties GetProperties()
        {
            CharacterClassProperties properties = new CharacterClassProperties();
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
