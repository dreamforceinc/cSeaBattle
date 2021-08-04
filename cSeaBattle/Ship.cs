// Ship.cs
//
using System;
using System.Collections.Generic;

namespace cSeaBattle
{
	class Ship
	{
		private static readonly Random rnd = new Random();
		private static readonly int[] SHIP_DATA = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
		private static int counter = 0;

		public int Id { get; }
		public int Decks { get; set; }
		public bool IsVertical { get; set; }
		public bool IsSunk { get; set; }
		public class Coordinates
		{
			public int X { get; set; }
			public int Y { get; set; }
		}
		public List<Coordinates> Coords { get; set; }

		public Ship()
		{
			Id = counter++;
			Decks = SHIP_DATA[Id];
			if (rnd.Next(2) == 1) IsVertical = true; else IsVertical = false;
			IsSunk = false;
			Coords = new List<Coordinates>();
			for (int k = 0; k < Decks; k++) Coords.Add(new Coordinates());
		}

		public void MakeRandomCoords(out int h, out int v)
		{
			if (IsVertical)
			{
				h = rnd.Next(0, 10);
				v = rnd.Next(0, 10 - Decks - 1);
			}
			else
			{
				h = rnd.Next(0, 10 - Decks - 1);
				v = rnd.Next(0, 10);
			}
		}
	}
}
