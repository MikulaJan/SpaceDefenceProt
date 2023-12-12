using SpaceDenfece;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(ShipAI))]
[RequireComponent(typeof(ThrusterPhysics))]
[RequireComponent(typeof(ThrusterBanks))]
[RequireComponent(typeof(Rigidbody))]
public class Ship : Targetable, IStoresCapable
{
	[Tooltip("Faction that the ship belongs to. Used in targeting for IFF.")]
	public Faction faction = Faction.White;

	[Tooltip("Name of the ship type as displayed on the targeting boxes and UI.")]
	public string shipTypeName;

	[Tooltip("Name of the individual ship as displayed on the targeting boxes and UI.")]
	public string shipName;

	[Tooltip("Whether or not the ship can be targeted at all.")]
	public bool targetable = true;

	public ITargetable target;

	public Animator animator;

	public float emptyMass = 10000f;

	private ShipAI _shipAI;

	private FlightComputer _flightComputer;

	private ThrusterPhysics _thrusterPhysics;

	private ThrusterBanks _thrusterBanks;

	private LandingGear _landingGear;

	private StoresManagement _stores;

	private FireControl _fireControl;

	private Radar _radar;

	private Gun _gun;

	private Rigidbody _rigidbody;

	private Collider _collider;

	private const float TIME_TO_RELOAD = 5f;

	private float timeSinceLanding;

	private bool takenOffSinceLastReload = true;

	private float initialPidZ;

	private float initialThrustDelay;

	private bool radarLock;

	[SerializeField]
    public ShipInput _input { get; private set; }



    public ShipAI Controls
	{
		get
		{
			return _shipAI;
		}
	}

	public FlightComputer FlightComputer
	{
		get
		{
			return _flightComputer;
		}
	}

	public ThrusterPhysics ThrusterPhysics
	{
		get
		{
			return _thrusterPhysics;
		}
	}

	public ThrusterBanks ThrusterBanks
	{
		get
		{
			return _thrusterBanks;
		}
	}

	public Rigidbody Rigidbody
	{
		get
		{
			return _rigidbody;
		}
	}

	public Collider Collider
	{
		get
		{
			return _collider;
		}
	}

	public Gun Gun
	{
		get
		{
			return _gun;
		}
	}

	public FireControl FireControl
	{
		get
		{
			return _fireControl;
		}
	}

	public LandingGear landingGear
	{
		get
		{
			return _landingGear;
		}
	}

	public StoresManagement stores
	{
		get
		{
			return _stores;
		}
	}

	public bool IsPlayerControlled
	{
		get
		{
			return _shipAI.usePlayerInput;
		}
	}

	public bool ParkingBrake
	{
		get
		{
			return _shipAI.parkingBrake;
		}
	}

	public override Vector3 Velocity
	{
		get
		{
			return _rigidbody.velocity;
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

	public override string Name
	{
		get
		{
			return shipName;
		}
		set
		{
			shipName = value;
		}
	}

	public override string TypeName
	{
		get
		{
			return shipTypeName;
		}
		set
		{
			shipTypeName = value;
		}
	}

	public override Faction Faction
	{
		get
		{
			return faction;
		}
	}

    public override bool Active { get => targetable; set => targetable = value; }

    public void Awake()
	{
        _input = GetComponent<ShipInput>();
        _shipAI = GetComponent<ShipAI>();
		_flightComputer = GetComponent<FlightComputer>();
		_thrusterPhysics = GetComponent<ThrusterPhysics>();
		_thrusterBanks = GetComponent<ThrusterBanks>();
		_landingGear = GetComponent<LandingGear>();
		_stores = GetComponent<StoresManagement>();
		_radar = GetComponent<Radar>();
		_gun = GetComponent<Gun>();
		_fireControl = GetComponent<FireControl>();
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.mass = emptyMass;
		_rigidbody.angularDrag = 0f;
		_rigidbody.drag = 0f;
		_rigidbody.useGravity = false;
		Transform transform = base.transform.Find("Model");
		if ((bool)transform)
		{
			_collider = transform.GetComponentInChildren<Collider>();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		ExplosionManager.OnExplosion += OnExplosion;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		ExplosionManager.OnExplosion -= OnExplosion;
	}

	public void Start()
	{
		if (_gun != null)
		{
			_gun.ParentShip = this;
		}
		ShipManager.AddShip(this);
		initialPidZ = _flightComputer.PIDZ.z;
		initialThrustDelay = _thrusterPhysics.thrustDelay;
	}

	public void Update()
	{
		if (IsPlayerControlled && HUDValues.reference != null)
		{
			HUDValues.reference.UpdateFlightParams(Mathf.Clamp(_shipAI.throttle * 0.5f, -1f, 1f), _rigidbody.velocity.magnitude * 1.94384f, 0f, Vector3.Angle(base.transform.forward, _rigidbody.velocity), base.transform.forward, base.transform.rotation.eulerAngles, base.transform.position, _rigidbody.velocity * 1000f);
			HUDValues.reference.ShowDisconnect(!_shipAI.fcsConnected);
			if ((bool)_radar)
			{
				target = _radar.RadarTarget;
				radarLock = _radar.Locked;
			}
			if ((bool)_landingGear)
			{
				HUDValues.reference.GearDownPct(_landingGear.gearDownPercent);
				HUDValues.reference.UpdateBrakes(_shipAI.brakes);
			}
			if ((bool)_fireControl)
			{
				_fireControl.target = target;
				_fireControl.radarLock = radarLock;
				HUDValues.reference.UpdateSelectedWeapon(_fireControl.GetNameOfSelectedWeapon(), _fireControl.GetTotalAmmoOfSelectedWeapon());
			}
		}
		if ((bool)_landingGear)
		{
			_landingGear.SetBrakePct(_shipAI.brakes);
		}
		if (IsPlayerControlled) 
		{
            UpdateShipMass();
            HandleReloadWhileLanded();
            CoordinateControls();
            RemoveThrustDelayWhenWeightOnWheels();
        }

	}

	public void FixedUpdate()
	{
		ThrusterBanks.SetTranslationRotation(ThrusterPhysics.AppliedTranslation, ThrusterPhysics.AppliedRotation);
	}

	private void UpdateShipMass()
	{
		if (_rigidbody != null && _stores != null)
		{
			_rigidbody.mass = emptyMass + _stores.GetTotalMassOfStores();
		}
	}

	private void RemoveThrustDelayWhenWeightOnWheels()
	{
		if (_landingGear != null)
		{
			if (_landingGear.IsWeightOnWheels())
			{
				_thrusterPhysics.thrustDelay = 0f;
				_flightComputer.PIDZ.z = 0f;
			}
			else
			{
				_thrusterPhysics.thrustDelay = initialThrustDelay;
				_flightComputer.PIDZ.z = initialPidZ;
			}
		}
	}

	private void CoordinateControls()
	{
		if (_flightComputer != null)
		{
			_flightComputer.SetInput(_input.Pitch, _input.Yaw, _input.Roll, _shipAI.throttle, _shipAI.fcsConnected);
			_thrusterPhysics.SetAppliedThrust(_flightComputer.CommandedThrust, _flightComputer.CommandedRotation);
		}
		else
		{
			_thrusterPhysics.SetAppliedThrust(new Vector3(0f, 0f, _shipAI.throttle), new Vector3(_input.Pitch, _input.Yaw, _input.Roll));
		}
	}

	private void HandleReloadWhileLanded()
	{
		if (_landingGear != null && _landingGear.IsWeightOnWheels())
		{
			timeSinceLanding += Time.deltaTime;
			if (timeSinceLanding > 5f && takenOffSinceLastReload)
			{
				_fireControl.ReloadAllWeapons();
				takenOffSinceLastReload = false;
				MonoBehaviour.print("RELOAD");
			}
		}
		else
		{
			takenOffSinceLastReload = true;
			timeSinceLanding = 0f;
		}
	}

	private void OnExplosion(Vector3 position, float damage, float radius, float impulse)
	{
		_rigidbody.AddExplosionForce(impulse, position, radius);
	}
}
