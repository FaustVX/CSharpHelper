using System.Collections.Generic;

namespace CSharpHelper
{

	public abstract class Cell<T> : Grid<T>.ICell
		where T : class, Grid<T>.ICell
	{
		private readonly int _x, _y;
		private T _up, _left, _down, _right;

		protected Cell(int x, int y, Grid<T> list, bool taurus)
		{
			_x = x;
			_y = y;

			if (x != 0)
			{
				_left = list[x - 1, y];
				(Left as Cell<T>)._right = this as T;
				if (taurus && X == list.Width - 1)
				{
					_right = list[0, y];
					(Right as Cell<T>)._left = this as T;
				}
			}

			if (y != 0)
			{
				_up = list[x, y - 1];
				(Up as Cell<T>)._down = this as T;
				if (taurus && Y == list.Height - 1)
				{
					_down = list[x, 0];
					(Down as Cell<T>)._up = this as T;
				}
			}
		}

		public T Up
		{
			get { return _up; }
		}

		public T Left
		{
			get { return _left; }
		}

		public T Down
		{
			get { return _down; }
		}

		public T Right
		{
			get { return _right; }
		}

		public int X
		{
			get { return _x; }
		}

		public int Y
		{
			get { return _y; }
		}
	}

	public abstract class DiagonalCell<T> : Cell<T>
		where T : DiagonalCell<T>
	{
		//private T _upLeft, _downLeft, _upRight, _downRight;

		protected DiagonalCell(int x, int y, Grid<T> list, bool taurus)
			: base(x, y, list, taurus)
		{ }

		public T UpLeft
		{
			get
			{
				if (Up != null)
					return Up.Left;
				return Left != null ? Left.Up : null;
			}
		}

		public T DownRight
		{
			get
			{
				if (Down != null)
					return Down.Right;
				return Right != null ? Right.Down : null;
			}
		}

		public T DownLeft
		{
			get
			{
				if (Down != null)
					return Down.Left;
				return Left != null ? Left.Down : null;
			}
		}

		public T UpRight
		{
			get
			{
				if (Up != null)
					return Up.Right;
				return Right != null ? Right.Up : null;
			}
		}
	}

	public class Grid<T>
		where T : class, Grid<T>.ICell
	{
		#region Inner Class Cell

		public interface ICell
		{
			T Up
			{
				get;
			}

			T Down
			{
				get;
			}

			T Left
			{
				get;
			}

			T Right
			{
				get;
			}

			int X
			{
				get;
			}

			int Y
			{
				get;
			}
		}

		#endregion

		private readonly T[,] _cells;
		private readonly int _width;
		private readonly int _height;

		protected Grid(int width, int height, System.Func<int, int, Grid<T>, T> createCell)
		{
			_width = width;
			_height = height;
			_cells = new T[_width,_height];

			for (int i = 0; i < _width; ++i)
				for (int j = 0; j < _height; ++j)
					_cells[i, j] = createCell(i, j, this); // (T)Cell.New(i, j, this as Grid<Cell>);
		}

		public T this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= _width || y >= _height)
					return null;
				return _cells[x, y];
			}
		}

		public T[,] Cells
		{
			get { return _cells; }
		}

		public int Width
		{
			get { return _width; }
		}

		public int Height
		{
			get { return _height; }
		}
	}
}