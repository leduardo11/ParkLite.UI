using System.Numerics;
using ParkLite.UI.Interfaces;
using Raylib_cs;

namespace ParkLite.UI.Widgets
{
	public class Table : IObject
	{
		public Rectangle Bounds { get; private set; }
		public int Rows { get; private set; }
		public int Columns { get; private set; }
		public string[,] Data { get; private set; }
		public int TextSize { get; private set; }

		private float[] _columnWidths;
		private float _cellHeight;
		private Color[,] _textColors;
		private Color[,] _bgColors;

		private const float MinColumnWidth = 40.0f;

		public Table(Rectangle bounds, int rows, int columns, int textSize = Constants.DefaultTextSize)
		{
			Bounds = bounds;
			Rows = rows;
			Columns = columns;
			TextSize = textSize;

			Data = new string[rows, columns];
			_columnWidths = new float[columns];
			_cellHeight = bounds.Height / rows;
			_textColors = new Color[Rows, Columns];
			_bgColors = new Color[Rows, Columns];

			InitDefaultCellColors(Constants.ButtonTextColor, Color.Blank);
			CalculateColumnWidths();
		}

		public Rectangle GetCellRect(int row, int col)
		{
			float x = Bounds.X;
			for (int i = 0; i < col; i++)
				x += _columnWidths[i];

			float y = Bounds.Y + row * _cellHeight;
			float width = _columnWidths[col];

			return new Rectangle(x, y, width, _cellHeight);
		}

		public string[] GetColumns()
		{
			if (Rows == 0) return [];
			var result = new string[Columns];

			for (int c = 0; c < Columns; c++)
				result[c] = Data[0, c];

			return result;
		}

		public void CalculateColumnWidths()
		{
			for (int col = 0; col < Columns; col++)
			{
				float maxWidth = MinColumnWidth;

				for (int row = 0; row < Rows; row++)
				{
					string text = Data[row, col] ?? "";
					float width = Raylib.MeasureText(text, TextSize) + 20;
					if (width > maxWidth) maxWidth = width;
				}
				_columnWidths[col] = maxWidth;
			}
		}

		public void SetCellTextColor(int row, int col, Color color)
		{
			if (row >= 0 && row < Rows && col >= 0 && col < Columns)
				_textColors[row, col] = color;
		}

		public void SetCellBackgroundColor(int row, int col, Color color)
		{
			if (row >= 0 && row < Rows && col >= 0 && col < Columns)
				_bgColors[row, col] = color;
		}

		public void SetCell(int row, int column, string text)
		{
			if (row >= 0 && row < Rows && column >= 0 && column < Columns)
				Data[row, column] = text;
		}

		public void SetData<T>(IList<T> items, string[] columns, Func<T, object>[] selectors)
		{
			Rows = items.Count + 1;
			Columns = columns.Length;

			UpdateDimensions();

			Data = new string[Rows, Columns];
			_textColors = new Color[Rows, Columns];
			_bgColors = new Color[Rows, Columns];

			for (int c = 0; c < Columns; c++)
				Data[0, c] = columns[c];

			for (int r = 0; r < items.Count; r++)
				for (int c = 0; c < Columns; c++)
					Data[r + 1, c] = selectors[c](items[r])?.ToString() ?? "";

			InitDefaultCellColors(Constants.ButtonTextColor, Color.Blank);
			CalculateColumnWidths();
		}

		public bool IsCellClicked(int row, int col, Vector2 mousePosition)
		{
			if (row < 0 || row >= Rows || col < 0 || col >= Columns)
				return false;

			float y = Bounds.Y + row * _cellHeight;
			float x = Bounds.X;

			for (int i = 0; i < col; i++)
				x += _columnWidths[i];

			float width = _columnWidths[col];
			var cellRect = new Rectangle(x, y, width, _cellHeight);

			return Raylib.CheckCollisionPointRec(mousePosition, cellRect);
		}

		public void Update()
		{
		}

		public void Draw()
		{
			float y = Bounds.Y;

			for (int row = 0; row < Rows; row++)
			{
				float x = Bounds.X;

				for (int col = 0; col < Columns; col++)
				{
					float width = _columnWidths[col];
					var cellBounds = new Rectangle(x, y, width, _cellHeight);

					if (row == 0)
						Raylib.DrawRectangleRec(cellBounds, Constants.TableHeaderColor);
					else if (_bgColors[row, col].A > 0)
						Raylib.DrawRectangleRec(cellBounds, _bgColors[row, col]);

					DrawHoverOverlay(cellBounds);

					Raylib.DrawRectangleLinesEx(cellBounds, 1, Constants.GridLineColor);
					TextUtils.DrawCenteredText(Data[row, col] ?? "", cellBounds, TextSize, _textColors[row, col]);

					x += width;
				}
				y += _cellHeight;
			}
		}

		public static Table CreateDefaultTable(Vector2 position, Vector2 size, int rows, int columns, int textSize = Constants.DefaultTextSize)
		{
			var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
			return new Table(bounds, rows, columns, textSize);
		}

		private void InitDefaultCellColors(Color textColor, Color bgColor)
		{
			for (int row = 0; row < Rows; row++)
				for (int col = 0; col < Columns; col++)
				{
					_textColors[row, col] = textColor;
					_bgColors[row, col] = bgColor;
				}
		}

		private void UpdateDimensions()
		{
			_columnWidths = new float[Columns];
			_cellHeight = Bounds.Height / Rows;
		}

		private static void DrawHoverOverlay(Rectangle cellBounds)
		{
			var mousePos = Raylib.GetMousePosition();

			if (Raylib.CheckCollisionPointRec(mousePos, cellBounds))
			{
				Raylib.DrawRectangleRec(cellBounds, Constants.HoverOverlayColor);
			}
		}
	}
}
