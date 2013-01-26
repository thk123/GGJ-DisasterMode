using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase.Characters
{
    struct CharacterClassProperties
    {
        public float thirstDecay;
        public float hungerDecay;
        public float hotTempMultiplier;
        public float coldTempMultiplier;
        public float healthDecay;

        public float thirstLevel;
        public float hungerLevel;
        public float hotTempLevel;
        public float coldTempLevel;
        public float healthLevel;

        public float trustLevel;
        public float trustMultiplier;
    }

    abstract class CharacterClass
    {
        const float ambientTemperatureRange = 10.0f;

        CharacterClassProperties characterProperties;
        public float CurrentThirst
        {
            get;
            private set;
        }

        public float CurrentHunger
        { 
            get;
            private set;
        }

        public float CurrentHotTemp
        {
            get;
            private set;
        }

        public float CurrentColdTemp
        {
            get;
            private set;
        }

        public float CurrentHealth
        {
            get;
            private set;
        }

        public float CurrentTrust
        {
            get;
            private set;
        }

        public CharacterClass(CharacterClassProperties characterProperties)
        {
            this.characterProperties = characterProperties;

            ResetLevelsToDefaultValues();
        }

        public void UpdateTemperature(float temperature)
        {
            if (Math.Abs(temperature) > ambientTemperatureRange)
            {
                if (temperature < 0)
                {
                    //affect the cold meter and reset the hot meter
                    CurrentColdTemp -= Math.Abs(temperature) * characterProperties.coldTempMultiplier;
                    CurrentHotTemp = characterProperties.hotTempLevel;
                }
                else
                {
                    CurrentHotTemp -= Math.Abs(temperature) * characterProperties.hotTempMultiplier;
                    CurrentColdTemp = characterProperties.coldTempLevel;
                }
            }
            else
            {
                //reset both temperatures as we have had a reasonable day
                CurrentColdTemp = characterProperties.coldTempLevel;
                CurrentHotTemp = characterProperties.hotTempLevel;
            }
        }

        public void ApplyTrustDelta(float trustDelta)
        {
            // are the trust multiplier and the trust delta pulling the same way
            // eg, if high trust delta (done something trustworthy) and high trust 
            // multiplier means big increase in trust
            float sign = trustDelta * characterProperties.trustMultiplier; 

            if (sign >= 0)
            {
                CurrentTrust += trustDelta * Math.Abs(characterProperties.trustMultiplier);
            }
            else
            {
                CurrentTrust += trustDelta * Math.Abs(1 / characterProperties.trustMultiplier);
            }            
        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        private void ResetLevelsToDefaultValues()
        {
            CurrentThirst = characterProperties.thirstLevel;
            CurrentHunger = characterProperties.hungerLevel;
            CurrentHotTemp = characterProperties.hotTempLevel;
            CurrentColdTemp = characterProperties.coldTempLevel;
            CurrentHealth = characterProperties.healthLevel;

            CurrentTrust = characterProperties.trustLevel;
        }
    }
}
