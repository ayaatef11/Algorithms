using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudokuo_game
{
    public struct Coord
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public const int GRID_LEN = 9;
        public Coord(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

}
