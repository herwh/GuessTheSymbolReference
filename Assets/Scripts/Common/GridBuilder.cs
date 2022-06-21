using UnityEngine;

namespace Common
{
    public class GridBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject _gridSpace;
        [SerializeField] private Cell _cell;

        private const float HALF = 0.5f;
        private Grid _grid;

        public Grid Grid => _grid;


        public void Build(int rows, int columns)
        {
            _grid = new Grid(rows, columns);
            CreateCell(rows, columns);
        }

        private void CreateCell(int rows, int columns)
        {
            var cellSize = _cell.GetSize();
            var offset = GetOffset(columns, rows, cellSize);

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var position = new Vector3(column * cellSize.x, row * cellSize.y, 0);
                    var cell = Instantiate(_cell, position + offset, Quaternion.identity);
                    cell.transform.SetParent(_gridSpace.transform, false);
                    _grid.AddCell(cell, row, column);
                }
            }
        }

        private Vector3 GetOffset(int columns, int rows, Vector3 cellSize)
        {
            return new Vector3(
                GetSideOffset(columns, cellSize.x),
                GetSideOffset(rows, cellSize.y),
                0);
        }

        private float GetSideOffset(int side, float size)
        {
            return -side * HALF * size + size * HALF;
        }
    }
}