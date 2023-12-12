public static class FactionUtils
{
	public static bool TargetIsFriendly(Faction target, Faction ownFaction)
	{
		return target == ownFaction;
	}

	public static bool TargetIsFriendlyOrNeutral(Faction target, Faction ownFaction)
	{
		return target == ownFaction || target == ownFaction;
	}

	public static bool TargetIsHostile(Faction target, Faction ownFaction)
	{
		return !TargetIsFriendlyOrNeutral(target, ownFaction);
	}
}
