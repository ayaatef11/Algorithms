using System;
using System.Collections.Generic;

namespace sudokuo_game
{
    public class SudokuSolver
    {
        private const int GRID_LEN = 9;

        public static bool Solve(Grid grid, Coord cellCoord = default)
        {
            Coord nextCoord = GetNextCellCoord(cellCoord);

            if (grid.CoordWasPreFilled(cellCoord))
            {
                if (cellCoord.Equals(new Coord(8, 8)))
                    return true;
                return Solve(grid, nextCoord);
            }

            List<int> values = grid.GetPossibleValuesForCellAtCoord(cellCoord);
            if (values.Count == 0)
                return false;

            foreach (int value in values)
            {
                grid.Update(cellCoord, value);
                if (cellCoord.Equals(new Coord(8, 8))) // Solved!
                    return true;

                bool nextCellIsSolved = Solve(grid, nextCoord);
                if (nextCellIsSolved)
                    return true;

                // If this value didn't work, we need to clear the grid of all the values that have been filled as a result of this.
                grid.ClearValuesStartingFromCoord(cellCoord);
            }

            if (cellCoord.Equals(new Coord(0, 0)))
                throw new InvalidOperationException("This puzzle doesn't have a solution!");

            return false;
        }

        private static Coord GetNextCellCoord(Coord coord)
        {
            if (coord.Col == GRID_LEN - 1 && coord.Row == GRID_LEN - 1)
                return coord;
            else if (coord.Col == GRID_LEN - 1)
                return new Coord(coord.Row + 1, 0);
            else
                return new Coord(coord.Row, coord.Col + 1);
        }
    }
}
