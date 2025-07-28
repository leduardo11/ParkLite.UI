using System.Numerics;
using ParkLite.Interfaces;
using Raylib_cs;

namespace ParkLite.UI
{
	public class TextInput : IObject
	{
		public Rectangle Bounds { get; private set; }
		public string Text { get; set; } = string.Empty;
		public int TextSize { get; private set; }
		public bool IsFocused { get; private set; }
		public object? Tag { get; private set; }

		private float _caretTime;

		private TextInput(Rectangle bounds, int textSize, object? tag)
		{
			Bounds = bounds;
			TextSize = textSize;
			Tag = tag;
		}

		public void Update()
		{
			var mousePosition = Raylib.GetMousePosition();

			if (Raylib.IsMouseButtonPressed(MouseButton.Left))
				IsFocused = Raylib.CheckCollisionPointRec(mousePosition, Bounds);

			if (IsFocused)
			{
				_caretTime += Raylib.GetFrameTime();
				int key = Raylib.GetCharPressed();

				while (key > 0)
				{
					Text += (char)key;
					key = Raylib.GetCharPressed();
				}

				if (TextUtils.ShouldRepeatKey(KeyboardKey.Backspace) && Text.Length > 0)
					Text = Text[..^1];
			}
			else
			{
				_caretTime = 0.0f;
			}
		}

		public void Draw()
		{
			Color bg = IsFocused ? Constants.ButtonColorHover : Constants.ButtonColorNormal;
			Raylib.DrawRectangleRec(Bounds, bg);

			TextUtils.DrawWrappedText(
				text: Text,
				bounds: Bounds,
				textSize: TextSize,
				color: Constants.ButtonTextColor,
				padding: Constants.TextInputPadding,
				lineSpacing: Constants.TextInputLineSpacing
			);

			TextUtils.DrawCaret(Text, Bounds, TextSize, _caretTime, Constants.TextInputPadding, Constants.CaretColor, IsFocused);
		}

		public static TextInput CreateDefaultInput(
			Vector2 position,
			Vector2 size,
			int textSize = Constants.DefaultTextSize,
			object? tag = null
		)
		{
			var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
			return new TextInput(bounds, textSize, tag);
		}
	}
}
