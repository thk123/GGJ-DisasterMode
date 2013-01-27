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
            ProcessingBucket[] neighbours = 
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
            return neighbours;
        }


        public void addNewAction(GameAction action, int xCoordinate, int yCoordinate)
        {
            Point? nullBucket = getBucketID(xCoordinate, yCoordinate);
            if (nullBucket == null) return;
            Point bucket = (Point)nullBucket;

            this.actions.Add(action);
            this.buckets[bucket.X, bucket.Y].addAction(action);
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

            if (this.grid.GetGridPointFromMousePosition(civilianPosition) != null)
            {
                civilians.Add(civilian);
            }
        }

        public void PopulateBuckets()
        {
            ClearBuckets();

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
                    Point position = (Point) action.ActionPosition;
                    Point gridPosition = (Point) this.grid.GetGridPointFromMousePosition(position);
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
            for (int i = 1; i < buckets.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < buckets.GetLength(1) - 1; j++)
                {
                    ProcessingBucket[] localArea = getNeighbours(i, j);
                    Point? cleanWaterPosition = null;
                    for (int k = 1; k < localArea.GetLength(0); k++)
                    {
                        if (localArea[k].hasCleanWater())
                        {
                            cleanWaterPosition = localArea[k].Water.Position;
                        }
                    }
                    if (cleanWaterPosition != null)
                    {
                        Point cleanWater = (Point)cleanWaterPosition;
                        for (int k = 1; k < localArea.GetLength(0); k++)
                        {
                            localArea[k].InformCiviliansNearestWater(new Vector2(cleanWater.X,
                                                        cleanWater.Y));
                        } 
                    }
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
