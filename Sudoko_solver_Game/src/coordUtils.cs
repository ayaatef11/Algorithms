using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
namespace sudokuo_game
{
    public  class coordUtils
    {
        coordUtils()
        {

        }
        public Coord GetNextCellCoord(Coord coord)
        {
            if (coord.Col == Coord.GRID_LEN - 1 && coord.Row == Coord.GRID_LEN - 1)
                return coord;
            else if (coord.Col == Coord.GRID_LEN - 1)
                return new Coord(coord.Row + 1, 0);
            return new Coord(coord.Row, coord.Col + 1);
        }

        public HashSet<Coord> GetNRandomCellCoords(int n)
        {
            Random rnd = new Random();
            HashSet<Coord> randomCellCoords = new HashSet<Coord>();

            while (n > 0)
            {
                Coord cellCoord = new Coord(rnd.Next(0, Coord.GRID_LEN), rnd.Next(0, Coord.GRID_LEN));
                if (randomCellCoords.Add(cellCoord))
                    n--;
            }

            return randomCellCoords;
        }
    }
}
