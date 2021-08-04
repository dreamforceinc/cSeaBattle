// cSeaBattle.cs
//
namespace cSeaBattle
{
	public enum Cell : int
	{
		EMPTY = 0,
		MISS = 5,
		HIT = 6,
		SUNK = 7
	}

	class cSeaBattle
	{
		public static void Main()
		{
			Battlefield seaBattle = new Battlefield();
			seaBattle.PlayTheGame();
		}
	}
}
