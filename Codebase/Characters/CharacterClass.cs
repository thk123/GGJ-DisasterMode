using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase.Characters
{
    public abstract class Civilian
    {
        const float ambientTemperatureRange = 10.0f;

        public const int SCALE_FACTOR = 1800;

        private Vector2 origin;
        private Vector2 goal;
        private Vector2 currentPosition;

        private int currentTimeStep;
        private int maxTimeStep;


        private Texture2D civilianTexture;
        private Color color;

        CivilianClassProperties characterProperties;
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

        public Civilian(CivilianClassProperties characterProperties, int startX, int startY, Texture2D texture)
        {
            this.characterProperties = characterProperties;

            this.currentPosition = new Vector2(startX, startY);
            this.civilianTexture = texture;

            this.color = Color.Black;

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

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Draw(this.civilianTexture, new Vector2(14.0f + (this.currentPosition.X * 0.31166f),
                    14.0f + (this.currentPosition.Y * 0.30166f)), 
                    null, this.color, 0.0f, new Vector2(), 5.0f, SpriteEffects.None, 1.0f);
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

    public struct CivilianClassProperties
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
}
