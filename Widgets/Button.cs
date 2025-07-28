using System.Numerics;
using ParkLite.UI.Interfaces;
using Raylib_cs;

namespace ParkLite.UI.Widgets
{
	public class Button : IObject
	{
		public Rectangle Bounds { get; private set; }
		public string Text { get; set; } = string.Empty;
		public int TextSize { get; private set; }
		public Action<Button>? OnClick { get; private set; }
		public bool IsHovered { get; private set; }
		public bool IsPressed { get; private set; }
		public object? Tag { get; private set; }

		private Button(Rectangle bounds, string text, Action<Button>? onClick, int textSize, object? tag)
		{
			Bounds = bounds;
			Text = text;
			TextSize = textSize;
			Tag = tag;
			OnClick = onClick;
		}

		public void Update()
		{
			var mousePosition = Raylib.GetMousePosition();
			IsHovered = Raylib.CheckCollisionPointRec(mousePosition, Bounds);
			IsPressed = IsHovered && Raylib.IsMouseButtonDown(MouseButton.Left);

			if (IsHovered && Raylib.IsMouseButtonReleased(MouseButton.Left))
			{
				OnClick?.Invoke(this);
			}
		}

		public void Draw()
		{
			Color bg = Constants.ButtonColorNormal;

			if (IsPressed)
				bg = Constants.ButtonColorPressed;
			else if (IsHovered)
				bg = Constants.ButtonColorHover;

			Raylib.DrawRectangleRec(Bounds, bg);

			//Small shaking effect for clicking
			int offset = IsPressed ? Constants.TextOffset : 0;

			var textLines = TextUtils.BreakTextInTwo(Text, TextSize, Bounds.Width);

			if (string.IsNullOrEmpty(textLines[1]))
			{
				TextUtils.DrawCenteredText(textLines[0], Bounds, TextSize, Constants.ButtonTextColor, offset);
			}
			else
			{
				int totalHeight = TextSize * 2;
				int textY = (int)(Bounds.Y + (Bounds.Height - totalHeight) / 2) + offset;
				int line1Width = Raylib.MeasureText(textLines[0], TextSize);
				int line2Width = Raylib.MeasureText(textLines[1], TextSize);
				int line1X = (int)(Bounds.X + (Bounds.Width - line1Width) / 2);
				int line2X = (int)(Bounds.X + (Bounds.Width - line2Width) / 2);

				Raylib.DrawText(textLines[0], line1X, textY, TextSize, Constants.ButtonTextColor);
				Raylib.DrawText(textLines[1], line2X, textY + TextSize, TextSize, Constants.ButtonTextColor);
			}
		}

		public static Button CreateDefaultBtn(
			Vector2 position,
			Vector2 size,
			string text,
			Action<Button>? onClick = null,
			int textSize = Constants.DefaultTextSize,
			object? tag = null
		)
		{
			var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
			return new Button(bounds, text, onClick, textSize, tag);
		}
	}
}
