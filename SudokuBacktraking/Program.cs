namespace SudokuBacktraking;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Test sin UI
        int[,] testBoard = new int[,]
        {
            {3, 0, 6, 5, 0, 8, 4, 0, 0},
            {5, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 8, 7, 0, 0, 0, 0, 3, 1},
            {0, 0, 3, 0, 1, 0, 0, 8, 0},
            {9, 0, 0, 8, 6, 3, 0, 0, 5},
            {0, 5, 0, 0, 9, 0, 6, 0, 0},
            {1, 3, 0, 0, 5, 0, 2, 5, 0},
            {0, 0, 0, 0, 0, 0, 0, 7, 4},
            {0, 0, 5, 2, 0, 6, 3, 0, 0}
        };
    
        SudokuSolver solver = new SudokuSolver();
    
        if (solver.Solve(testBoard))
        {
            Console.WriteLine("Solución:");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(testBoard[i, j] + " ");
                Console.WriteLine();
            }
        }
    
        Console.ReadLine();
    
        // Luego ejecuta la UI
        Application.EnableVisualStyles();
        Application.Run(new SudokuForm());
    }
}