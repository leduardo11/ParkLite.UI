using Raylib_cs;

namespace ParkLite.UI
{
	public static class ColorPalette
	{
		public static Color DarkCustomGray => new(40, 40, 40, 255);
		public static Color SemiTransparentCustom => new(255, 255, 255, 80);
	}

	public static class Constants
	{
		public const int ScreenWidth = 1200;
		public const int ScreenHeight = 900;
		public const int TargetFPS = 60;
		public const int DefaultTextSize = 20;
		public const int TextInputPadding = 5;
		public const int TextInputLineSpacing = 2;
		public const int TextOffset = 3;
		public const int LineOffset = 5;

		public const float MinColumnWidth = 40.0f;
		public const float DefaultButtonWidth = 120.0f;
		public const float DefaultButtonHeight = 50.0f;


		public const string ScreenTitle = "ParkLiteUI";
		public const string ClickSoundPath = "Assets/Sounds/click.wav";

		public static readonly Color ButtonColorNormal = Color.Gray;
		public static readonly Color ButtonColorHover = Color.DarkGray;
		public static readonly Color ButtonColorPressed = ColorPalette.DarkCustomGray;
		public static readonly Color ButtonTextColor = Color.RayWhite;
		public static readonly Color CaretColor = Color.RayWhite;
		public static readonly Color GridLineColor = Color.RayWhite;
		public static readonly Color TableHeaderColor = ColorPalette.DarkCustomGray;
		public static readonly Color HoverOverlayColor = ColorPalette.SemiTransparentCustom;
	}
}