using Raylib_cs;

namespace ParkLite.Interfaces
{
	public interface IObject
	{
		Rectangle Bounds { get; }

		void Update();
		void Draw();
	}
}