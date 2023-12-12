using UnityEngine;

public class Turret : Targetable, IStoresCapable
{
	[Header("General:")]
	[Tooltip("Faction that the ship belongs to. Used in targeting for IFF.")]
	public Faction faction = Faction.White;

	[Tooltip("Name of the turret type as displayed on the targeting boxes and UI.")]
	public string turretTypeName = string.Empty;

	[Tooltip("Name of the individual ship as displayed on the targeting boxes and UI.")]
	public string turretName = string.Empty;

	[Tooltip("Whether or not the ship can be targeted at all.")]
	public bool targetable = true;

	[Header("Rotation properties:")]
	public float turnRate = 30f;

	public float maxElevation = 60f;

	public float maxDepression = 5f;

	[Space]
	public bool limitedTraverse;

	public float maxRightTraverse = 60f;

	public float maxLeftTraverse = 60f;

	[Space]
	public bool runRotationsInFixed;

	public ITargetable target;

	public Vector3 lookAtPoint;

	private Transform turretBase;

	private Transform turretBarrels;

	private TurretRotations turretControl;

	private TurretAutomatedTargeting autoTargeting;

	private Gun gun;

	private Launcher launcher;

	private bool valid;

	public Gun Gun
	{
		get
		{
			return gun;
		}
	}

	public Launcher Launcher
	{
		get
		{
			return launcher;
		}
	}

	public Vector3 BarrelForward
	{
		get
		{
			if (turretBarrels != null)
			{
				return turretBarrels.forward;
			}
			return base.transform.forward;
		}
	}

	public override Vector3 Velocity
	{
		get
		{
			return Vector3.zero;
		}
	}

	public override Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public override Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	public override bool Active
	{
		get
		{
			return targetable;
		}
		set
		{
			targetable = value;
		}
	}

	public override string Name
	{
		get
		{
			return turretName;
		}
		set
		{
		}
	}

	public override string TypeName
	{
		get
		{
			return turretTypeName;
		}
		set
		{
		}
	}

	public override Faction Faction
	{
		get
		{
			return faction;
		}
	}

	public void Awake()
	{
		gun = GetComponentInChildren<Gun>();
		launcher = GetComponentInChildren<Launcher>();
		autoTargeting = GetComponentInChildren<TurretAutomatedTargeting>();
		turretBase = base.transform.Find("Base");
		if ((bool)turretBase)
		{
			turretBarrels = turretBase.Find("Barrels");
		}
		if (turretBase == null)
		{
			Debug.LogWarning(base.name + ": Turret has no base. Must have transform named \"Base\" as a child of turret GameObject. Turret invalid.");
		}
		if (turretBarrels == null)
		{
			Debug.LogWarning(base.name + ": Turret has no barrels. Must have transform named \"Barrels\" as a child of Base transform. Turret invalid.");
		}
		valid = turretBarrels != null && turretBase != null;
		if (valid)
		{
			turretControl = new TurretRotations(base.transform, turretBase, turretBarrels, turnRate, maxLeftTraverse, maxRightTraverse, limitedTraverse, maxElevation, maxDepression);
		}
	}

	public void Update()
	{
		bool key = Input.GetKey(KeyCode.F);
		if (gun != null && key)
		{
			gun.Fire();
		}
		if (autoTargeting != null)
		{
			target = autoTargeting.Target;
		}
		if (target != null)
		{
			if (gun != null && turretBarrels != null)
			{
				lookAtPoint = WeaponHelpers.CalculateLeadForGuns(gun.muzzleVelocity, target.Position, target.Velocity, turretBarrels.position, Vector3.zero);
			}
			else
			{
				lookAtPoint = target.Position;
			}
		}
		if (!runRotationsInFixed)
		{
			RotateTurretToPoint(lookAtPoint);
		}
	}

	public void FixedUpdate()
	{
		if (runRotationsInFixed)
		{
			RotateTurretToPoint(lookAtPoint);
		}
	}

	private void RotateTurretToPoint(Vector3 lookAt)
	{
		if (lookAt != Vector3.zero)
		{
			if (turretBase != null)
			{
				turretControl.RotateturretBaseToTarget(lookAt);
			}
			if (turretBarrels != null)
			{
				turretControl.RotateturretBarrelsToTarget(lookAt);
			}
		}
	}
}
