using System.Numerics;
using ParkLite.UI;
using Raylib_cs;

namespace ParkLite.Core;

public class App
{
	private enum ViewState { AccountList }
	private readonly ViewState _view = ViewState.AccountList;
	private readonly Table _accountTable;
	private readonly Button _addAccountBtn;

	private record Account(int Id, string Name, int ContactCount, int VehicleCount);
	private readonly List<Account> _accounts =
	[
		new(1, "Alice Family", 2, 1),
		new(2, "Bob Family", 3, 2),
		new(3, "Charlie Clan", 1, 3),
	];

	public App()
	{
		Raylib.InitWindow(Constants.ScreenWidth, Constants.ScreenHeight, Constants.ScreenTitle);
		Raylib.SetTargetFPS(60);

		_addAccountBtn = Button.CreateDefaultBtn(
			new Vector2(100, 30),
			new Vector2(120, 50),
			"Add Account",
			btn => Console.WriteLine("Add Account Clicked")
		);

		_accountTable = BuildAccountTable();
	}

	private Table BuildAccountTable()
	{
		var table = Table.CreateDefaultTable(new Vector2(100, 90), new Vector2(600, 400), 10, 5);

		table.SetCell(0, 0, "ID");
		table.SetCell(0, 1, "Name");
		table.SetCell(0, 2, "Contacts");
		table.SetCell(0, 3, "Vehicles");
		table.SetCell(0, 4, "Actions");

		for (int i = 0; i < _accounts.Count; i++)
		{
			var acc = _accounts[i];
			int row = i + 1;

			table.SetCell(row, 0, acc.Id.ToString());
			table.SetCell(row, 1, acc.Name);
			table.SetCell(row, 2, acc.ContactCount.ToString());
			table.SetCell(row, 3, acc.VehicleCount.ToString());
			table.SetCell(row, 4, "[View]");
			table.SetCellTextColor(row, 4, Color.Blue);
		}

		table.CalculateColumnWidths();
		return table;
	}

	public void Run()
	{
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Draw();
		}
		Raylib.CloseWindow();
	}

	private void Update()
	{
		if (_view == ViewState.AccountList)
		{
			_addAccountBtn.Update();
			_accountTable.Update();
		}
	}

	private void Draw()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Black);

		if (_view == ViewState.AccountList)
		{
			Raylib.DrawText("Parking Accounts", 250, 10, 20, Color.White);
			_addAccountBtn.Draw();
			_accountTable.Draw();
		}

		Raylib.EndDrawing();
	}
}
