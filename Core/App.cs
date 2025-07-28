using Raylib_cs;
using ParkLite.Interfaces;
using ParkLite.Core.Views;

namespace ParkLite.Core
{
	public class App
	{
		private IObject _currentView;

		public App()
		{
			Raylib.InitWindow(Constants.ScreenWidth, Constants.ScreenHeight, Constants.ScreenTitle);
			Raylib.SetTargetFPS(Constants.TargetFPS);

			_currentView = new AccountListView(this);
		}

		public void Run()
		{
			while (!Raylib.WindowShouldClose())
			{
				_currentView.Update();

				Raylib.BeginDrawing();
				Raylib.ClearBackground(Color.Black);
				_currentView.Draw();
				Raylib.EndDrawing();
			}

			Raylib.CloseWindow();
		}

		public void SwitchView(IObject newView)
		{
			_currentView = newView;
		}
	}
}
