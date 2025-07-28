using Raylib_cs;

namespace ParkLite.UI.Interfaces
{
	public interface IObject
	{
		Rectangle Bounds { get; }

		void Update();
		void Draw();
	}
}