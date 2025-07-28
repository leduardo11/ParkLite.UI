using Raylib_cs;

namespace ParkLite
{
	public static class Constants
	{
		public const int ScreenWidth = 800;
		public const int ScreenHeight = 600;
		public const int TargetFPS = 60;
		public const int DefaultTextSize = 20;
		public const int TextInputPadding = 5;
		public const int TextInputLineSpacing = 2;
		public const int TextOffset = 2;
		public const int LineOffset = 5;

		public const string ScreenTitle = "ParkLiteUI";

		public static readonly Color ButtonColorNormal = Color.Gray;
		public static readonly Color ButtonColorHover = Color.DarkGray;
		public static readonly Color ButtonColorPressed = new(40, 40, 40, 255);
		public static readonly Color ButtonTextColor = Color.RayWhite;
		public static readonly Color CaretColor = Color.RayWhite;
		public static readonly Color GridLineColor = Color.RayWhite;
		public static readonly Color TableHeaderColor = new(40, 40, 40, 255);
	}
}