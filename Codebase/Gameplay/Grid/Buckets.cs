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
    class Buckets
    {
        private GameGrid grid;
        private List<GameAction> actions;
        private List<Civilian> civilians;
        private List<Dropoff> drops;
        private ProcessingBucket[,] buckets;

        public Buckets(GameGrid grid, List<GameAction> actions, List<Civilian> civilians, List<Dropoff> drops, List<Water> naturals)
        {
            this.grid = grid;
            this.buckets = new ProcessingBucket[grid.CellCountX,grid.CellCountY];
            for (int i = 0; i < buckets.GetLength(0); i++)
            {
                for (int j = 0; j < buckets.GetLength(1); j++)
                {
                    this.buckets[i, j] = new ProcessingBucket();
                }
            }


            this.actions = actions;
            this.civilians = civilians;
            this.drops = drops;
        }

        private bool errorCheck(int x, int y)
        {
            if ((x >= grid.CellCountX) || (y >= grid.CellCountY)) { throw new Exception("no such bucket:" + x + ", " + y); }

            return false;
        }

        public bool hasSafeWater(int x, int y)
        {
            errorCheck(x, y);
            return false;
        }

        public bool hasWater(int x, int y)
        {
            errorCheck(x, y);
            return false;
        }

        public void addWater(int xCoordinate, int yCoordinate)
        {
            Point? nullBucket = getBucketID(xCoordinate, yCoordinate);
            if (nullBucket == null) return;
            Point bucket = (Point)nullBucket;
            this.buckets[bucket.X, bucket.Y].addWater(new Water(xCoordinate, yCoordinate));

        }

        private Point? getBucketID(int x, int y)
        {
            return grid.GetGridPointFromMousePosition(new Point(x,y));
        }

        public ProcessingBucket[] getNeighbours(int x, int y)
        {
            ProcessingBucket[] neighbours;
            
            if ((x == 0) & (y == 0))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y+1], 
                    buckets[x+1,y], 
                    buckets[x+1,y+1],              
                };
            }
            else if ( ( x == 0 )  && ( y == 17 ) )
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y-1], 
                    buckets[x+1,y], 
                    buckets[x+1,y-1],              
                };
            }
            else if ((x == 17) && (y == 0))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y+1], 
                    buckets[x-1,y], 
                    buckets[x-1,y+1],              
                };
            }
            else if ((x == 17) && (y == 17))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y-1], 
                    buckets[x-1,y], 
                    buckets[x-1,y-1],              
                };
            }
            else if ((x > 0) && (x < 17) && (y == 0))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y+1],
                    buckets[x+1,y], 
                    buckets[x+1,y+1], 
                    buckets[x-1,y], 
                    buckets[x-1,y+1],              
                };
            }
            else if ((x > 0) && (x < 17) && (y == 17))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y-1],
                    buckets[x+1,y], 
                    buckets[x+1,y-1], 
                    buckets[x-1,y], 
                    buckets[x-1,y-1],              
                };
            }
            else if ((y > 0) && (y < 17) && (x == 0))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x+1,y],
                    buckets[x,y+1], 
                    buckets[x+1,y+1], 
                    buckets[x,y-1], 
                    buckets[x+1,y-1],              
                };
            }
            else if ((y > 0) && (y < 17) && (x == 17))
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x-1,y],
                    buckets[x,y+1], 
                    buckets[x-1,y+1], 
                    buckets[x,y-1], 
                    buckets[x-1,y-1],              
                };
            }
            else
            {
                return new ProcessingBucket[] 
                { 
                    buckets[x,y], 
                    buckets[x,y+1], 
                    buckets[x,y-1], 
                    buckets[x+1,y], 
                    buckets[x+1,y+1],
                    buckets[x+1,y-1],
                    buckets[x-1,y], 
                    buckets[x-1,y+1], 
                    buckets[x-1,y-1]                
                };
            }
        }


        public void addNewAction(GameAction action, int xCoordinate, int yCoordinate)
        {
            /*Point? nullBucket = getBucketID(xCoordinate, yCoordinate);
            if (nullBucket == null) return;
            Point bucket = (Point)nullBucket;*/
            if (action.ActionType == ActionType.DirectAction)
            {
                this.actions.Add(action);
                this.buckets[xCoordinate, yCoordinate].addAction(action);

                ProcessingBucket[] localArea = getNeighbours(xCoordinate, yCoordinate);

                foreach (ProcessingBucket adjacentBucked in localArea)
                {
                    Rectangle targetPoint = grid.GetGridRectangleFromGridPoint(action.ActionDirection.Value);
                    adjacentBucked.InformCiviliansTarget(
                        new Vector2(targetPoint.Center.X, targetPoint.Center.Y)); ;
                }
            }

         
        }

        public void addNewDrop(Dropoffs.Dropoff drop, int xCoordinate, int yCoordinate)
        {
            Point? nullBucket = getBucketID(xCoordinate, yCoordinate);
            if (nullBucket == null) return;
            Point bucket = (Point)nullBucket;

            this.drops.Add(drop);
            this.buckets[bucket.X, bucket.Y].addDrop(drop);
        }

        public void addNewCivilian(Civilian civilian)
        {
            Point civilianPosition = 
                new Point(civilian.DrawableX, civilian.DrawableY);

            if (this.grid.GetGridPointFromMousePosition(civilianPosition, true).HasValue)
            {
                civilians.Add(civilian);
            }
            else
            {
                throw new Exception("invalid civilian position");
            }
        }

        public void PopulateBuckets()
        {
            //20ms
            ClearBuckets();

            //20ms
            foreach (Civilian civilian in civilians)
            {
                Point civilianPosition = new Point(civilian.DrawableX, civilian.DrawableY);
                Point? val = this.grid.GetGridPointFromMousePosition(civilianPosition, true);
                if(val.HasValue)
                {
                    Point gridPosition = val.Value;
                    buckets[gridPosition.X, gridPosition.Y].addCivillian(civilian);
                }
                else
                {
                    throw new Exception();
                }
            }

            foreach (GameAction action in actions)
            {
                if (action.ActionState == ActionState.Active)
                {
                    Point gridPosition = (Point) action.ActionPosition;
                    buckets[gridPosition.X, gridPosition.Y].addAction(action);
                }
            }

            foreach (Dropoff drop in drops)
            {
                if (drop.CurrentState != DropoffState.Decayed)
                {
                    Point position = (Point)drop.Position;
                    Point gridPosition = (Point) this.grid.GetGridPointFromMousePosition(position);
                    buckets[gridPosition.X, gridPosition.Y].addDrop(drop);
                }
            }

            ProcessCivilianKnowledgeModel();
        }

        public void ProcessCivilianKnowledgeModel()
        {
            for (int i = 0; i < buckets.GetLength(0); i++)
            {
                for (int j = 0; j < buckets.GetLength(1); j++)
                {
                    foreach (Civilian civ in buckets[i, j].GetCivilians())
                    {
                        civ.ResetKnowledge();
                    }
                    
                    Point? cleanWaterPosition = buckets[i, j].CleanWaterLocation;
                    Point? foodPosition = buckets[i, j].FoodLocation;
                    Point? medsPosition = buckets[i, j].MedsLocation;
                    Point? shelterPosition = buckets[i, j].ShelterLocation;
                    //Point? instructionDestination = buckets[i, j].InstructionLocation;
                    
                    

                    ProcessingBucket[] localArea = getNeighbours(i, j);
                    for (int k = 1; k < localArea.GetLength(0); k++)
                    {
                        if (cleanWaterPosition != null)
                        {
                            localArea[k].InformCiviliansNearestWater(
                                new Vector2(cleanWaterPosition.Value.X, cleanWaterPosition.Value.Y)); ;
                        }

                        if (foodPosition != null)
                        {
                            localArea[k].InformCiviliansNearestFood(
                                new Vector2(foodPosition.Value.X, foodPosition.Value.Y));
                        }

                        if (medsPosition != null)
                        {
                            localArea[k].InformCiviliansNearestMeds(
                                new Vector2(medsPosition.Value.X, medsPosition.Value.Y));
                        }

                        if (shelterPosition != null)
                        {
                            localArea[k].InformCiviliansNearestShelter(
                                new Vector2(shelterPosition.Value.X, shelterPosition.Value.Y));
                        }
                    }
                }
            }
        }

        public void ApplyDensityIllnessPenalty()
        {
            for (int i = 0; i < buckets.GetLength(0); i++)
            {
                for (int j = 0; j < buckets.GetLength(1); j++)
                {
                    buckets[i, j].InflictCiviliansHealthPenalty();
                }
            }
        }

        public void ClearBuckets()
        {
            foreach (ProcessingBucket bucket in buckets)
            {
                bucket.Clear();
            }
        }
    }
}
