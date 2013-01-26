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

        public bool hasWater()
        {
            return (this.Water != null);
        }

        public bool hasCleanWater()
        {
            return (this.Water != null) && (this.Water.IsClean());
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
