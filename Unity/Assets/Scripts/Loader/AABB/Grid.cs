using System;
using System.Collections.Generic;
using System.Linq;

namespace AABB
{
    /// <summary>
    /// Base spacial hashing of world's boxes
    /// </summary>
    public class Grid
    {
        public class Cell
        {
            public Cell(int x, int y, float cellSize)
            {
                Bounds = new RectangleF(x * cellSize, y * cellSize, cellSize, cellSize);
            }

            //equal unityengin.bounds
            public RectangleF Bounds { get; private set; }

            public IEnumerable<IBox> Children => children;

            private readonly List<IBox> children = new();

            public void Add(IBox box)
            {
                children.Add(box);
            }

            public bool Contains(IBox box)
            {
                return children.Contains(box);
            }

            public bool Remove(IBox box)
            {
                return children.Remove(box);
            }

            public int Count()
            {
                return children.Count;
            }
        }

        public Grid(int width, int height, float cellSize)
        {
            Cells = new Cell[width, height];
            CellSize = cellSize;
        }

        public float CellSize { get; set; }

        #region Size

        public float Width => Columns * CellSize;
        public float Height => Rows * CellSize;
        public int Columns => Cells.GetLength(0);
        public int Rows => Cells.GetLength(1);

        #endregion

        public Cell[,] Cells { get; private set; }

        /// <summary>
        /// query all box in the selected area
        /// </summary>
        public IEnumerable<Cell> QueryCells(float x, float y, float w, float h)
        {
            var minX = (int)(x / CellSize);
            var minY = (int)(y / CellSize);
            var maxX = (int)(x + w - 1) / CellSize + 1;
            var maxY = (int)(y + h - 1) / CellSize + 1;

            minX = Math.Max(0, minX);
            minY = Math.Max(0, minY);
            maxX = Math.Min(Columns - 1, maxX);
            maxY = Math.Min(Rows - 1, maxY);

            List<Cell> result = new List<Cell>();

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    var cell = Cells[i, j];

                    //也许不应该动态插入格子
                    if (cell == null)
                    {
                        cell = new Cell(i, j, CellSize);
                        Cells[i, j] = cell;
                    }

                    result.Add(cell);
                }
            }

            return result;
        }

        public IEnumerable<IBox> QueryBoxes(float x, float y, float w, float h)
        {
            var cell = QueryCells(x, y, w, h);

            return cell.SelectMany((c) => c.Children).Distinct(); //取唯一元素
        }

        public void Add(IBox box)
        {
            var cells = QueryCells(box.X, box.Y, box.Width, box.Height);

            foreach (Cell cell in cells)
            {
                if (!cell.Contains(box))
                {
                    cell.Add(box);
                }
            }
        }

        public void Update(IBox box, RectangleF from)
        {
            var fromCell = QueryCells(from.X, from.Y, from.Width, from.Height);
            var removed = false;

            foreach (var cell in fromCell)
            {
                removed |= cell.Remove(box);
            }

            if (removed)
            {
                Add(box);
            }
        }

        public bool Remove(IBox box)
        {
            var cells = this.QueryCells(box.X, box.Y, box.Width, box.Height);

            var removed = false;
            foreach (var cell in cells)
            {
                removed |= cell.Remove(box);
            }

            return removed;
        }

        public override string ToString()
        {
            return $"[Grid: Width={Width}, Height={Height}, Columns={Columns}, Rows={Rows}]";
        }
    }
}