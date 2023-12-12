using System.Collections.Generic;
using UnityEngine;

public static class TargetManager
{
	public delegate void TargetAdded(ITargetable newTarget);

	public delegate void TargetRemoved(ITargetable removedTarget);

	private static List<ITargetable> allTargets;

	public static event TargetAdded TargetAddedEvent;

	public static event TargetRemoved TargetRemovedEvent;

	public static void AddTarget(ITargetable target)
	{
		CheckAllTargetsInitialized();
		allTargets.Add(target);
		if (TargetManager.TargetAddedEvent != null)
		{
			TargetManager.TargetAddedEvent(target);
		}
	}

	public static void RemoveTarget(ITargetable target)
	{
		CheckAllTargetsInitialized();
		if (TargetManager.TargetRemovedEvent != null)
		{
			TargetManager.TargetRemovedEvent(target);
		}
		allTargets.Remove(target);
	}

	private static void CheckAllTargetsInitialized()
	{
		if (allTargets == null)
		{
			allTargets = new List<ITargetable>();
		}
	}

	public static List<ITargetable> GetAllTargets()
	{
		return new List<ITargetable>(allTargets);
	}

	public static List<ITargetable> GetAllTargetsInRange(Vector3 fromPosition, float range, ITargetable self = null)
	{
		List<ITargetable> list = new List<ITargetable>();
		if (allTargets != null)
		{
			Vector3 zero = Vector3.zero;
			float num = range * range;
			float num2 = 0f;
			foreach (ITargetable allTarget in allTargets)
			{
				num2 = (allTarget.Position - fromPosition).sqrMagnitude;
				if (num2 > 0.1f && num2 < num && allTarget != self)
				{
					list.Add(allTarget);
				}
			}
		}
		return list;
	}
}
