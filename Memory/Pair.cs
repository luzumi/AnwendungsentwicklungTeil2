using System;

namespace Memory
{
    public class Pair
    {
        private int Points;
        private string name;
        private double solveTime;
        private int tileNumber;
        private DateTime solveDate;
        private int rank;

        
        public int Points1
        {
            get => Points;
            set => Points = value;
        }

        public string Name1
        {
            get => name;
            set => name = value;
        }

        public double SolveTime
        {
            get => solveTime;
            set => solveTime = value;
        }

        public int TileNumber
        {
            get => tileNumber;
            set => tileNumber = value;
        }

        public DateTime SolveDate
        {
            get => solveDate;
            set => solveDate = value;
        }

        public int Rank
        {
            get => rank;
            set => rank = value;
        }

    }
}
