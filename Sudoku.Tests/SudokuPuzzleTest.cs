using Microsoft.SqlServer.Server;
using NUnit.Framework;
using SudokuPuzzle;
using System.IO;
using System.Windows.Forms;

namespace Sudoku.Tests
{
    [TestFixture]
    public class SudokuPuzzleTest
    {
        private Form1 form;
        private const string sudokuPuzzleDirectory = @"C:\Temp\Sudoku\";

        [SetUp]
        public void Setup()
        {
            form = new Form1();
        }

        [Test]
        public void TestSolvablePuzzle()
        {
            LoadPuzzle("01.txt");
            Assert.IsTrue(form.SolveSudoku(GetBoard()));
            int[,] expectedSolution = new int[,]
            {
                { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
                { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
            };

            AssertBoardEquals(expectedSolution, GetBoard());
        }

        [Test]
        public void TestUnsolvablePuzzle()
        {
            LoadPuzzle("empty.txt");
            Assert.IsFalse(form.SolveSudoku(GetBoard()));
        }

        private void LoadPuzzle(string fileName)
        {
            string filePath = Path.Combine(sudokuPuzzleDirectory, fileName);
            using (StreamReader reader = new StreamReader(filePath))
            {
                for (int row = 0; row < 9; row++)
                {
                    string[] line = reader.ReadLine()?.Split(' ');
                    for (int col = 0; col < 9; col++)
                    {
                        form.textBoxes[row, col].Text = line?[col];
                    }
                }
            }
        }

        private int[,] GetBoard()
        {
            int[,] board = new int[9, 9];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (int.TryParse(form.textBoxes[row, col].Text, out int value))
                    {
                        board[row, col] = value;
                    }
                    else
                    {
                        board[row, col] = 0;
                    }
                }
            }
            return board;
        }

        private void AssertBoardEquals(int[,] expected, int[,] actual)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Assert.AreEqual(expected[row, col], actual[row, col]);
                }
            }
        }
    }
}
