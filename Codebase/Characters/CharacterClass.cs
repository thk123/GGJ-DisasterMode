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
        public static Random RANDOM = new Random(1000);

        public const int SCALE_FACTOR = 1800;

        private Vector2 origin;
        private Vector2 goal;
        private Vector2 currentPosition;

        public Vector2? NearestKnownWaterSource
        {
            get;
            private set;
        }

        public void SetNearestKnownWaterSource(Vector2 waterScreenLocation)
        {
            this.NearestKnownWaterSource = 
                new Vector2( ((waterScreenLocation.X - 14) / 0.31166f),
                ((waterScreenLocation.Y - 14) / 0.30166f) );
            this.color = Color.Red;
        }


        public int X
        {
            get
            {
                return (int) currentPosition.X;
            }
        }

        public int Y
        {
            get
            {
                return (int) currentPosition.Y;
            }
        }

        public int DrawableX
        {
            get
            {
                double floorVal = 14.0 + Math.Floor(currentPosition.X * 0.31166);

                
                return (int) floorVal;
            }
        }

        public int DrawableY
        {
            get
            {
                double floorVal = 14.0 + Math.Floor(currentPosition.Y * 0.30166);
                return (int) floorVal;
            }
        }

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

            this.NearestKnownWaterSource = null;

            ResetLevelsToDefaultValues();
            setRandomGoal();
        }

        public void setRandomGoal()
        {
            const int MIN = 10;
            const int MAX = 250;
            
            float xGoal, yGoal;
            int direction = RANDOM.Next(4);

            if (direction == 0)
            {
                xGoal = currentPosition.X + RANDOM.Next(MIN, MAX);
                yGoal = currentPosition.Y + RANDOM.Next(MIN, MAX);
            }
            else if (direction == 1)
            {
                xGoal = currentPosition.X - RANDOM.Next(MIN, MAX);
                yGoal = currentPosition.Y - RANDOM.Next(MIN, MAX);
            }
            else if (direction == 2)
            {
                xGoal = currentPosition.X - RANDOM.Next(MIN, MAX);
                yGoal = currentPosition.Y + RANDOM.Next(MIN, MAX);
            }
            else
            {
                xGoal = currentPosition.X + RANDOM.Next(MIN, MAX);
                yGoal = currentPosition.Y - RANDOM.Next(MIN, MAX);
            }
            this.goal = new Vector2(xGoal, yGoal); 
            
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
            if (currentPosition.X < 0)
            {
                currentPosition.X += SCALE_FACTOR;
            }
            else if (currentPosition.X > SCALE_FACTOR)
            {
                currentPosition.X -= SCALE_FACTOR;
            }
            if (currentPosition.Y < 0)
            {
                currentPosition.Y += SCALE_FACTOR;
            }
            else if (currentPosition.Y > SCALE_FACTOR)
            {
                currentPosition.Y -= SCALE_FACTOR;
            }


            if (Vector2.DistanceSquared(currentPosition, goal) > 600)
            {
                setRandomGoal();
            }

            if (NearestKnownWaterSource.HasValue && 
                (Vector2.DistanceSquared(currentPosition, NearestKnownWaterSource.Value) > 24000) )
            {
                goal = NearestKnownWaterSource.Value;
            }
            else if ( (RANDOM.Next(100) < 5) && (Math.Abs(currentPosition.X - goal.X) < 2) && (Math.Abs(currentPosition.Y - goal.Y) < 2))
            {
                setRandomGoal();
            }

            Vector2 direction = goal - currentPosition;

            if ( (Vector2.DistanceSquared(goal, currentPosition) > 2.5) )
            {
                direction.Normalize();
                currentPosition += direction;
            }
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
