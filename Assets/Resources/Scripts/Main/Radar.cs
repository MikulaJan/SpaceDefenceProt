using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
	public float lockTime = 1f;

	public float lockAngle = 15f;

	[Tooltip("Time between radar updates.")]
	public float refreshDelay = 1f;

	[Tooltip("How far the radar can detect neutral/enemy targets.")]
	public float range = 10000f;

	[Tooltip("How far the radar can detect friendly targets.")]
	public float friendlyRange = 10000f;

	private List<RadarTarget> allTargets;

	private List<RadarTarget> visibleTargets;

	private RadarTarget activeTarget;

	private Ship ship;

	private float lockCountdown;

	private bool hasStarted;

	private IEnumerator RunRadarModelToPopulateTargetListCoroutine;

	public ITargetable RadarTarget
	{
		get
		{
			if (activeTarget != null)
			{
				return activeTarget.Target;
			}
			return null;
		}
	}

	public bool Locked
	{
		get
		{
			return lockCountdown <= 0f;
		}
	}

	public void Awake()
	{
		ship = GetComponent<Ship>();
		allTargets = new List<RadarTarget>();
		visibleTargets = new List<RadarTarget>();
		RunRadarModelToPopulateTargetListCoroutine = RunRadarModelToPopulateTargetList();
	}

	public void OnEnable()
	{
		TargetManager.TargetAddedEvent += TargetAddedEventHandler;
		TargetManager.TargetRemovedEvent += TargetRemovedEventHandler;
		StartCoroutine(RunRadarModelToPopulateTargetListCoroutine);
	}

	public void OnDisable()
	{
		TargetManager.TargetAddedEvent -= TargetAddedEventHandler;
		TargetManager.TargetRemovedEvent -= TargetRemovedEventHandler;
		StopCoroutine(RunRadarModelToPopulateTargetListCoroutine);
	}

	public void TargetAddedEventHandler(ITargetable newTarget)
	{
		if (hasStarted && ship != newTarget)
		{
			allTargets.Add(new RadarTarget(newTarget));
		}
	}

	public void TargetRemovedEventHandler(ITargetable removedTarget)
	{
		if (hasStarted)
		{
			allTargets.Remove(allTargets.Find((RadarTarget x) => x == removedTarget));
			if (ship.IsPlayerControlled && removedTarget.AssignedHudTargetBox != null)
			{
				HUDTargetManager.reference.RemoveTarget(removedTarget);
			}
		}
	}

	public void Start()
	{
		hasStarted = true;
		allTargets.Clear();
		List<ITargetable> list = TargetManager.GetAllTargets();
		foreach (ITargetable item in list)
		{
			if (ship != item)
			{
				allTargets.Add(new RadarTarget(item));
			}
		}
		lockCountdown = lockTime;
	}

	public void Update()
	{
		if (visibleTargets.Count <= 0)
		{
			return;
		}
		if (Input.GetButtonDown("Fire4"))
		{
			if (activeTarget != null)
			{
				lockCountdown = lockTime;
				activeTarget.Locked = false;
			}
			activeTarget = CycleToNextTarget();
			if (activeTarget != null)
			{
				activeTarget.SetTargeted(true);
			}
			UpdateTargets();
		}
		AttemptToLockTarget();
	}

	private void UpdateTargets()
	{
		if (ship.IsPlayerControlled)
		{
			HUDTargetManager.reference.target = activeTarget.Target;
		}
		foreach (RadarTarget allTarget in allTargets)
		{
			if (activeTarget != null)
			{
				allTarget.IsTargeted = allTarget == activeTarget;
			}
			else
			{
				allTarget.IsTargeted = false;
			}
		}
	}

	private RadarTarget CycleToNextTarget()
	{
		RadarTarget result = null;
		float num = 0f;
		float num2 = 0f;
		foreach (RadarTarget visibleTarget in visibleTargets)
		{
			num = visibleTarget.TimeSinceTargeted();
			if (num > num2)
			{
				num2 = num;
				result = visibleTarget;
			}
		}
		return result;
	}

	private IEnumerator RunRadarModelToPopulateTargetList()
	{
		float distanceToTarget2 = float.MaxValue;
		while (true)
		{
			visibleTargets.Clear();
			foreach (RadarTarget allTarget in allTargets)
			{
				if (allTarget.Active)
				{
					distanceToTarget2 = Vector3.Distance(allTarget.Position, base.transform.position);
					if (allTarget.Faction == ship.Faction && distanceToTarget2 < friendlyRange)
					{
						allTarget.Friendly = FactionUtils.TargetIsFriendly(allTarget.Faction, ship.Faction);
						AddVisibleTarget(allTarget);
					}
					else if (distanceToTarget2 < range)
					{
						AddVisibleTarget(allTarget);
					}
				}
				else
				{
					RemoveVisibleTarget(allTarget);
				}
			}
			if (!visibleTargets.Contains(activeTarget))
			{
				activeTarget = null;
			}
			yield return new WaitForSeconds(refreshDelay);
		}
	}

	private void AddVisibleTarget(RadarTarget target)
	{
		visibleTargets.Add(target);
		if (ship.IsPlayerControlled && HUDTargetManager.reference != null)
		{
			HUDTargetManager.reference.AddNewTarget(target.Target);
		}
	}

	private void RemoveVisibleTarget(RadarTarget target)
	{
		visibleTargets.Remove(target);
		target.IsTargeted = false;
		target.Locked = false;
		if (ship.IsPlayerControlled)
		{
			HUDTargetManager.reference.RemoveTarget(target.Target);
		}
	}

	private void AttemptToLockTarget()
	{
		if (activeTarget != null && !FactionUtils.TargetIsFriendlyOrNeutral(activeTarget.Faction, ship.Faction))
		{
			if (Vector3.Angle(ship.transform.forward, activeTarget.Position - ship.transform.position) < lockAngle)
			{
				lockCountdown -= Time.deltaTime;
				if (lockCountdown <= 0f)
				{
					lockCountdown = 0f;
				}
				activeTarget.Locked = lockCountdown <= 0f;
			}
			else
			{
				activeTarget.Locked = false;
				lockCountdown = lockTime;
			}
			if (ship.IsPlayerControlled && HUDTargetManager.reference != null)
			{
				HUDTargetManager.reference.lockProgress = 1f - lockCountdown / lockTime;
			}
		}
		else if (ship.IsPlayerControlled && HUDTargetManager.reference != null)
		{
			HUDTargetManager.reference.lockProgress = 0f;
		}
	}
}
