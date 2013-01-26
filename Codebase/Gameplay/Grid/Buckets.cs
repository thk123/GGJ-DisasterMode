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

        private Point? getBucketID(int x, int y)
        {
            return grid.GetGridPointFromMousePosition(new Point(x,y));
        }

        private ProcessingBucket[] getNeighbours(int x, int y)
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


            this.buckets[bucket.X, bucket.Y].addAction(action);
        }

        public void addNewDrop(Dropoffs.Dropoff drop, int xCoordinate, int yCoordinate)
        {
            Point? nullBucket = getBucketID(xCoordinate, yCoordinate);
            if (nullBucket == null) return;
            Point bucket = (Point)nullBucket;

            this.buckets[bucket.X, bucket.Y].addDrop(drop);
        }

        public void ClearBuckets()
        {
            foreach (ProcessingBucket bucket in buckets)
            {
                bucket.clear();
            }
        }
    }
}
