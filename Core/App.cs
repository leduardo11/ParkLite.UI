using Raylib_cs;
using ParkLite.UI.Core.Views;
using ParkLite.UI.Interfaces;
using ParkLite.UI.Widgets;

namespace ParkLite.UI.Core
{
	public class App
	{
		private IObject _currentView;

		public App()
		{
			Raylib.InitWindow(Constants.ScreenWidth, Constants.ScreenHeight, Constants.ScreenTitle);
			Raylib.SetWindowState(ConfigFlags.ResizableWindow);
			Raylib.SetTargetFPS(Constants.TargetFPS);
			Raylib.InitAudioDevice();
			Button.LoadClickSound(Constants.ClickSoundPath);

			_currentView = new AccountListView(this);
		}

		public void Run()
		{
			while (!Raylib.WindowShouldClose())
			{
				_currentView.Update();

				Raylib.BeginDrawing();

				Raylib.ClearBackground(Color.Black);
				Raylib.DrawRectangleLines(
				Constants.LineOffset,
				Constants.LineOffset,
				Raylib.GetScreenWidth() - Constants.LineOffset * 2,
				Raylib.GetScreenHeight() - Constants.LineOffset * 2,
				Color.RayWhite);
				_currentView.Draw();

				Raylib.EndDrawing();
			}

			Button.UnloadClickSound();
			Raylib.CloseAudioDevice();
			Raylib.CloseWindow();
		}

		public void SwitchView(IObject newView)
		{
			_currentView = newView;
		}
	}
}
