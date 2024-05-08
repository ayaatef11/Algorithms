using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudokuo_game
 { 

class sudoko_generator {
        coordUtils cc;
    public  static bool FillWithValidSolution(Grid grid, Coord currCoord = default)
    {
        Coord nextCoord = cc.GetNextCellCoord(currCoord);

        List<int> values = grid.GetPossibleValuesForCellAtCoord(currCoord);
        if (values.Count == 0)
            return false;

        // We randomize the values so that we get a random solution each time.
        int rndSeed = (int)DateTime.Now.Ticks;
        Random rnd = new Random(rndSeed);
        values = values.OrderBy(x => rnd.Next()).ToList();

        foreach (int value in values)
        {
            grid.Update(currCoord, value);
            if (currCoord.Equals(new Coord(8, 8))) // Done
                return true;

            bool nextCellIsFilled = FillWithValidSolution(grid, nextCoord);
            if (nextCellIsFilled)
                return true;

            // If this value didn't work, we need to clear the grid of all the values that have been filled as a result of this.
            grid.ClearValuesStartingFromCoord(currCoord);
        }

        return false;
    }

    public static void RemoveValuesFromSolution(Grid grid, int valuesToRemove)
    {
        List<Coord> randomCellCoords = cc.GetNRandomCellCoords(valuesToRemove);
        foreach (Coord coord in randomCellCoords)
            grid.Update(coord, 0);
    }

    public static Grid GeneratePuzzle()
    {
        Grid grid = new Grid();
        FillWithValidSolution(grid);
        RemoveValuesFromSolution(grid, 30);
        return grid;
    }
}}
