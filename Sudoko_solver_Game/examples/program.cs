using system;
namespace sudokuo_game
{
    class program
    {
        Grid grid;
        grid.set_initial_state_from_file("examples//sample1.txt");
console.writeline(grid);
    sudoku.solve(&grid);
console.writeline(grid);
    }
}