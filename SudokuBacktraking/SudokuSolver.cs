namespace SudokuBacktraking;

public class SudokuSolver
{
  
    private bool FindEmptyCell(int[,] board, out int row, out int col)
    {
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    return true;
                }
            }
        }

        row = -1;
        col = -1;
        return false;
    }
    private bool UsedInRow(int[,] board, int row, int num)
    {
        for (int col = 0; col < 9; col++)
        {
            if (board[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }
    private bool UsedInCol(int[,] board, int col, int num)
    {
        for (int row = 0; row < 9; row++)
        {
            if (board[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }

    private bool UsedInBox(int[,] board, int boxStartRow, int boxStartCol, int num)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row + boxStartRow, col + boxStartCol] == num)
                {
                    return true;

                }
            }
        }
        return false;
    }

    public bool Solve(int[,] board)
    {
       
        return SolveSudoku(board);
        
    }
    private bool IsSafe(int[,] board, int row, int col, int num)
    {
        return !UsedInRow(board, row, num) &&
               !UsedInCol(board, col, num) &&
               !UsedInBox(board, row - row % 3, col - col % 3, num);
    }
    
    private bool SolveSudoku(int[,] board)
    {
        if (!FindEmptyCell(board, out int row, out int col))
        {
            return true; 
        }

        for (int num = 1; num <= 9; num++)
        {
            if (IsSafe(board, row, col, num))
            {
                board[row, col] = num;

                if (SolveSudoku(board))
                {
                    return true;
                }

                board[row, col] = 0; 
            }
        }

        return false; 
    }
}