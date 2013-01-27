using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GGJ_DisasterMode.Codebase.Characters.Decision;

namespace GGJ_DisasterMode.Codebase.Characters
{
    public abstract class Civilian
    {
        private static Random RANDOM = new Random(1000);
        
        const float ambientTemperatureRange = 10.0f;

        public const int SCALE_FACTOR = 1800;

        private Vector2 origin;
        private Vector2 goal;
        private Vector2 currentPosition;

        private static Texture2D gravestone = null;

        public Vector2? NearestKnownWaterSource
        {
            get;
            private set;
        }

        public Vector2? NearestKnownFoodSource
        {
            get;
            private set;
        }

        public Vector2? NearestKnownHealthSource
        {
            get;
            private set;
        }

        public Vector2? NearestKnownTempSource
        {
            get;
            private set;
        }

        public Vector2? NearestInstructionDirection
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

        public void SetNearestKnownFoodSource(Vector2 foodScreenLocation)
        {
            this.NearestKnownFoodSource =
                new Vector2(((foodScreenLocation.X - 14) / 0.31166f),
                ((foodScreenLocation.Y - 14) / 0.30166f));
            this.color = Color.Red;
        }

        public void SetNearestKnownHealthSource(Vector2 healthScreenLocation)
        {
            this.NearestKnownHealthSource =
                new Vector2(((healthScreenLocation.X - 14) / 0.31166f),
                ((healthScreenLocation.Y - 14) / 0.30166f));
            this.color = Color.Red;
        }

        public void SetNearestKnownTempSource(Vector2 tempScreenLocation)
        {
            this.NearestKnownTempSource =
                new Vector2(((tempScreenLocation.X - 14) / 0.31166f),
                ((tempScreenLocation.Y - 14) / 0.30166f));
            this.color = Color.Red;
        }

        public void SetNearestInstructionDirection(Vector2 instructionDestinationScreenLocation)
        {
            this.NearestInstructionDirection =
                new Vector2(((instructionDestinationScreenLocation.X - 14) / 0.31166f),
                ((instructionDestinationScreenLocation.Y - 14) / 0.30166f));
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
        public Texture2D GraphTexture
        {
            get;
            private set;
        }
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

        bool isDead;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
            }
        }

        Behaviour CurrentBehaviour
        {
            get
            {
                if (CurrentTrust <= 55.0f)
                {
                    return Behaviour.Rebellious;
                }
                else
                {
                    return Behaviour.Conforming;
                }
            }
        }

        public Civilian(CivilianClassProperties characterProperties, int startX, int startY)
        {
            this.characterProperties = characterProperties;

            this.currentPosition = new Vector2(startX, startY);
            

            this.color = Color.Black;

            this.NearestKnownWaterSource = null;

            ResetLevelsToDefaultValues();
            this.goal = DecisionProcessing.RandomGoal(currentPosition);
        }

        public void LoadContent(ContentManager content)
        {
            this.civilianTexture = content.Load<Texture2D>(characterProperties.dotTexturePath);
            this.GraphTexture = content.Load<Texture2D>(characterProperties.graphTexturePath);

            if (gravestone == null)
            {
                gravestone = content.Load<Texture2D>("Graphics//People//tombstone");
            }
        }

        public void ProcessDay()
        {
            CurrentHunger -= characterProperties.hungerDecay;
            CurrentThirst -= characterProperties.thirstDecay;

            if (CurrentHunger <= 0 ||
                CurrentThirst <= 0 ||
                CurrentHealth <= 0 ||
                CurrentColdTemp <= 0 ||
                CurrentHotTemp <= 0)
            {
                IsDead = true;
            }
        }

        public void UpdateTemperature(float temperature)
        {
            if (Math.Abs(temperature) > TemperatureManager.GetAmbientTemperatureRange())
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

        public void ApplyHealthPenalty(float penalty)
        {
            CurrentHealth -= penalty * characterProperties.healthVulnerability;
        }


        public void Update(GameTime gameTime)
        {
            if (IsDead)
                return;

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

            if ((this.NearestKnownWaterSource.HasValue) &&
                (Vector2.DistanceSquared(currentPosition,
NearestKnownWaterSource.Value) < 30000)
                && CurrentThirst < 100.0f)
            {
                CurrentThirst += 0.05f;
            }
            if ((this.NearestKnownTempSource.HasValue) &&
                (Vector2.DistanceSquared(currentPosition, NearestKnownTempSource.Value) < 30000))
            {
                if (CurrentHotTemp < 100.0f) { CurrentHotTemp += 5.0f; }
                if (CurrentColdTemp < 100.0f) { CurrentColdTemp += 5.0f; }
            }
            if ((this.NearestKnownHealthSource.HasValue) &&
                (Vector2.DistanceSquared(currentPosition, NearestKnownHealthSource.Value) < 30000)
                && CurrentHealth < 100.0f)
            {
                CurrentHealth += 0.05f;
            }
            if ((this.NearestKnownFoodSource.HasValue) &&
                (Vector2.DistanceSquared(currentPosition,
NearestKnownWaterSource.Value) < 30000)
                && CurrentHunger < 100.0f)
            {
                CurrentHunger += 0.05f;
            }

            Needs CurrentNeeds = new Needs();
            CurrentNeeds.Cold = CurrentColdTemp;
            CurrentNeeds.Health = CurrentHealth;
            CurrentNeeds.Hot = CurrentHotTemp;
            CurrentNeeds.Thirst = CurrentThirst;
            CurrentNeeds.Hunger = CurrentHunger;

            bool HasUrgentNeeds = CurrentNeeds.AreUrgent();

            float speedMultiplier = 0.0f;
            if (!HasUrgentNeeds && (Vector2.DistanceSquared(currentPosition, goal) > 64000))
            {
                goal = DecisionProcessing.RandomGoal(currentPosition);
            }
            else
            {

                KnowledgeModel Knowledge = new KnowledgeModel();
                Knowledge.ClosestWater =
                    (NearestKnownWaterSource.HasValue) ? NearestKnownWaterSource.Value : (Vector2?)null;
                Knowledge.ClostestFood =
                    (NearestKnownFoodSource.HasValue) ? NearestKnownFoodSource.Value : (Vector2?)null;
                Knowledge.ClosestMedicine =
                    (NearestKnownHealthSource.HasValue) ? NearestKnownHealthSource.Value : (Vector2?)null;
                Knowledge.ClosestShelter =
                    (NearestKnownTempSource.HasValue) ? NearestKnownTempSource.Value : (Vector2?)null;
                Knowledge.ClosestGameAction =
                    (NearestInstructionDirection.HasValue) ? NearestInstructionDirection.Value : (Vector2?)null;

                goal = DecisionProcessing.Run(currentPosition, Knowledge, CurrentNeeds, CurrentBehaviour);
                speedMultiplier = 3.0f;
            }

            Vector2 direction = goal - currentPosition;

            if ((Vector2.DistanceSquared(goal, currentPosition) > 2.5))
            {
                direction.Normalize();
                currentPosition += (direction * speedMultiplier);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            if (!IsDead)
            {
                Rectangle sourceRectangle;
                if (CurrentBehaviour == Behaviour.Rebellious)
                {
                    sourceRectangle = new Rectangle(30, 0, 15, 15);
                }
                else
                {
                    sourceRectangle = new Rectangle(0, 0, 15, 15);
                }

                spriteBatch.Draw(this.civilianTexture, new Vector2(14.0f + (this.currentPosition.X * 0.31166f),
                        14.0f + (this.currentPosition.Y * 0.30166f)),
                        sourceRectangle, Color.White, 0.0f, new Vector2(), 1.0f, SpriteEffects.None, 1.0f);
            }
            else
            {
                spriteBatch.Draw(gravestone, new Vector2(14.0f + (this.currentPosition.X * 0.31166f),
                        14.0f + (this.currentPosition.Y * 0.30166f)),
                        null, Color.White, 0.0f, new Vector2(), 1.0f, SpriteEffects.None, 1.0f);
            }
        }

        static Random random = new Random();

        private void ResetLevelsToDefaultValues()
        {
            CurrentThirst = characterProperties.thirstLevel;
            CurrentHunger = characterProperties.hungerLevel;
            CurrentHotTemp = characterProperties.hotTempLevel;
            CurrentColdTemp = characterProperties.coldTempLevel;
            CurrentHealth = characterProperties.healthLevel;

            CurrentTrust = characterProperties.trustLevel - random.Next(50);
            Console.WriteLine(CurrentTrust);
        }
    }

    public struct CivilianClassProperties
    {
        public float thirstDecay;
        public float hungerDecay;
        public float hotTempMultiplier;
        public float coldTempMultiplier;
        public float healthVulnerability;

        public float thirstLevel;
        public float hungerLevel;
        public float hotTempLevel;
        public float coldTempLevel;
        public float healthLevel;

        public float trustLevel;
        public float trustMultiplier;

        public string dotTexturePath;
        public string graphTexturePath;
    }
}
