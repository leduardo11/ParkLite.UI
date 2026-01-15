using System.Numerics;
using ParkLite.UI.Interfaces;
using Raylib_cs;

namespace ParkLite.UI.Widgets
{
	public class Button : IWidget
	{
		public Rectangle Bounds { get; private set; }
		public string Text { get; set; } = string.Empty;
		public int TextSize { get; private set; }
		public Action<Button>? OnClick { get; private set; }
		public bool IsHovered { get; private set; }
		public bool IsPressed { get; private set; }
		public object? Tag { get; private set; }

		public bool Enabled { get; set; } = true;
		public bool Visible { get; set; } = true;

		private static Sound? _clickSound;

		public static void LoadClickSound(string path)
		{
			_clickSound = Raylib.LoadSound(path);
		}

		public static void UnloadClickSound()
		{
			if (_clickSound is not null)
			{
				Raylib.UnloadSound(_clickSound.Value);
				_clickSound = null;
			}
		}

		private Button(string text, Rectangle bounds, Action<Button>? onClick, int textSize, object? tag)
		{
			Bounds = bounds;
			Text = text;
			TextSize = textSize;
			Tag = tag;
			OnClick = onClick;
		}

		public void Update()
		{
			if (!Enabled || !Visible)
			{
				IsHovered = false;
				IsPressed = false;
				return;
			}

			var mousePosition = Raylib.GetMousePosition();

			IsHovered = this.HitTest(mousePosition);
			IsPressed = IsHovered && Raylib.IsMouseButtonDown(MouseButton.Left);

			if (IsHovered && Raylib.IsMouseButtonReleased(MouseButton.Left))
			{
				if (_clickSound is not null)
					Raylib.PlaySound(_clickSound.Value);

				OnClick?.Invoke(this);
			}
		}

		public void Draw()
		{
			if (!Visible)
				return;

			var bg =
				IsPressed ? Constants.ButtonColorPressed :
				IsHovered ? Constants.ButtonColorHover :
				Constants.ButtonColorNormal;

			Raylib.DrawRectangleRec(Bounds, bg);

			// Small shaking effect for clicking
			int offset = IsPressed ? Constants.TextOffset : 0;

			var lines = TextUtils.BreakTextInTwo(Text, TextSize, Bounds.Width);
			DrawCenteredOneOrTwoLines(lines[0], lines[1], Bounds, TextSize, Constants.ButtonTextColor, offset);
		}

		private static void DrawCenteredOneOrTwoLines(
			string line1,
			string line2,
			Rectangle bounds,
			int textSize,
			Color color,
			int yOffset = 0
		)
		{
			if (string.IsNullOrEmpty(line2))
			{
				TextUtils.DrawCenteredText(line1, bounds, textSize, color, yOffset);
				return;
			}

			int totalHeight = textSize * 2;
			int startY = (int)(bounds.Y + (bounds.Height - totalHeight) / 2) + yOffset;

			int w1 = Raylib.MeasureText(line1, textSize);
			int w2 = Raylib.MeasureText(line2, textSize);

			int x1 = (int)(bounds.X + (bounds.Width - w1) / 2);
			int x2 = (int)(bounds.X + (bounds.Width - w2) / 2);

			Raylib.DrawText(line1, x1, startY, textSize, color);
			Raylib.DrawText(line2, x2, startY + textSize, textSize, color);
		}

		public static Button CreateDefaultBtn(
			string text,
			Vector2 position,
			Vector2? size = null,
			Action<Button>? onClick = null,
			int textSize = Constants.DefaultTextSize,
			object? tag = null
		)
		{
			var btnSize = size ?? new Vector2(Constants.DefaultButtonWidth, Constants.DefaultButtonHeight);
			var bounds = new Rectangle(position.X, position.Y, btnSize.X, btnSize.Y);
			return new Button(text, bounds, onClick, textSize, tag);
		}
	}
}
