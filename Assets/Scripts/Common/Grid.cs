using UnityEngine;

namespace Common
{
    public class Grid
    {
        private Cell[,] _cells;

        public Cell[,] Cells => _cells;

        public void AddCell(Cell cell, int row, int column)
        {
            _cells[row, column] = cell;
        }

        public void RemoveCells()
        {
            foreach (Cell cell in _cells)
            {
                Object.Destroy(cell.gameObject);
            }
        }

        public Grid(int rows, int columns)
        {
            _cells = new Cell[rows, columns];
        }

        public void Clear()
        {
            foreach (Cell cell in _cells)
            {
                cell.Clear();
            }
        }
    }
}