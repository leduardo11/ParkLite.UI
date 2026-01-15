using System.Numerics;
using Raylib_cs;
using ParkLite.UI.Interfaces;

namespace ParkLite.UI.Interfaces
{
	public interface IWidget : IObject
	{
		bool Enabled { get; set; }
		bool Visible { get; set; }
		object? Tag { get; }
	}
}
