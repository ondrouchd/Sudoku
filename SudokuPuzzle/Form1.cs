﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuPuzzle
{
    public partial class Form1 : Form
    {
        internal MaskedTextBox[,] textBoxes = new MaskedTextBox[9, 9];

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    MaskedTextBox textBox = new MaskedTextBox
                    {
                        Width = 30,
                        Height = 30,
                        MaxLength = 1,
                        TextAlign = HorizontalAlignment.Center,
                        Location = new System.Drawing.Point(30 * col, 30 * row),
                        Mask = "0"
                    };
                    textBox.MaskInputRejected += TextBox_MaskInputRejected;
                    Controls.Add(textBox);
                    textBoxes[row, col] = textBox;
                }
            }

            System.Windows.Forms.Button save = new System.Windows.Forms.Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(60 * 0, 30 * 9),
                Width = 60,
                Height = 30
            };
            save.Click += btnSave_Click;
            Controls.Add(save);

            System.Windows.Forms.Button load = new System.Windows.Forms.Button
            {
                Text = "Load",
                Location = new System.Drawing.Point(60 * 1, 30 * 9),
                Width = 60,
                Height = 30
            };
            load.Click += btnLoad_Click;
            Controls.Add(load);

            System.Windows.Forms.Button solve = new System.Windows.Forms.Button
            {
                Text = "Solve",
                Location = new System.Drawing.Point(60 * 2, 30 * 9),
                Width = 60,
                Height = 30
            };
            solve.Click += btnSolve_Click;
            Controls.Add(solve);
        }

        private void TextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            MaskedTextBox textBox = sender as MaskedTextBox;
            if (textBox != null)
            {
                textBox.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int row = 0; row < 9; row++)
                        {
                            for (int col = 0; col < 9; col++)
                            {
                                if (col == 8)
                                {
                                    writer.Write(textBoxes[row, col].Text);
                                }
                                else
                                {
                                    writer.Write(textBoxes[row, col].Text + " ");
                                }
                            }
                            writer.WriteLine();
                        }
                    }
                }
            }

            // clean up the board
            CleanUpTheBoard();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                bool cleanUp = true;
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        for (int row = 0; row < 9; row++)
                        {
                            string lineContent = reader.ReadLine();
                            if (lineContent != null)
                            {
                                cleanUp = false;
                                string[] line = lineContent.Split(' ');
                                for (int col = 0; col < 9; col++)
                                {
                                    textBoxes[row, col].Text = line[col];
                                }
                            }
                        }

                        if (cleanUp)
                        {
                            MessageBox.Show("No data found in the file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            int[,] board = new int[9, 9];

            // read the board
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (int.TryParse(textBoxes[row, col].Text, out int value))
                    {
                        board[row, col] = value;
                    }
                    else
                    {
                        board[row, col] = 0;
                    }
                }
            }

            // check if sudoku is solved
            if (SolveSudoku(board))
            {
                MessageBox.Show("Solution found!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No solution found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal bool SolveSudoku(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
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
            }

            return true;
        }

        private bool IsSafe(int[,] board, int row, int col, int num)
        {
            for (int x = 0; x < 9; x++)
            {
                if (board[row, x] == num || board[x, col] == num)
                {
                    return false;
                }
            }

            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[startRow + i, startCol + j] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CleanUpTheBoard()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    textBoxes[row, col].Text = "";
                }
            }
        }
    }
}
