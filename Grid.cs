using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace CSharpHelper
{
	/// <summary>
	/// Mode de séléction des cellules voisines
	/// </summary>
	public enum ArroundSelectMode
	{
		/// <summary>
		/// En croix (<see cref="Grid{T}.ICell.Up"/>, <see cref="Grid{T}.ICell.Down"/>, <see cref="Grid{T}.ICell.Left"/> et <see cref="Grid{T}.ICell.Right"/>)
		/// </summary>
		Cross,
		/// <summary>
		/// Tout le tour (<see cref="Cross"/> et <see cref="Diagonal"/>)
		/// </summary>
		Round,
		/// <summary>
		/// En diagonal (les 4 coins)
		/// </summary>
		Diagonal
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight,
		Stay
	}

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
		public Cell(int x, int y, Grid<T> list, bool torus)
		{
			_x = x;
			_y = y;

			if(torus)
			{
				_up = list[x, (y > 0 ? y : list.Height) - 1];
				_down = list[x, (y == list.Height - 1 ? 0 : y + 1)];
				_left = list[(x > 0 ? x : list.Width) - 1, y];
				_right = list[(x == list.Width - 1 ? 0 : x + 1), y];
			}
			else
			{
				_up = (y > 0) ? list[x, y - 1] : null;
				_down = (y <= list.Height - 1) ? list[x, y + 1] : null;
				_left = (x > 0) ? list[x - 1, y] : null;
				_right = (x <= list.Width - 1) ? list[x + 1, y] : null;
			}

			if (Up != null)
				Up._down = this as T;
			if (Down != null)
				Down._up = this as T;
			if (Left != null)
				Left._right = this as T;
			if (Right != null)
				Right._left = this as T;
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

		/// <summary>
		/// liste toutes les cellules <typeparamref name="T"/> voisines en fonction du mode de sélection
		/// </summary>
		/// <param name="mode">mode de sélection des cellules voisines</param>
		/// <returns>la liste des cellules voisines non nulles</returns>
		public virtual IEnumerable<T> Arround(ArroundSelectMode mode)
		{
			if(mode != ArroundSelectMode.Diagonal)
			{
				if (Up != null)
					yield return Up;
				if (Down != null)
					yield return Down;
				if (Left != null)
					yield return Left;
				if (Right != null)
					yield return Right;
			}

			if (mode == ArroundSelectMode.Cross)
				yield break;

			if (Up != null && Up.Left != null)
				yield return Up.Left;
			if (Up != null && Up.Right != null)
				yield return Up.Right;
			if (Down != null && Down.Left != null)
				yield return Down.Left;
			if (Down != null && Down.Right != null)
				yield return Down.Right;
		}

		public virtual T this[Direction direction]
		{
			get
			{
				switch (direction)
				{
					case Direction.Up:
						return Up;
					case Direction.Down:
						return Down;
					case Direction.Left:
						return Left;
					case Direction.Right:
						return Right;
					case Direction.UpLeft:
						if (Up != null)
							return Up.Left;
						if (Left != null)
							return Left.Up;
						break;
					case Direction.UpRight:
						if (Up != null)
							return Up.Right;
						if (Right != null)
							return Right.Up;
						break;
					case Direction.DownLeft:
						if (Down != null)
							return Down.Left;
						if (Left != null)
							return Left.Down;
						break;
					case Direction.DownRight:
						if (Down != null)
							return Down.Right;
						if (Right != null)
							return Right.Down;
						break;
					case Direction.Stay:
						return this as T;
					default:
						throw new ArgumentOutOfRangeException("direction");
				}
				return null;
			}
		}
	}

	/// <summary>
	/// classe de cellules basique gérant les diagonales
	/// </summary>
	/// <typeparam name="T">Type de cellules, héritant de la classe <see cref="DiagonalCell{T}"/></typeparam>
	public class DiagonalCell<T> : Cell<T>
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
		public DiagonalCell(int x, int y, Grid<T> list, bool torus)
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
					else
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
					else
						_upRight = (Right != null ? Right.Up : null);
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
					else
						_downLeft = (Left != null ? Left.Down : null);
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
					else
						_downRight = (Right != null ? Right.Down : null);
				}
				return _downRight;
			}
		}

		public override T this[Direction direction]
		{
			get
			{
				switch (direction)
				{
					case Direction.Up:
						return Up;
					case Direction.Down:
						return Down;
					case Direction.Left:
						return Left;
					case Direction.Right:
						return Right;
					case Direction.UpLeft:
						return UpLeft;
					case Direction.UpRight:
						return UpRight;
					case Direction.DownLeft:
						return DownLeft;
					case Direction.DownRight:
						return DownRight;
					case Direction.Stay:
						return this as T;
					default:
						throw new ArgumentOutOfRangeException("direction");
				}
			}
		}

		/// <summary>
		/// liste toutes les cellules <typeparamref name="T"/> voisines en fonction du mode de sélection
		/// </summary>
		/// <param name="mode">mode de sélection des cellules voisines</param>
		/// <returns>la liste des cellules voisines non nulles</returns>
		public override IEnumerable<T> Arround(ArroundSelectMode mode)
		{
			if (mode != ArroundSelectMode.Diagonal)
			{
				if (Up != null)
					yield return Up;
				if (Down != null)
					yield return Down;
				if (Left != null)
					yield return Left;
				if (Right != null)
					yield return Right;
			}

			if (mode == ArroundSelectMode.Cross)
				yield break;

			if (UpLeft != null)
				yield return UpLeft;
			if (UpRight != null)
				yield return UpRight;
			if (DownLeft != null)
				yield return DownLeft;
			if (DownRight != null)
				yield return DownRight;
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

			int Y { get;}

			IEnumerable<T> Arround(ArroundSelectMode mode);

			T this[Direction direction]
			{
				get;
			}
		}

		public interface IPathCell
		{
			int CellCost { get; }

			bool CanWalk { get; }
		}

		public struct DirectionalCell
		{
			private readonly T _cell;
			private readonly Direction _direction;

			public DirectionalCell(T cell, Direction direction)
			{
				_cell = cell;
				_direction = direction;
			}

			public DirectionalCell(T cell, T nextCell)
			{
				_cell = cell;
				if (Equals(cell, default(T)))
				{
					_direction = Direction.Stay;
					return;
				}
				Point direction = new Point(nextCell.X - _cell.X, nextCell.Y - _cell.Y);

				_direction = Direction.Stay;
				if (direction.X < 0)
				{
					if (direction.Y < 0)
						_direction = Direction.UpLeft;
					else if (direction.Y == 0)
						_direction = Direction.Left;
					else if (direction.Y > 0)
						_direction = Direction.DownLeft;
				}
				else if (direction.X == 0)
				{
					if (direction.Y < 0)
						_direction = Direction.Up;
					else if (direction.Y == 0)
						_direction = Direction.Stay;
					else if (direction.Y > 0)
						_direction = Direction.Down;
				}
				else if (direction.X > 0)
				{
					if (direction.Y < 0)
						_direction = Direction.UpRight;
					else if (direction.Y == 0)
						_direction = Direction.Right;
					else if (direction.Y > 0)
						_direction = Direction.DownRight;
				}
			}

			public T Cell
			{
				get { return _cell; }
			}

			public Direction Direction
			{
				get { return _direction; }
			}
		}

	public class NodeProxy<U>
		where U : T, IPathCell
	{
		private readonly Node<U> _node;

		public NodeProxy(Node<U> node)
		{
			_node = node;
		}

		public Node<U> Parent
		{
			get { return _node.Parent; }
		}

		public U Cell
		{
			get { return _node.Cell; }
		}

		public int Value
		{
			get { return _node.Value; }
		}
	}

	[DebuggerDisplay("Cell: {Cell}, Val: {Value}")]
	[DebuggerTypeProxy(typeof(NodeProxy<>))]
	public class Node<U>
			where U : T, IPathCell
		{
			public Node<U> Parent { get; private set; }
			public U Cell { get; private set; }
			public int Value { get; private set; }

			public Node(int x, int y)
			{
				Parent = null;
				Cell = default(U);
				Value = int.MaxValue;
			}

			public void Modify(Node<U> parent, U cell, int value)
			{
				Parent = parent;
				Cell = cell;
				Value = value;
			}
		}

		#endregion

		private readonly T[,] _cells;
		private readonly int _width, _height;
		private readonly CreateCell _createCell;
		private int _total;

		/// <summary>
		/// Délégate de création de cellules
		/// </summary>
		/// <param name="x">Position en X</param>
		/// <param name="y">Position en Y</param>
		/// <param name="list">Liste contenant toutes les <see cref="ICell"/></param>
		/// <returns></returns>
		public delegate T CreateCell(int x, int y, Grid<T> list);

		/// <summary>
		/// Constructeur de la grille
		/// </summary>
		/// <param name="width">Nombre de cellules horizontalement</param>
		/// <param name="height">Nombre de cellules verticalement</param>
		/// <param name="createCell">constructeur de cellules <typeparamref name="T"/> en fonction de sa position x, y et de la <see cref="Grid{T}"/></param>
		protected Grid(int width, int height, CreateCell createCell)
		{
			_width = width;
			_height = height;
			_createCell = createCell;
			_total = _width * _height;
			_cells = new T[_width,_height];

			//Parallel.For(0, _width, x => Parallel.For(0, _height, y => _cells[x, y] = createCell(x, y, this)));

			for (int x = 0; x < _width; ++x)
				for (int y = 0; y < _height; ++y)
					_cells[x, y] = _createCell(x, y, this);
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

		public int Total
		{
			get { return _total; }
		}

		private static Node<TPathCell> AStar<TPathCell>(Node<TPathCell> parent, TPathCell end, Func<TPathCell, TPathCell, bool> canWalk, ArroundSelectMode mode, int minStep, Node<TPathCell>[,] map, int value)
			where TPathCell : T, IPathCell
		{
			foreach (var t in parent.Cell.Arround(mode).Cast<TPathCell>())
			{
				var node = map[t.X, t.Y];

				if (Equals(t, end))
				{
					if (node.Value > value)
						node.Modify(parent, t, value + t.CellCost);
				}

				if (canWalk(t, parent.Cell) && node.Value > value)
				{
					node.Modify(parent, t, value + t.CellCost);
					var n = AStar(node, end, canWalk, mode, minStep, map, value + 1);
					if (n != null)
						return n;
				}
			}
			return null;
		}

		public IEnumerable<DirectionalCell> AStar<TPathCell>(TPathCell start, TPathCell end, Func<TPathCell, TPathCell, bool> canWalk, ArroundSelectMode mode)
			where TPathCell : T, IPathCell
		{
			if (Equals(start, end))
				return new DirectionalCell[0];

			Node<TPathCell>[,] map = new Node<TPathCell>[Width, Height];
			for (int x = 0; x < Width; ++x)
				for (int y = 0; y < Height; ++y)
					map[x, y] = new Node<TPathCell>(x, y);

			Node<TPathCell> startNode = map[start.X, start.Y];
			startNode.Modify(null, start, 0);

			AStar(startNode, end, canWalk, mode, (int)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2)), map, 1);

			IList<DirectionalCell> result = new List<DirectionalCell>();
			var endNode = map[end.X, end.Y];
			if (endNode.Parent != null)
				result.Add(new DirectionalCell(endNode.Cell, Direction.Stay));
			for (Node<TPathCell> n = endNode; n != null; n = n.Parent)
				if (!Equals(n.Cell, default(TPathCell)) && n.Parent != null)
					result.Add(new DirectionalCell(n.Parent.Cell, n.Cell));
			if (result.Count == 0)
				throw new Exception("Impossible de trouver un chemin");
			return result.Reverse();
		}

		public IEnumerable<T> Range(T cell, int r)
		{
			List<T> cells = new List<T>();
			while (r > 0)
				foreach (T c in Circle(cell, r--).Where(c => !cells.Contains(c)))
				{
					yield return c;
					cells.Add(c);
				}
		}

		public IEnumerable<T> Circle(T cell, int r)
		{
			if (r < 0)
				yield break;

			int x = 0;
			int y = r;
			int d = r - 1;

			List<T> list = new List<T>
				{
					cell
				};

			while (y >= x)
			{
				Point p2 = new Point(x + cell.X, y + cell.Y);
				var c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(y + cell.X, x + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(-x + cell.X, y + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(-y + cell.X, x + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(x + cell.X, -y + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(y + cell.X, -x + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(-x + cell.X, -y + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}

				p2 = new Point(-y + cell.X, -x + cell.Y);
				c = this[p2.X, p2.Y];
				if (!Equals(c, default(T)))
					if (!list.Contains(this[p2.X, p2.Y]))
					{
						list.Add(this[p2.X, p2.Y]);
						yield return this[p2.X, p2.Y];
					}


				if (d >= 2 * (x - 1))
				{
					d -= 2 * x;
					x++;
				}
				else if (d <= 2 * (r - y))
				{
					d += 2 * y - 1;
					y--;
				}
				else
				{
					d += 2 * (y - x - 1);
					y--;
					x++;
				}
			}
		}
	}
}