using System.Numerics;
using ParkLite.Interfaces;
using Raylib_cs;

namespace ParkLite.UI
{
	public class Table : IObject
	{
		public Rectangle Bounds { get; private set; }
		public int Rows { get; private set; }
		public int Columns { get; private set; }
		public string[,] Data { get; private set; }
		public int TextSize { get; private set; }

		private readonly float[] _columnWidths;
		private readonly float _cellHeight;
		private readonly Color[,] _textColors;
		private readonly Color[,] _bgColors;

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

		public void CalculateColumnWidths()
		{
			for (int col = 0; col < Columns; col++)
			{
				float maxWidth = 0;
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

					Raylib.DrawRectangleLinesEx(cellBounds, 1, Constants.GridLineColor);

					if (_bgColors[row, col].A > 0 || row == 0)
						Raylib.DrawRectangleRec(cellBounds, row == 0 ? Constants.TableHeaderColor : _bgColors[row, col]);

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
	}
}
