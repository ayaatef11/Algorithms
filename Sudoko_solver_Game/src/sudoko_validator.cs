using System;

namespace sudokuo_game
{
    public class SudokuValidator
    {
        private const int GRID_LEN = 9;

        public static bool IsValidSolution(Grid grid, Coord currCoord = default)
        {
            int value = grid.Get(currCoord);
            if (value == 0)
                return false; // It's unfinished.
            else if (grid.ValueExistsElsewhereInColumn(currCoord, value) ||
                     grid.ValueExistsElsewhereInRow(currCoord, value) ||
                     grid.ValueExistsElsewhereIn3x3Grid(currCoord, value))
                return false;
            if (currCoord.Equals(new Coord(8, 8)))
                return true;
            else
                return IsValidSolution(grid, GetNextCellCoord(currCoord));
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
