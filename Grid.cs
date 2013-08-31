namespace CSharpHelper
{

	/// <summary>
	/// classe de cellules basique 
	/// </summary>
	/// <typeparam name="T">Type de cellules, héritant de la classe <see cref="Cell{T}"/></typeparam>
	public class Cell<T> : Grid<T>.ICell
		where T : Cell<T>
	{
		private readonly int _x, _y;
		private T _up, _left, _down, _right;

		/// <summary>
		/// Constructeur de la  cellule
		/// </summary>
		/// <param name="x">position en X</param>
		/// <param name="y">position en Y</param>
		/// <param name="list">liste de cellules <typeparamref name="T"/></param>
		/// <param name="torus">indique si les cellules agissent comme un torus(les cellules du haut sont liées aux cellules du bas, et vics-versa, pareil pour la gauche et la droite)</param>
		protected Cell(int x, int y, Grid<T> list, bool torus)
		{
			_x = x;
			_y = y;

			if (x != 0)
			{
				_left = list[x - 1, y];
				(Left as Cell<T>)._right = this as T;
				if (torus && X == list.Width - 1)
				{
					_right = list[0, y];
					(Right as Cell<T>)._left = this as T;
				}
			}

			if (y != 0)
			{
				_up = list[x, y - 1];
				(Up as Cell<T>)._down = this as T;
				if (torus && Y == list.Height - 1)
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

	/// <summary>
	/// classe de cellules basique gérant les diagonales
	/// </summary>
	/// <typeparam name="T">Type de cellules, héritant de la classe <see cref="DiagonalCell{T}"/></typeparam>
	public class DiagonalCell<T> : Cell<T>
		where T : DiagonalCell<T>
	{
		//private T _upLeft, _downLeft, _upRight, _downRight;

		/// <summary>
		/// Constructeur de la  cellule
		/// </summary>
		/// <param name="x">position en X</param>
		/// <param name="y">position en Y</param>
		/// <param name="list">liste de cellules <typeparamref name="T"/></param>
		/// <param name="torus">indique si les cellules agissent comme un torus(les cellules du haut sont liées aux cellules du bas, et vics-versa, pareil pour la gauche et la droite)</param>
		protected DiagonalCell(int x, int y, Grid<T> list, bool torus)
			: base(x, y, list, torus)
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

		public T UpRight
		{
			get
			{
				if (Up != null)
					return Up.Right;
				return Right != null ? Right.Up : null;
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

		public T DownRight
		{
			get
			{
				if (Down != null)
					return Down.Right;
				return Right != null ? Right.Down : null;
			}
		}
	}

	/// <summary>
	/// Classe permettant la gestion de cellules de type <typeparamref name="T"/> dans une grille
	/// </summary>
	/// <typeparam name="T">Type de cellules, implémentant l'interface <see cref="ICell"/></typeparam>
	public class Grid<T>
		where T : class, Grid<T>.ICell
	{
		#region Inner Class Cell

		/// <summary>
		/// Interface de base pour les cellules
		/// </summary>
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

		/// <summary>
		/// Constructeur de la grille
		/// </summary>
		/// <param name="width">Nombre de cellules horizontalement</param>
		/// <param name="height">Nombre de cellules verticalement</param>
		/// <param name="createCell">constructeur de cellules <typeparamref name="T"/> en fonction de sa position x, y et de la <see cref="Grid"/> de <typeparamref name="T"/></param>
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