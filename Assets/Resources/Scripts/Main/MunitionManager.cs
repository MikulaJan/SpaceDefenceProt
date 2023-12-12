using System.Collections.Generic;

public static class MunitionManager
{
	private static List<Munition> allMunitions;

	public static void AddMunition(Munition munition)
	{
		CheckAllMunitionsInitialized();
		allMunitions.Add(munition);
	}

	public static void RemoveShip(Munition munition)
	{
		CheckAllMunitionsInitialized();
		allMunitions.Remove(munition);
	}

	private static void CheckAllMunitionsInitialized()
	{
		if (allMunitions == null)
		{
			allMunitions = new List<Munition>();
		}
	}

	public static Munition[] GetAllMunitions()
	{
		return allMunitions.ToArray();
	}
}
