using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGJ_DisasterMode.Codebase.Characters;
using GGJ_DisasterMode.Codebase.Dropoffs;
using GGJ_DisasterMode.Codebase.Gameplay.Grid.Resources;
using GGJ_DisasterMode.Codebase.Screens;
using GGJ_DisasterMode.Codebase.Actions;

namespace GGJ_DisasterMode.Codebase.Gameplay.Grid
{
    class ProcessingBucket
    {
        private List<GameAction> actions;
        private List<Civilian> civilians;
        private List<Dropoff> drops;
        public Water Water
        {
            get;
            set;
        }

        public Point? CleanWaterLocation
        {
            get
            {
                Point? p = null;
                hasCleanWater(out p);
                return p;
            }
        }

        public Point? FoodLocation
        {
            get
            {
                Point? p = null;
                hasFood(out p);
                return p;
            }
        }

        public ProcessingBucket()
        {
            this.actions = new List<GameAction>();
            this.civilians = new List<Civilian>();
            this.drops = new List<Dropoff>();
            this.Water = null;
        }

        public void InformCiviliansNearestWater(Vector2 nearestWater)
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.SetNearestKnownWaterSource(nearestWater);
            }
        }

        public void InformCiviliansNearestFood(Vector2 nearestFood)
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.SetNearestKnownFoodSource(nearestFood);
            }
        }

        public List<Civilian> GetCivilians()
        {
            return civilians;
        }

        public bool hasWater()
        {
            return (this.Water != null);
        }

        private bool hasCleanWater(out Point? location)
        {
            location = null;
            if((this.Water != null) && (this.Water.IsClean()))
            {
                location = this.Water.Position;
                return true;
            }

            foreach (Dropoff d in drops.Where(drop => drop.IsAvaliable))
            {
                if (d.DropoffType == DropoffType.Dropoff_Water_Low ||
                    d.DropoffType == DropoffType.Dropoff_Water_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Water_High)
                {
                    location = d.Position;
                    return true;
                }
            }
            location = null;
            return false;
        }

        private bool hasFood(out Point? location)
        {
            location = null;
            foreach (Dropoff d in drops.Where(drop => drop.IsAvaliable))
            {
                if (d.DropoffType == DropoffType.Dropoff_Food_Low ||
                    d.DropoffType == DropoffType.Dropoff_Food_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Food_High)
                {
                    location = d.Position;
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            civilians.Clear();
            ClearExpiredDrops();
        }

        public void ClearActions()
        {
            actions.Clear();
        }

        private void ClearExpiredDrops()
        {
            Dropoff[] tempDrops = drops.ToArray();
            drops.Clear();
            for (int i = 0; i < tempDrops.GetLength(0); i++)
            {
                if (tempDrops[i].CurrentState != DropoffState.Decayed)
                {
                    drops.Add(tempDrops[i]);
                }
            }
        }

        public void addAction(GameAction action)
        {
            GameAction[] tempActions = actions.ToArray();
            actions.Clear();
            for (int i = 0; i < tempActions.GetLength(0); i++)
            {
                if (tempActions[i].ActionState != ActionState.Inactive)
                {
                    actions.Add(tempActions[i]);
                }
            }
        }

        public void addDrop(Dropoff drop)
        {
            drops.Add(drop);
        }

        public void addCivillian(Civilian civillian)
        {
            civilians.Add(civillian);
        }

        public void addWater(Water water)
        {
            this.Water = water;
        }
    }
}
