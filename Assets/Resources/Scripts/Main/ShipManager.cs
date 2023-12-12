using System.Collections.Generic;

public static class ShipManager
{
	private static List<Ship> allShips;

	public static void AddShip(Ship ship)
	{
		CheckAllShipsInitialized();
		allShips.Add(ship);
	}

	public static void RemoveShip(Ship ship)
	{
		CheckAllShipsInitialized();
		allShips.Remove(ship);
	}

	private static void CheckAllShipsInitialized()
	{
		if (allShips == null)
		{
			allShips = new List<Ship>();
		}
	}

	private static Ship[] GetAllShips()
	{
		return allShips.ToArray();
	}
}
