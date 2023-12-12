using UnityEngine;

[RequireComponent(typeof(StoresCarriage))]
public class Launcher : MonoBehaviour
{
	[Header("General launcher options:")]
	[Tooltip("When true, launcher has infinite reloads.")]
	public bool infiniteAmmo;

	[Tooltip("How many missiles this launcher carries.")]
	public int munitionCount = 1;

	[Tooltip("Time between successive shots.")]
	public float fireDelay = 6f;

	[Tooltip("Target of anything that will be launched from here.")]
	public Rigidbody targetRigidbody;

	public ITargetable target;

	[Tooltip("Starting velocity of any weapon launched from this launcher.")]
	public Vector3 initialVelocity; 

	[Header("Weapon fire delay overrides:")]
	[Tooltip("Override the fire delay of a weapon upon launch. Used to force the delayed firing of weapons after it leaves the launcher.")]
	public bool overrideFireDelay;

	public float newFireDelay;

	protected float _magazineReloadCooldown;

	protected int _missileCount;

	protected Munition _missilePrefab;

	protected StoresCarriage _storesCarriage;

	protected LauncherMountCooldown[] _mountCooldowns;

	private int totalMounts;

	private int mountNum;

	public int AmmoCount
	{
		get
		{
			return _missileCount;
		}
	}

	protected void Awake()
	{
		_storesCarriage = GetComponent<StoresCarriage>();
	}

	protected void Start()
	{
		totalMounts = _storesCarriage.NumOfMountPoints;
		_missileCount = munitionCount * Difficulty.weaponMultiplier;
		_mountCooldowns = new LauncherMountCooldown[totalMounts];
		for (int i = 0; i < totalMounts; i++)
		{
			_mountCooldowns[i] = new LauncherMountCooldown(fireDelay);
		}
	}

	public void OnEnable()
	{
		_storesCarriage.NewStorePrefabSetEvent += HandleNewStorePrefabSet;
	}

	public void OnDisable()
	{
		_storesCarriage.NewStorePrefabSetEvent -= HandleNewStorePrefabSet;
	}

	protected void Update()
	{
		if ((bool)targetRigidbody)
		{
			target = targetRigidbody.GetComponent<ITargetable>();
		}
		if (infiniteAmmo || (_missileCount >= totalMounts && _missilePrefab != null))
		{
			for (int i = 0; i < _mountCooldowns.Length; i++)
			{
				if (_mountCooldowns[i].UpdateCooldownAndCheckIfFinished())
				{
					Store component = _missilePrefab.GetComponent<Store>();
					if (component != null)
					{
						_storesCarriage.HangStore(component, i);
					}
				}
			}
		}
		if (_magazineReloadCooldown > 0f)
		{
			_magazineReloadCooldown -= Time.deltaTime;
		}
	}

	[ContextMenu("Launch weapon")]
	public void LaunchWeapon()
	{
		if (_storesCarriage.StoresCarrier != null)
		{
			LaunchWeapon(_storesCarriage.StoresCarrier.Velocity);
		}
		else
		{
			LaunchWeapon(Vector3.zero);
		}
	}

	public bool LaunchWeapon(Vector3 parentVelocity)
	{
		bool result = false;
		if (infiniteAmmo || _missileCount > 0)
		{
			Store store = _storesCarriage.mountPoints[mountNum].store;
			if (store != null)
			{
				Munition component = store.GetComponent<Munition>();
				if (component != null)
				{
					component.target = target;
					Vector3 vector = _storesCarriage.mountPoints[mountNum].transform.TransformDirection(initialVelocity);
					if (overrideFireDelay)
					{
						component.fireDelay = newFireDelay;
					}
					component.Launch(parentVelocity + vector);
					_mountCooldowns[mountNum].ResetCooldown();
					mountNum++;
					if (mountNum >= totalMounts)
					{
						mountNum = 0;
					}
					result = true;
					_missileCount--;
				}
				else
				{
					Debug.LogWarning(base.name + ": Failed to launch because store was not a launchable weapon.");
				}
			}
		}
		return result;
	}

	public Munition GetMunitionFromStoresCarriage()
	{
		Munition result = null;
		Store storePrefab = _storesCarriage.storePrefab;
		if (storePrefab != null)
		{
			result = storePrefab.GetComponent<Munition>();
		}
		return result;
	}

	private void HandleNewStorePrefabSet(object sender, NewStorePrefabSetEventArgs e)
	{
		_missilePrefab = e.storePrefab.GetComponent<Munition>();
		if (_missilePrefab == null)
		{
			MonoBehaviour.print(base.name + ": New store created on launcher that is not a launchable weapon.");
		}
	}

	public void Reload()
	{
		_missileCount = munitionCount * Difficulty.weaponMultiplier;
		LauncherMountCooldown[] mountCooldowns = _mountCooldowns;
		foreach (LauncherMountCooldown launcherMountCooldown in mountCooldowns)
		{
			launcherMountCooldown.SetFinished();
		}
	}

	public string GetNameOfMunition()
	{
		if (_missilePrefab != null)
		{
			return _missilePrefab.hudDisplayName;
		}
		return string.Empty;
	}
}
