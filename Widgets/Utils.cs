using Raylib_cs;

namespace ParkLite.UI.Widgets
{
	public static class TextUtils
	{
		private static readonly Dictionary<KeyboardKey, double> _nextRepeatTimes = [];
		private static readonly HashSet<KeyboardKey> _heldKeys = [];
		private const double InitialDelay = 0.5;
		private const double RepeatRate = 0.05;

		public static bool ShouldRepeatKey(KeyboardKey key)
		{
			double now = Raylib.GetTime();

			if (Raylib.IsKeyDown(key))
			{
				if (!_heldKeys.Contains(key))
				{
					_heldKeys.Add(key);
					_nextRepeatTimes[key] = now + InitialDelay;
					return true;
				}

				if (now >= _nextRepeatTimes[key])
				{
					_nextRepeatTimes[key] = now + RepeatRate;
					return true;
				}
			}
			else
			{
				_heldKeys.Remove(key);
				_nextRepeatTimes.Remove(key);
			}

			return false;
		}

		public static void DrawWrappedText(string text, Rectangle bounds, int textSize, Color color, float padding = 5, float lineSpacing = 2)
		{
			float x = bounds.X + padding;
			float y = bounds.Y + padding;
			float maxWidth = bounds.Width - padding * 2;
			string currentLine = "";

			foreach (char c in text)
			{
				string testLine = currentLine + c;
				int testWidth = Raylib.MeasureText(testLine, textSize);

				if (testWidth > maxWidth)
				{
					Raylib.DrawText(currentLine, (int)x, (int)y, textSize, color);
					y += textSize + lineSpacing;
					currentLine = c.ToString();
				}
				else
				{
					currentLine = testLine;
				}
			}

			if (!string.IsNullOrEmpty(currentLine))
			{
				Raylib.DrawText(currentLine, (int)x, (int)y, textSize, color);
			}
		}

		public static void DrawCaret(string text, Rectangle bounds, int textSize, float blinkTime, float padding, Color color, bool isFocused)
		{
			if (!isFocused || ((int)(blinkTime * 2) % 2) != 0) return;

			float x = bounds.X + padding + Raylib.MeasureText(text, textSize);
			float y = bounds.Y + padding;

			Raylib.DrawRectangle((int)x, (int)y, 1, textSize, color);
		}

		public static string[] BreakTextInTwo(string text, int textSize, float maxWidth)
		{
			int fullWidth = Raylib.MeasureText(text, textSize);

			if (fullWidth <= maxWidth)
				return [text, ""];

			int mid = text.Length / 2;
			int split = text.LastIndexOf(' ', mid);

			if (split <= 0) split = mid;

			string line1 = text[..split].Trim();
			string line2 = text[split..].Trim();
			return [line1, line2];
		}

		public static void DrawCenteredText(string text, Rectangle bounds, int textSize, Color color, int verticalOffset = 0)
		{
			int textWidth = Raylib.MeasureText(text, textSize);
			int textX = (int)(bounds.X + (bounds.Width - textWidth) / 2);
			int textY = (int)(bounds.Y + (bounds.Height - textSize) / 2) + verticalOffset;

			Raylib.DrawText(text, textX, textY, textSize, color);
		}
	}
}
