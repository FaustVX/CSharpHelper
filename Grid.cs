using System.Diagnostics;
using System.Threading.Tasks;

namespace CSharpHelper
{

	/// <summary>
	/// classe de cellules basique 
	/// </summary>
	/// <typeparam name="T">Type de cellules, héritant de la classe <see cref="Cell{T}"/></typeparam>
	public abstract class Cell<T> : Grid<T>.ICell
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

			//_up = list[x, (y > 0 ? y : list.Height) - 1];
			//_down = list[x, (y < list.Height - 1 ? y + 1 : 0)];
			//_left = list[(x > 0 ? x : list.Width) - 1, y];
			//_right = list[(x < list.Width - 1 ? x + 1 : 0), y];

			_up = list[x, (y > 0 ? y : list.Height) - 1];
			_down = list[x, (y == list.Height - 1 ? 0 : y + 1)];
			_left = list[(x > 0 ? x : list.Width) - 1, y];
			_right = list[(x == list.Width - 1 ? 0 : x + 1), y];

			if (Up != null)
				Up._down = this as T;
			if (Down != null)
				Down._up = this as T;
			if (Left != null)
				Left._right = this as T;
			if (Right != null)
				Right._left = this as T;

			//if (x != 0)
			//{
			//	_left = list[x - 1, y];
			//	(Left as Cell<T>)._right = this as T;
			//	if (torus && X == list.Width - 1)
			//	{
			//		_right = list[0, y];
			//		(Right as Cell<T>)._left = this as T;
			//	}
			//}

			//if (y != 0)
			//{
			//	_up = list[x, y - 1];
			//	(Up as Cell<T>)._down = this as T;
			//	if (torus && Y == list.Height - 1)
			//	{
			//		_down = list[x, 0];
			//		(Down as Cell<T>)._up = this as T;
			//	}
			//}
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
	public abstract class DiagonalCell<T> : Cell<T>
		where T : DiagonalCell<T>
	{
		private T _upLeft, _downLeft, _upRight, _downRight;

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
				if (_upLeft == null)
				{
					if (Up != null)
						_upLeft = Up.Left;
					_upLeft = Left != null ? Left.Up : null;
				}
				return _upLeft;
			}
		}

		public T UpRight
		{
			get
			{
				if (_upRight == null)
				{
					if (Up != null)
						_upRight = Up.Right;
					_upRight = Right != null ? Right.Up : null;
				}
				return _upRight;
			}
		}

		public T DownLeft
		{
			get
			{
				if (_downLeft == null)
				{
					if (Down != null)
						_downLeft = Down.Left;
					_downLeft = Left != null ? Left.Down : null;
				}
				return _downLeft;
			}
		}

		public T DownRight
		{
			get
			{
				if (_downRight == null)
				{
					if (Down != null)
						_downRight = Down.Right;
					_downRight = Right != null ? Right.Down : null;
				}
				return _downRight;
			}
		}
	}

	/// <summary>
	/// Classe permettant la gestion de cellules de type <typeparamref name="T"/> dans une grille
	/// </summary>
	/// <typeparam name="T">Type de cellules, implémentant l'interface <see cref="ICell"/></typeparam>
	public class Grid<T>
		where T : Grid<T>.ICell
	{
		#region Inner Class Cell

		/// <summary>
		/// Interface de base pour les cellules
		/// </summary>
		public interface ICell
		{
			T Up { get; }

			T Down { get; }

			T Left { get; }

			T Right { get; }

			int X { get; }

			int Y { get; }
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
		/// <param name="createCell">constructeur de cellules <typeparamref name="T"/> en fonction de sa position x, y et de la <see cref="Grid{T}"/></param>
		protected Grid(int width, int height, System.Func<int, int, Grid<T>, T> createCell)
		{
			_width = width;
			_height = height;
			_cells = new T[_width,_height];

			Parallel.For(0, _width, x => Parallel.For(0, _height, y => _cells[x, y] = createCell(x, y, this)));

			//for (int i = 0; i < _width; ++i)
			//	for (int j = 0; j < _height; ++j)
			//		_cells[i, j] = createCell(i, j, this);
		}

		public T this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= _width || y >= _height)
					return default(T);
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