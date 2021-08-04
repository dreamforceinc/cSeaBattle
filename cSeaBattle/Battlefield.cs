// Battlefield.cs
//
using System;

namespace cSeaBattle
{
	class Battlefield
	{
		private static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		private const int max_ships = 10;
		private Ship[] ships = new Ship[max_ships];
		private bool gameOver = false;
		private int countSunkenShips = 0, step = 0;
		private static (int dx, int dy)[] deltas = new (int, int)[] { (0, 0), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1), (-1, 1), (-1, 0), (-1, -1) };

		public static int[,] bf = new int[10, 10];

		public Battlefield()
		{
			for (int i = 0; i < ships.Length; i++)
			{
				ships[i] = new Ship();
				PlaceShip(ships[i]);
			}
		}

		private void PrintTitle()
		{
			Console.Clear();
#if DEBUG
			Console.WriteLine($" Морской бой. Версия {version}");
#else
			Console.WriteLine($" Морской бой. Версия {version.Major}.{version.Minor}");
#endif
			Console.WriteLine(" Copyright (c) 2021 by W0LF aka 'dreamforce'");
			Console.WriteLine();
		}

		private void PlaceShip(Ship ship)
		{
			bool isShipPlaced = false;

			while (!isShipPlaced)
			{
				ship.MakeRandomCoords(out int x, out int y);

				if (IsCellsEmpty(ship, x, y))
				{
					for (int deck = 0; deck < ship.Decks; deck++)
					{
						bf[x, y] = ship.Decks;
						ship.Coords[deck].X = x;
						ship.Coords[deck].Y = y;
						if (ship.IsVertical) y++; else x++;
					}
					isShipPlaced = true;
				}
			}
		}

		private bool IsCellsEmpty(Ship ship, int x, int y)
		{
			for (int deck = 0; deck < ship.Decks; deck++)
			{
				foreach (var d in deltas)
				{
					if (((x + d.dx >= 0 && x + d.dx <= 9) && (y + d.dy >= 0 && y + d.dy <= 9)) && (bf[x + d.dx, y + d.dy] != 0)) return false;
				}
				if (ship.IsVertical) y++; else x++;
			}
			return true;
		}

		private Ship GetShip(int x, int y)
		{
			foreach (var ship in ships)
			{
				for (int i = 0; i < ship.Decks; i++)
				{
					if (x == ship.Coords[i].X && y == ship.Coords[i].Y) return ship;
				}
			}
			return null;
		}

		private void CheckShip(Ship ship)
		{
			foreach (var coord in ship.Coords)
			{
				if (bf[coord.X, coord.Y] != (int)Cell.HIT)
				{
					ship.IsSunk = false;
					return;
				}
			}
			ship.IsSunk = true;
			foreach (var coord in ship.Coords) bf[coord.X, coord.Y] = (int)Cell.SUNK;
		}

		public void PlayTheGame()
		{
			string input = "";
			char horz;
			int x, y;
			Ship ship = null;

			while (!gameOver)
			{
				PrintTitle();
#if DEBUG
				ShowBattlefield(true);
#else
				ShowBattlefield(false);
#endif
				Console.Write("Введите координату выстрела - сперва буква (A - J), потом цифра (0 - 9). Например - 'D5'\n> ");
				input = Console.ReadLine().ToUpper();
				if (input == "Q") break;

				if (input.Length != 2) continue;
				if ("ABCDEFGHIJ".Contains(input[0].ToString())) horz = input[0]; else continue;
				if (int.TryParse(input[1].ToString(), out int res) && (res >= 0 && res <= 9)) y = res; else continue;
				x = horz - 'A';

				step++;
				ship = GetShip(x, y);
				if (null == ship)
				{
					bf[x, y] = (int)Cell.MISS;
					continue;
				}
				if (ship.IsSunk) continue;
				else
				{
					bf[x, y] = (int)Cell.HIT;
					CheckShip(ship);
					if (ship.IsSunk) countSunkenShips++;
				}

				if (countSunkenShips >= max_ships) gameOver = true;
			}
			PrintTitle();
			ShowBattlefield(true);
		}


		public void ShowBattlefield(bool showShips)
		{
			char symbol;
			Console.WriteLine("    A B C D E F G H I J");
			Console.WriteLine("   +-------------------+");
			for (int v = 0; v < 10; v++)
			{
				Console.Write(' ');
				Console.Write(v.ToString() + " |");
				for (int h = 0; h < 10; h++)
				{
					if (showShips) symbol = (char)(bf[h, v] + 48); else symbol = ' ';
					switch (bf[h, v])
					{
						case (int)Cell.EMPTY:
							symbol = ' ';
							break;
						case (int)Cell.MISS:
							{
								symbol = '*';
								if (gameOver && showShips) symbol = ' ';
							}
							break;
						case (int)Cell.HIT:
							symbol = '@';
							break;
						case (int)Cell.SUNK:
							symbol = 'X';
							break;
					}
					Console.Write(symbol); Console.Write('|');
				}
				if (v == 1) Console.Write("\t* - Промах");
				if (v == 2) Console.Write("\t@ - Попадание");
				if (v == 3) Console.Write("\tX - Потоплен");
				if (gameOver && v == 7) Console.Write("\tG A M E   O V E R !");
				Console.WriteLine();
			}
			Console.WriteLine("   +-------------------+");
			Console.WriteLine($"\n   Ход: {step}\n   Осталось кораблей: {max_ships - countSunkenShips}\n");
		}
	}
}
