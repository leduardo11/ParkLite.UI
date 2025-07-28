using System.Numerics;
using Raylib_cs;
using ParkLite.UI.Interfaces;
using ParkLite.UI.Widgets;

namespace ParkLite.UI.Core.Views
{
	public class AccountListView : IObject
	{
		private readonly App _app;
		private readonly Table _accountTable;
		private readonly Button _addAccountBtn;

		private record Account(int Id, string Name, int ContactCount, int VehicleCount);
		private readonly List<Account> _accounts =
			[
				new(1, "Alice Family", 2, 1),
				new(2, "Bob Family", 3, 2),
				new(3, "Charlie Clan", 1, 3),
			];

		public Rectangle Bounds => new(0, 0, Constants.ScreenWidth, Constants.ScreenHeight);

		public AccountListView(App app)
		{
			_app = app;

			_addAccountBtn = Button.CreateDefaultBtn(
				new Vector2(100, 30),
				new Vector2(120, 50),
				"Add Account",
				btn => Console.WriteLine("Add Account Clicked") // TODO: Switch to AccountDetailView with new account
			);

			_accountTable = BuildAccountTable();
		}

		private Table BuildAccountTable()
		{
			var columns = new[] { "ID", "Name", "Contacts", "Vehicles", "Actions" };

			Func<Account, object>[] selectors =
			[
				a => a.Id,
				a => a.Name,
				a => a.ContactCount,
				a => a.VehicleCount,
				a => "[View]"
			];

			var table = Table.CreateDefaultTable(new Vector2(100, 90), new Vector2(600, 400), 1, columns.Length);
			table.SetData(_accounts, columns, selectors);

			for (int row = 1; row < table.Rows; row++)
				table.SetCellTextColor(row, 4, Color.Blue);

			// TODO: Add click detection on "Actions" column for each row to open detail view

			return table;
		}

		public void Update()
		{
			_addAccountBtn.Update();
			_accountTable.Update();

			// Detect clicks on [View] action cells (column 4)
			var mousePos = Raylib.GetMousePosition();
			if (Raylib.IsMouseButtonPressed(MouseButton.Left))
			{
				for (int row = 1; row < _accountTable.Rows; row++)
				{
					if (_accountTable.IsCellClicked(row, 4, mousePos))
					{
						var selectedAccount = _accounts[row - 1];
						// Switch to detail view for selectedAccount
						Console.WriteLine($"View clicked for account {selectedAccount.Name}");
						// _app.SwitchView(new AccountDetailView(_app, selectedAccount));
					}
				}
			}
		}

		public void Draw()
		{
			Raylib.DrawText("Parking Accounts", 250, 10, 20, Color.White);
			_addAccountBtn.Draw();
			_accountTable.Draw();
		}
	}
}
