using sudokuo_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace sudokuo_game
{
    public class Grid
    {
        Coord coord = new Coord();
        private int[,] grid = new int[9, 9];
        private HashSet<Coord> coords_that_were_pre_filled;
        public Grid()
        {
            int[] filled_array = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = filled_array[j];
                }
            }
        }
        //setters and getters
        public void SetInitialState(int[,] grid)
        {
            coords_that_were_pre_filled.Clear();

            for (int row = 0; row < Coord.GRID_LEN; row++)
            {
                for (int col = 0; col < Coord.GRID_LEN; col++)
                {
                    int cellValue = grid[row, col];

                    if (cellValue < 0 || cellValue > 9)
                    {
                        throw new ArgumentException("Cell value should be in range 0 <= x <= 9");
                    }

                    this.grid[row, col] = cellValue;
                    if (cellValue == 0) continue;
                    coords_that_were_pre_filled.Add(new Coord(row, col));
                }
            }
        }
        public void SetInitialStateFromFile(string filename)
        {
            try
            {
                using (StreamReader file = new StreamReader(filename))
                {
                    for (int row = 0; row < Coord.GRID_LEN; row++)
                    {
                        string line = file.ReadLine();
                        if (line == null)
                            throw new ArgumentException("Too few lines provided. Please supply 9 lines.");

                        string[] values = line.Split(' ');
                        if (values.Length != Coord.GRID_LEN)
                            throw new ArgumentException("Incorrect number of values provided in line. Please supply 9 values.");

                        for (int col = 0; col < Coord.GRID_LEN; col++)
                        {
                            int value;
                            if (!int.TryParse(values[col], out value))
                                throw new ArgumentException("Invalid value provided.");

                            grid[row, col] = value;
                            if (value != 0)
                                coords_that_were_pre_filled.Add(new Coord(row, col));
                        }
                    }

                    if (!file.EndOfStream)
                        throw new ArgumentException("Too many lines provided. Please supply only 9 lines.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        public void Update(Coord coord, int value)
        {
            if (value < 0 || value > 9)
                throw new ArgumentException("Cell value should be in range 0 <= x <= 9");

            grid[coord.Row, coord.Col] = value;
        }

        public int Get(Coord coord)
        {
            return grid[coord.Row, coord.Col];
        }

        public void ClearValuesStartingFromCoord(Coord coord)
        {
            for (int row = coord.Row; row < Coord.GRID_LEN; row++)
            {
                for (int col = coord.Col; col < Coord.GRID_LEN; col++)
                {
                    Coord currentCoord = new Coord(row, col);
                    if (!coords_that_were_pre_filled.Contains(currentCoord))
                        grid[row, col] = 0;
                }
            }
        }

        /*----------*/
        /* Checkers */

        public bool ValueExistsElsewhereInColumn(Coord coord, int value)
        {
            return Enumerable.Range(0, Coord.GRID_LEN)
                .Any(row => row != coord.Row && grid[row, coord.Col] == value);
        }

        public bool ValueExistsElsewhereInRow(Coord coord, int value)
        {
            return Enumerable.Range(0, Coord.GRID_LEN)
                .Any(col => col != coord.Col && grid[coord.Row, col] == value);
        }

        public bool ValueExistsElsewhereIn3x3Grid(Coord coord, int value)
        {
            int rowStart = (coord.Row / 3) * 3;
            int rowEnd = rowStart + 2;

            int colStart = (coord.Col / 3) * 3;
            int colEnd = colStart + 2;

            for (int row = rowStart; row <= rowEnd; row++)
            {
                for (int col = colStart; col <= colEnd; col++)
                {
                    Coord coo = new Coord(row, col);
                    if (coo != coord && grid[row, col] == value)
                        return true;
                }
            }

            return false;
        }

        public bool CoordWasPreFilled(Coord coord)
        {
            return coords_that_were_pre_filled.Contains(coord);
        }

        public List<int> GetPossibleValuesForCellAtCoord(Coord coord)
        {
            List<int> values = Enumerable.Range(1, 9).ToList();
            List<int> filteredValues = new List<int>();

            foreach (int value in values)
            {
                if (!ValueExistsElsewhereInColumn(coord, value) &&
                    !ValueExistsElsewhereInRow(coord, value) &&
                    !ValueExistsElsewhereIn3x3Grid(coord, value))
                {
                    filteredValues.Add(value);
                }
            }

            return filteredValues;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Grid))
                return false;

            Grid otherGrid = (Grid)obj;
            for (int row = 0; row < Coord.GRID_LEN; row++)
            {
                for (int col = 0; col < Coord.GRID_LEN; col++)
                {
                    Coord coord = new Coord(row, col);
                    if (Get(coord) != otherGrid.Get(coord))
                        return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            string output = "+-------+-------+-------+\n";
            for (int row = 0; row < Coord.GRID_LEN; row++)
            {
                output += "|";
                for (int col = 0; col < Coord.GRID_LEN; col++)
                {
                    output += " " + grid[row, col];
                    if (col % 3 == 2)
                        output += " |";
                }
                output += "\n";
                if (row % 3 == 2)
                    output += "+-------+-------+-------+\n";
            }
            return output;
        }

        /* public int Get(Coord coord)
         {
             return grid[coord.Row, coord.Col];
         }*/

        public void SetInitialValue(Coord coord, int value)
        {
            grid[coord.Row, coord.Col] = value;
            if (value != 0)
                coords_that_were_pre_filled.Add(coord);
        }
    }

}
