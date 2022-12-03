using System;
using System.Collections.Generic;

namespace Game.Common.Map
{
    [Serializable]
    public class GameMapBoundaryList
    {
        private List<GameMapBoundary> boundaries = new List<GameMapBoundary>();
        private List<GameMapBoundary> sortedBoundaries = new List<GameMapBoundary>();
        private GameMapBoundary[] sortedBoundariesArr;


        public void AddBoundary(GameMapBoundary boundary)
        {
            boundaries.Add(boundary);
            SortBoundaries();
        }

        public void RemoveBoundary(GameMapBoundary boundary)
        {
            boundaries.Remove(boundary);
            SortBoundaries();
        }

        private void SortBoundaries()
        {
            sortedBoundariesArr = new GameMapBoundary[boundaries.Count];
            sortedBoundaries = new List<GameMapBoundary>(boundaries);
            
            sortedBoundaries.Sort(CompareBoundaries);
            sortedBoundaries.CopyTo(sortedBoundariesArr);
        }

        private int CompareBoundaries(GameMapBoundary b1, GameMapBoundary b2)
        {
            return b1.Priority.CompareTo(b2.Priority);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clone"> Clone if you need to mess with the list</param>
        /// <returns></returns>
        public  GameMapBoundary[] GetBoundaries(bool clone = false)
        {
            if (clone)
            {
                GameMapBoundary[] clonedBoudnaries = new GameMapBoundary[boundaries.Count];
                sortedBoundaries.CopyTo(clonedBoudnaries);
                
                return clonedBoudnaries;
            }

            return sortedBoundariesArr;
        }
        

    }
}