using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))]
public class TurretAutomatedTargeting : MonoBehaviour 
{
	[Tooltip("Max range at which the turret will automatically acquire a target.")]
	public float targetingRange = 1000f;

	[Tooltip("Time between searches for new targets.")]
	public float updateDelay = 5f;

	[Tooltip("Variable additional time added (between zero and the value provided) on top of the update delay.")]
	public float updateRateVariation = 5f;

	[Tooltip("Time between target acquired and firing.")]
	public float targetLockTime = 2f;

	[Tooltip("Max angle allowed between target and turret weapons to fire.")]
	public float offAngleAllowance = 5f;

	[Tooltip("Time between firing off successive munitions for a launcher.")]
	public float fireDelay = 6f;

	public string targetName;

	private Turret turret;

	private Launcher launcher;

	private Gun gun;

	private float fireCountdown;

	private float lockCountdown;

	private bool locked;

	private ITargetable target;

	private IEnumerator UpdateTargetSelectionCoroutine;

	public ITargetable Target
	{
		get
		{
			return target;
		}
	}

	public bool Locked
	{
		get
		{
			return locked;
		}
	}

	public Vector3 TargetPos
	{
		get
		{
			if (target != null)
			{
				return target.Position;
			}
			return base.transform.forward * 100f;
		}
	}

	public void Awake()
	{
		turret = GetComponent<Turret>();
	}

	public void OnEnable()
	{
		UpdateTargetSelectionCoroutine = UpdateTargetSelection();
		StartCoroutine(UpdateTargetSelectionCoroutine);
	}

	public void OnDisable()
	{
		StopCoroutine(UpdateTargetSelectionCoroutine);
	}

	public void Start()
	{
		launcher = turret.Launcher;
		gun = turret.Gun;
		lockCountdown = targetLockTime;
	}

	public void Update()
	{
		fireCountdown -= Time.deltaTime;
		if (target != null)
		{
			if (Vector3.Distance(target.Position, base.transform.position) > targetingRange)
			{
				target = null;
				locked = false;
				return;
			}
			lockCountdown -= Time.deltaTime;
			locked = true;
			if (lockCountdown <= 0f && offAngleAllowance >= Vector3.Angle(turret.BarrelForward, target.Position - base.transform.position))
			{
				if (gun != null)
				{
					gun.Fire();
				}
				if (launcher != null && fireCountdown <= 0f)
				{
					fireCountdown = fireDelay;
					launcher.target = target;
					launcher.LaunchWeapon();
				}
			}
		}
		else
		{
			lockCountdown = targetLockTime;
			locked = false;
		}
	}

	public Vector3 GetTargetPosition()
	{
		if (target != null)
		{
			return target.Position;
		}
		return base.transform.forward * 100f;
	}

	private IEnumerator UpdateTargetSelection()
	{
		List<ITargetable> inRange2 = new List<ITargetable>();
		List<ITargetable> hostiles2 = new List<ITargetable>();
		while (true)
		{
			if (target == null)
			{
				inRange2 = TargetManager.GetAllTargetsInRange(base.transform.position, targetingRange);
				hostiles2 = inRange2.FindAll((ITargetable x) => FactionUtils.TargetIsHostile(x.Faction, turret.Faction));
				if (hostiles2.Count > 0)
				{
					int index = Random.Range(0, hostiles2.Count);
					target = hostiles2[index];
					targetName = target.Name;
				}
			}
			float nextUpdateIn = updateDelay + Random.Range(0f, updateRateVariation);
			yield return new WaitForSeconds(nextUpdateIn);
		}
	}
}
