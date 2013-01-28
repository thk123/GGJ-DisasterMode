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
        //private List<GameAction> actions;
        private List<Civilian> civilians;
        //private List<Dropoff> drops;


        public bool Meds
        {
            get;
            set;
        }
        public bool Food
        {
            get;
            set;
        }
        public bool Drink
        {
            get;
            set;
        }
        public bool Shelter
        {
            get;
            set;
        }

        public bool Water
        {
            get;
            set;
        }

        public Point? CleanWaterLocation
        {
            get;
            set;
        }

        public Point? FoodLocation
        {
            get;
            set;
        }

        public Point? MedsLocation
        {
            get;
            set;
        }

        public Point? ShelterLocation
        {
            get;
            set;
        }

        /*public Point? InstructionLocation
        {
            get
            {
                Point? p = null;
                hasInstruction(out p);
                return p;
            }
        }*/

        public ProcessingBucket()
        {
            //this.actions = new List<GameAction>();
            this.civilians = new List<Civilian>();
            //this.drops = new List<Dropoff>();
            this.Water = false;
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

        public void InformCiviliansNearestMeds(Vector2 nearestMeds)
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.SetNearestKnownHealthSource(nearestMeds);
            }
        }

        public void InformCiviliansNearestShelter(Vector2 nearestShelter)
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.SetNearestKnownTempSource(nearestShelter);
            }
        }

        public void InformCiviliansTarget(Vector2 target)
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.SetNearestInstructionDirection(target);
            }
        }

        public void InflictCiviliansHealthPenalty()
        {
            foreach (Civilian civilian in civilians)
            {
                civilian.ApplyHealthPenalty(civilians.Count);
            }
        }

        public List<Civilian> GetCivilians()
        {
            return civilians;
        }

        public bool hasWater()
        {
            return Water;
        }

        public void Clear()
        {
            civilians.Clear();

            Food = false;
            Water = false;
            Shelter = false;
            Meds = false;

            FoodLocation = null;
            CleanWaterLocation = null;
            ShelterLocation = null;
            MedsLocation = null;
        }

        public void ClearActions()
        {
            //actions.Clear();
        }

        public void addAction(GameAction action)
        {
            //actions.Add(action);
        }

        public void addDrop(Dropoff d)
        {
            if (d.IsAvaliable == false)
                return;

            if (this.Shelter == false)
            {
                this.Shelter = (d.DropoffType ==
DropoffType.Dropoff_Temperature_Low ||
                    d.DropoffType == DropoffType.Dropoff_Temperature_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Temperature_High);
                this.ShelterLocation = d.Position;
            }
            if (this.Meds == false)
            {
                this.Meds = (d.DropoffType == DropoffType.Dropoff_Health_Low ||
                    d.DropoffType == DropoffType.Dropoff_Health_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Health_High);
                this.MedsLocation = d.Position;
            }
            if (this.Water == false)
            {
                this.Water = (d.DropoffType == DropoffType.Dropoff_Water_Low ||
                    d.DropoffType == DropoffType.Dropoff_Water_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Water_High);
                this.CleanWaterLocation = d.Position;
            }
            if (this.Food == false)
            {
                this.Food = (d.DropoffType == DropoffType.Dropoff_Food_Low ||
                    d.DropoffType == DropoffType.Dropoff_Food_Medium ||
                    d.DropoffType == DropoffType.Dropoff_Food_High);
                this.FoodLocation = d.Position;
            }

            //drops.Add(drop);
        }

        public void addCivillian(Civilian civillian)
        {
            civilians.Add(civillian);
        }

        public void addWater(Water water)
        {
            this.Water = true;
            this.CleanWaterLocation = water.Position;
        }
    }
}