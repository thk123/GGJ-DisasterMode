using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase.Characters.Decision
{
    public enum Priority
    {
        Water, Food, Shelter, Medicine, None
    }
    
    public struct KnowledgeModel
    {
        public Vector2? ClosestWater;
        public Vector2? ClostestFood;
        public Vector2? ClosestMedicine;
        public Vector2? ClosestShelter;
        public Vector2? ClosestGameAction;
    }

    public struct Needs
    {
        public float Thirst;
        public float Hunger;
        public float Cold;
        public float Hot;
        public float Health;

        public bool AreUrgent()
        {
            return (Thirst < 25.0f) || (Hunger < 25.0f) ||
                (Cold < 25.0f) || (Hot < 25.0f) || (Health < 25.0f);
        }

        public void Eliminate(Priority CurrentPriority)
        {
            switch (CurrentPriority)
            {
                case (Priority.Medicine):
                    {
                        this.Health = 100.0f;
                        break;
                    }
                case (Priority.Food):
                    {
                        this.Hunger = 100.0f;
                        break;
                    }
                case (Priority.Shelter):
                    {
                        this.Hot = 100.0f;
                        this.Cold = 100.0f;
                        break;
                    }
                case (Priority.Water):
                    {
                        this.Thirst = 100.0f;
                        break;
                    }
            }
        }
    }

    public enum Behaviour
    {
        Conforming, Rebellious
    }
    
    static class DecisionProcessing
    {
        private static Random RANDOM = new Random(1000);
        
        public static Vector2 Run(Vector2 Position, KnowledgeModel Knowledge,
            Needs Needs, Behaviour Behaviour)
        {
            
            if (Knowledge.ClosestGameAction.HasValue &&
                Behaviour == Behaviour.Conforming)
            {
                return (Vector2) Knowledge.ClosestGameAction;
            }
            
            bool IsSatisfied = false;
            Priority myPriority = GetPriority(Needs);
            for (int i = 1; (i < 5) && (IsSatisfied == false); i++)
            {
                if (CanBeSatisfied(Position, myPriority, Knowledge) == true)
                {
                    IsSatisfied = true;
                    break;
                }
                Needs.Eliminate(myPriority);
                myPriority = GetPriority(Needs);
            }
            if (IsSatisfied == false)
            {
                myPriority = Priority.None;
            }

            return RunPriority(myPriority, Knowledge, Position);
        }

        private static bool CanBeSatisfied(Vector2 CurrentPosition, Priority CurrentPriority, 
            KnowledgeModel CurrentKnowledge)
        {
            const int DISTANCE = 25000;
            
            switch (CurrentPriority)
            {
                case (Priority.None):
                    {
                        return true;
                        break;
                    }
                case (Priority.Medicine):
                    {
                        return (CurrentKnowledge.ClosestMedicine.HasValue
                            && (Vector2.DistanceSquared(CurrentPosition,
                            CurrentKnowledge.ClosestMedicine.Value) < DISTANCE)) ;
                    }
                case (Priority.Food):
                    {
                        return (CurrentKnowledge.ClostestFood.HasValue
                             && (Vector2.DistanceSquared(CurrentPosition,
                             CurrentKnowledge.ClostestFood.Value) < DISTANCE));
                    }
                case (Priority.Shelter):
                    {
                        return (CurrentKnowledge.ClosestShelter.HasValue
                             && (Vector2.DistanceSquared(CurrentPosition,
                             CurrentKnowledge.ClosestShelter.Value) < DISTANCE));
                    }
                case (Priority.Water):
                    {
                        return (CurrentKnowledge.ClosestWater.HasValue
                             && (Vector2.DistanceSquared(CurrentPosition,
                             CurrentKnowledge.ClosestWater.Value) < DISTANCE));
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private static Vector2 RunPriority(Priority CurrentPriority, KnowledgeModel CurrentKnowledge,
            Vector2 CurrentPosition)
        {
            switch (CurrentPriority)
            {
                case (Priority.Medicine):
                    {
                        return CurrentKnowledge.ClosestMedicine.Value;
                    }
                case (Priority.Food):
                    {
                        return CurrentKnowledge.ClostestFood.Value;
                    }
                case (Priority.Shelter):
                    {
                        return CurrentKnowledge.ClosestShelter.Value;
                    }
                case (Priority.Water):
                    {
                        return CurrentKnowledge.ClosestWater.Value;
                    }
                default:
                    {
                        return RandomGoal(CurrentPosition);
                    }
            }
        }

        public static Vector2 RandomGoal(Vector2 CurrentPosition)
        {
            const int MIN = 10;
            const int MAX = 250;

            float xGoal, yGoal;
            int direction = RANDOM.Next(4);

            if (direction == 0)
            {
                xGoal = CurrentPosition.X + RANDOM.Next(MIN, MAX);
                yGoal = CurrentPosition.Y + RANDOM.Next(MIN, MAX);
            }
            else if (direction == 1)
            {
                xGoal = CurrentPosition.X - RANDOM.Next(MIN, MAX);
                yGoal = CurrentPosition.Y - RANDOM.Next(MIN, MAX);
            }
            else if (direction == 2)
            {
                xGoal = CurrentPosition.X - RANDOM.Next(MIN, MAX);
                yGoal = CurrentPosition.Y + RANDOM.Next(MIN, MAX);
            }
            else
            {
                xGoal = CurrentPosition.X + RANDOM.Next(MIN, MAX);
                yGoal = CurrentPosition.Y - RANDOM.Next(MIN, MAX);
            }
            return new Vector2(xGoal, yGoal);
        }

        private static Priority GetPriority(Needs CurrentNeeds)
        {
            float thirst = CurrentNeeds.Thirst;
            float hunger = CurrentNeeds.Hunger;
            float cold = CurrentNeeds.Cold;
            float hot = CurrentNeeds.Hot;
            float health = CurrentNeeds.Health;
            
            if ((thirst > 50.0f) && (hunger > 50.0f) && (cold > 50.0f)
                && (hot > 50.0f) && (health > 50.0f))
            {
                return Priority.None;
            }
            else if (thirst < 10.0f)
            {
                return Priority.Water;
            }
            else if (health < 15.0f)
            {
                return Priority.Medicine;
            }
            else if ( (hot < 15.0f) || (cold < 15.0f) )
            {
                return Priority.Shelter;
            }
            else if (hunger < 10.0f)
            {
                return Priority.Food;
            }
            else if ((thirst > 30.0f) && (hunger > 30.0f) && (cold > 30.0f)
                && (hot > 30.0f) && (health < 30.0f))
            {
                return Priority.Medicine;
            }
            else if ((thirst > 40.0f) && (hunger > 40.0f) && ((cold < 50.0f)
                || (hot < 50.0f)) && (health > 40.0f))
            {
                return Priority.Shelter;
            }
            else if ((thirst < 20.0f) && (hunger > 50.0f) && ((cold > 40.0f)
                || (hot > 40.0f)) && (health > 50.0f))
            {
                return Priority.Water;
            }
            else if ((thirst > 50.0f) && (hunger > 50.0f) && (cold > 50.0f)
                && (hot > 50.0f) && (health < 50.0f))
            {
                return Priority.Medicine;
            }
            else
            {
                return Priority.None;
            }
        }
    }
}
