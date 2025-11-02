using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SudokuBacktraking
{
    public partial class SudokuForm : Form
    {
        private TextBox[,] cells = new TextBox[9, 9];
        private Panel gridContainer; // Contenedor para dibujar bordes gruesos
        private TableLayoutPanel gridPanel;
        private Button btnSolve;
        private Button btnClear;
        private Label lblTitle;
        private Label lblStatus;
        private SudokuSolver solver = new SudokuSolver();

        public SudokuForm()
        {
            InitializeComponent();
            SetupComponent();
            InitializeCustomComponents();
        }

        private void SetupComponent()
        {
            this.Text = "Sudoku Solver - Backtracking";
            this.Size = new Size(650, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InitializeCustomComponents()
        {
            // Panel principal
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Título
            lblTitle = new Label
            {
                Text = "SUDOKU SOLVER - BACKTRACKING",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40,
                Dock = DockStyle.Top
            };

            // Contenedor con Paint personalizado para bordes gruesos
            gridContainer = new Panel
            {
                Size = new Size(550, 550),
                Location = new Point(50, 60),
                BackColor = Color.White
            };
            gridContainer.Paint += GridContainer_Paint;

            // Grid Panel (Cuadrícula 9x9) - SIN bordes propios
            gridPanel = new TableLayoutPanel
            {
                RowCount = 9,
                ColumnCount = 9,
                Size = new Size(540, 540),
                Location = new Point(5, 5), // Dentro del contenedor
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                BackColor = Color.Transparent
            };

            // Configurar filas y columnas
            for (int i = 0; i < 9; i++)
            {
                gridPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
                gridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            }

            // Crear las 81 celdas (TextBoxes)
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    cells[row, col] = new TextBox
                    {
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Arial", 20, FontStyle.Bold),
                        MaxLength = 1,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.White,
                        ForeColor = Color.FromArgb(52, 73, 94),
                        Tag = new Point(row, col)
                    };

                    // Eventos
                    cells[row, col].KeyPress += Cell_KeyPress;
                    cells[row, col].TextChanged += Cell_TextChanged;
                    cells[row, col].Enter += Cell_Enter;
                    cells[row, col].Paint += Cell_Paint; // Para bordes personalizados

                    gridPanel.Controls.Add(cells[row, col], col, row);
                }
            }

            gridContainer.Controls.Add(gridPanel);

            // Panel de botones
            Panel buttonPanel = new Panel
            {
                Height = 70,
                Dock = DockStyle.Bottom
            };

            btnSolve = CreateButton("Resolver", new Point(150, 17), Color.FromArgb(46, 204, 113));
            btnClear = CreateButton("Limpiar", new Point(320, 17), Color.FromArgb(231, 76, 60));

            btnSolve.Click += BtnSolve_Click;
            btnClear.Click += BtnClear_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnSolve, btnClear });

            // Label de estado
            lblStatus = new Label
            {
                Text = "Ingrese los números iniciales del Sudoku (puede incluir valores incorrectos)",
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30,
                Width = 600,
                Location = new Point(25, 620)
            };

            // Agregar controles al formulario
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(gridContainer);
            mainPanel.Controls.Add(lblStatus);
            mainPanel.Controls.Add(buttonPanel);
            this.Controls.Add(mainPanel);
        }

        // Dibujar bordes gruesos para las cajas 3x3
        private void GridContainer_Paint(object sender, PaintEventArgs e)
        {
            using (Pen thickPen = new Pen(Color.Black, 3))
            using (Pen thinPen = new Pen(Color.Black, 1))
            {
                int cellSize = 60;
                
                // Bordes gruesos (cajas 3x3)
                for (int i = 0; i <= 3; i++)
                {
                    int pos = 5 + i * cellSize * 3;
                    // Líneas verticales gruesas
                    e.Graphics.DrawLine(thickPen, pos, 5, pos, 545);
                    // Líneas horizontales gruesas
                    e.Graphics.DrawLine(thickPen, 5, pos, 545, pos);
                }

                // Bordes finos (celdas individuales)
                for (int i = 1; i < 9; i++)
                {
                    if (i % 3 != 0) // Saltar las que ya son gruesas
                    {
                        int pos = 5 + i * cellSize;
                        // Líneas verticales finas
                        e.Graphics.DrawLine(thinPen, pos, 5, pos, 545);
                        // Líneas horizontales finas
                        e.Graphics.DrawLine(thinPen, 5, pos, 545, pos);
                    }
                }
            }
        }

        // Bordes internos de las celdas (opcional, para mayor claridad)
        private void Cell_Paint(object sender, PaintEventArgs e)
        {
            TextBox cell = (TextBox)sender;
            Point pos = (Point)cell.Tag;
            int row = pos.X;
            int col = pos.Y;

            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                // Borde derecho fino
                if (col < 8 && (col + 1) % 3 != 0)
                    e.Graphics.DrawLine(pen, cell.Width - 1, 0, cell.Width - 1, cell.Height);
                
                // Borde inferior fino
                if (row < 8 && (row + 1) % 3 != 0)
                    e.Graphics.DrawLine(pen, 0, cell.Height - 1, cell.Width, cell.Height - 1);
            }
        }

        private Button CreateButton(string text, Point location, Color backColor)
        {
            return new Button
            {
                Text = text,
                Size = new Size(140, 45),
                Location = location,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // ============================================
        // EVENTOS DE LAS CELDAS
        // ============================================

        private void Cell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '1' || e.KeyChar > '9'))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
            }
        }

        private void Cell_TextChanged(object sender, EventArgs e)
        {
            TextBox cell = (TextBox)sender;
            
            if (cell.Text.Length > 0)
            {
                if (cell.Text[0] < '1' || cell.Text[0] > '9')
                {
                    cell.Text = "";
                }
            }
        }

        private void Cell_Enter(object sender, EventArgs e)
        {
            TextBox cell = (TextBox)sender;
            cell.SelectAll();
        }

        // ============================================
        // EVENTOS DE BOTONES
        // ============================================

        private void BtnSolve_Click(object sender, EventArgs e)
        {
            try
            {
                int[,] board = GetBoardFromUI();

                if (!HasAnyNumber(board))
                {
                    lblStatus.Text = "Debe ingresar al menos un número inicial";
                    lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
                    MessageBox.Show("Debe ingresar al menos un número inicial.", 
                        "Tablero vacío", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }

                bool[,] initialCells = GetInitialCellsMap(board);

                lblStatus.Text = "Resolviendo con backtracking...";
                lblStatus.ForeColor = Color.FromArgb(243, 156, 18);
                Application.DoEvents();

                if (solver.Solve(board))
                {
                    ShowSolution(board, initialCells);
                    lblStatus.Text = "¡Sudoku resuelto! El backtracking encontró la solución";
                    lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
                    MessageBox.Show("¡Sudoku resuelto exitosamente!\n\nEl algoritmo de backtracking encontró una solución válida.", 
                        "Éxito", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "No existe solución. El backtracking probó todas las combinaciones posibles";
                    lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
                    MessageBox.Show("No existe solución para este Sudoku.\n\nEl algoritmo de backtracking exploró todas las posibilidades y no encontró una configuración válida.", 
                        "Sin solución", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error al resolver el Sudoku";
                lblStatus.ForeColor = Color.FromArgb(231, 76, 60);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "¿Está seguro de que desea limpiar todo el tablero?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearBoard();
                lblStatus.Text = "Tablero limpio. Ingrese los números iniciales del Sudoku";
                lblStatus.ForeColor = Color.FromArgb(127, 140, 141);
            }
        }

        // ============================================
        // MÉTODOS AUXILIARES
        // ============================================

        private int[,] GetBoardFromUI()
        {
            int[,] board = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (int.TryParse(cells[i, j].Text, out int value))
                    {
                        board[i, j] = value;
                    }
                    else
                    {
                        board[i, j] = 0;
                    }
                }
            }

            return board;
        }

        private bool HasAnyNumber(int[,] board)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] != 0)
                        return true;
                }
            }
            return false;
        }

        private bool[,] GetInitialCellsMap(int[,] board)
        {
            bool[,] initialCells = new bool[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    initialCells[i, j] = board[i, j] != 0;
                }
            }

            return initialCells;
        }

        private void ShowSolution(int[,] board, bool[,] initialCells)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].Text = board[i, j].ToString();

                    if (initialCells[i, j])
                    {
                        cells[i, j].ForeColor = Color.FromArgb(52, 73, 94);
                        cells[i, j].Font = new Font("Arial", 20, FontStyle.Bold);
                        cells[i, j].ReadOnly = true;
                    }
                    else
                    {
                        cells[i, j].ForeColor = Color.FromArgb(52, 152, 219);
                        cells[i, j].Font = new Font("Arial", 20, FontStyle.Regular);
                        cells[i, j].ReadOnly = true;
                    }
                    
                    cells[i, j].Invalidate(); // Redibujar bordes
                }
            }
        }

        private void ClearBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].Text = "";
                    cells[i, j].ReadOnly = false;
                    cells[i, j].ForeColor = Color.FromArgb(52, 73, 94);
                    cells[i, j].Font = new Font("Arial", 20, FontStyle.Bold);
                    cells[i, j].BackColor = Color.White;
                    cells[i, j].Invalidate(); // Redibujar bordes
                }
            }
        }
    }
}