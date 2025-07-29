using System.Numerics;
using ParkLite.UI.Interfaces;
using Raylib_cs;

namespace ParkLite.UI.Widgets
{
	public class Table : IObject
	{
		public Rectangle Bounds { get; private set; }
		public int Rows => _cells.Count;
		public int Columns { get; private set; }
		public int TextSize { get; private set; }

		private float[] _columnWidths;
		private float _cellHeight;
		private readonly List<TableRow> _cells = [];
		private (int row, int col)? _hoveredCell;
		private Vector2 _mousePos;

		public Table(Rectangle bounds, int rows, int columns, int textSize = Constants.DefaultTextSize)
		{
			Bounds = bounds;
			Columns = columns;
			TextSize = textSize;

			_columnWidths = new float[columns];
			_cellHeight = bounds.Height / rows;

			for (int i = 0; i < rows; i++)
				_cells.Add(new TableRow(columns, Constants.ButtonTextColor, Color.Blank));
		}

		public static Table CreateDefaultTable(Vector2 position, Vector2 size, int rows, int columns, int textSize = Constants.DefaultTextSize)
		{
			var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
			return new Table(bounds, rows, columns, textSize);
		}

		public void SetCell(int row, int col, string text, Color? textColor = null, Color? bgColor = null)
		{
			if (row >= 0 && row < Rows && col >= 0 && col < Columns)
				_cells[row].Cells[col] = new TableCell(text, textColor ?? Constants.ButtonTextColor, bgColor ?? Color.Blank);
		}

		public void SetCellTextColor(int row, int col, Color color)
		{
			if (row >= 0 && row < Rows && col >= 0 && col < Columns)
				_cells[row].Cells[col].TextColor = color;
		}

		public void SetCellBackgroundColor(int row, int col, Color color)
		{
			if (row >= 0 && row < Rows && col >= 0 && col < Columns)
				_cells[row].Cells[col].BgColor = color;
		}

		public void SetData<T>(IList<T> items, string[] headers, Func<T, object>[] selectors)
		{
			CellsClear();
			Columns = headers.Length;
			_cellHeight = Bounds.Height / (items.Count + 1);
			_columnWidths = new float[Columns];

			var header = new TableRow(Columns, Constants.ButtonTextColor, Constants.TableHeaderColor, true);
			for (int c = 0; c < Columns; c++)
				header.Cells[c] = new TableCell(headers[c], Constants.ButtonTextColor, Constants.TableHeaderColor);
			_cells.Add(header);

			foreach (var item in items)
			{
				var row = new TableRow(Columns, Constants.ButtonTextColor, Color.Blank);
				for (int c = 0; c < Columns; c++)
					row.Cells[c] = new TableCell(selectors[c](item)?.ToString() ?? "", Constants.ButtonTextColor, Color.Blank);
				_cells.Add(row);
			}

			CalculateColumnWidths();
		}

		public void Draw()
		{
			if (_cells.Count == 0) return;

			float y = Bounds.Y;

			for (int r = 0; r < Rows; r++)
			{
				float x = Bounds.X;

				for (int c = 0; c < Columns; c++)
				{
					var cell = _cells[r].Cells[c];
					var rect = GetCellRect(r, c);

					if (cell.BgColor.A > 0)
						Raylib.DrawRectangleRec(rect, cell.BgColor);

					if (_cells[r].IsHeader)
						Raylib.DrawRectangleRec(rect, Constants.TableHeaderColor);

					if (_hoveredCell == (r, c))
						Raylib.DrawRectangleRec(rect, Constants.HoverOverlayColor);

					Raylib.DrawRectangleLinesEx(rect, 1, Constants.GridLineColor);
					TextUtils.DrawCenteredText(cell.Text, rect, TextSize, cell.TextColor);

					x += _columnWidths[c];
				}
				y += _cellHeight;
			}
		}

		public void Update()
		{
			_mousePos = Raylib.GetMousePosition();
			_hoveredCell = null;

			for (int r = 0; r < Rows; r++)
			{
				for (int c = 0; c < Columns; c++)
				{
					if (Raylib.CheckCollisionPointRec(_mousePos, GetCellRect(r, c)))
					{
						_hoveredCell = (r, c);
						return;
					}
				}
			}
		}

		public bool IsCellClicked(int row, int col, Vector2? mouseOverride = null)
		{
			if (row < 0 || row >= Rows || col < 0 || col >= Columns)
				return false;

			var pos = mouseOverride ?? _mousePos;
			return Raylib.CheckCollisionPointRec(pos, GetCellRect(row, col));
		}

		private Rectangle GetCellRect(int row, int col)
		{
			float x = Bounds.X;
			for (int i = 0; i < col; i++)
				x += _columnWidths[i];

			float y = Bounds.Y + row * _cellHeight;
			return new Rectangle(x, y, _columnWidths[col], _cellHeight);
		}

		private void CalculateColumnWidths()
		{
			for (int c = 0; c < Columns; c++)
			{
				float max = Constants.MinColumnWidth;

				foreach (var row in _cells)
				{
					string text = row.Cells[c].Text ?? "";
					float w = Raylib.MeasureText(text, TextSize) + 20;
					if (w > max) max = w;
				}
				_columnWidths[c] = max;
			}
		}

		private void CellsClear() => _cells.Clear();

		private class TableCell(string text, Color textColor, Color bgColor)
		{
			public string Text = text;
			public Color TextColor = textColor;
			public Color BgColor = bgColor;
		}

		private class TableRow(int cols, Color defaultText, Color defaultBg, bool isHeader = false)
		{
			public List<TableCell> Cells { get; } = [.. Enumerable.Range(0, cols).Select(_ => new TableCell("", defaultText, defaultBg))];
			public bool IsHeader { get; } = isHeader;
		}
	}
}
