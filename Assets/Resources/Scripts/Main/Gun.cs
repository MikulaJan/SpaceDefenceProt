using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform[] barrels;

	public Bullet bulletPrefab;

	public float muzzleVelocity;

	public float roundsPerMinute;

	public float dispersion;

	public ParticleSystem muzzleFlash;

	private float fireDelay = 1f;

	private float fireTime = float.MinValue;

	private int barrelNum;

	private int lastBarrelInList;

	private Ship firedFrom;

	public Ship ParentShip
	{
		get
		{
			return firedFrom;
		}
		set
		{
			firedFrom = value;
		}
	}

	public Ship FiredFrom
	{
		get
		{
			return firedFrom;
		}
	}

	public void Awake()
	{
		if (bulletPrefab == null)
		{
			Debug.LogWarning(base.name + ": No bullet prefab on gun.");
		}
		if (barrels == null)
		{
			Debug.LogWarning(base.name + ": No barrels for gun to fire from.");
		}
	}

	public void Start()
	{
		lastBarrelInList = barrels.Length - 1;
		if (roundsPerMinute != 0f)
		{
			fireDelay = roundsPerMinute / 60f;
			fireDelay = 1f / fireDelay;
		}
	}

	public void Fire()
	{
		Vector3 inheritedVelocity = Vector3.zero;
		if (firedFrom != null)
		{
			inheritedVelocity = FiredFrom.Rigidbody.velocity;
		}
		Fire(inheritedVelocity);
	}

	public void Fire(Vector3 inheritedVelocity)
	{
		if (!(Time.time - fireTime >= fireDelay))
		{
			return;
		}
		fireTime = Time.time;
		if (bulletPrefab != null && barrels != null)
		{
			Vector3 forwardWithDeviationAt100m = WeaponHelpers.GetForwardWithDeviationAt100m(barrels[barrelNum], dispersion);
			Quaternion rotation = Quaternion.LookRotation(forwardWithDeviationAt100m, barrels[barrelNum].up);
			Bullet bullet = Object.Instantiate(bulletPrefab, barrels[barrelNum].position, rotation);
			Vector3 vector = forwardWithDeviationAt100m.normalized * muzzleVelocity;
			bullet.Fire(vector + inheritedVelocity, firedFrom);
			if ((bool)muzzleFlash)
			{
				Object.Instantiate(muzzleFlash, barrels[barrelNum], false);
			}
			barrelNum++;
			if (barrelNum > lastBarrelInList)
			{
				barrelNum = 0;
			}
		}
	}
}
