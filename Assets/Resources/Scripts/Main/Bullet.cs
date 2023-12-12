using UnityEngine;

public class Bullet : MonoBehaviour
{
	public bool moveInFixedUpdate = true;

	public float timeToLive;

	public float damage;

	public ParticleSystem hitFx;

	public bool ExplodeOnTimeout;

	private Vector3 velocity = Vector3.zero;

	private Ship firedFrom;

	private float fireTime;

	private bool fired;

	private bool timedOut;

	private const float VELOCITY_MULT = 1.5f;

	public Ship FiredFrom
	{
		get
		{
			return firedFrom;
		}
	}

	public void Update()
	{
		if (fired)
		{
			if (Time.time - fireTime > timeToLive)
			{
				timedOut = true;
				DestroyBullet();
			}
			if (!moveInFixedUpdate)
			{
				MoveBullet();
			}
		}
	}

	public void FixedUpdate()
	{
		if (moveInFixedUpdate)
		{
			MoveBullet();
		}
	}

	private void MoveBullet()
	{
		if (!fired)
		{
			return;
		}
		base.transform.Translate(velocity * Time.deltaTime, Space.World);
		Ray ray = new Ray(base.transform.position, velocity.normalized);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray, out hitInfo, velocity.magnitude * 1.5f * Time.deltaTime);
		Debug.DrawRay(base.transform.position, velocity.normalized * velocity.magnitude * 1.5f * Time.deltaTime);
		if (flag)
		{
			GameObject other = hitInfo.transform.gameObject;
			if (!HitOwnShip(other))
			{
				DestroyBullet(hitInfo.point);
			}
		}
	}

	private bool HitOwnShip(GameObject other)
	{
		bool result = false;
		Ship component = other.GetComponent<Ship>();
		if (component != null && component == firedFrom)
		{
			result = true;
		}
		return result;
	}

	public void Fire(Vector3 initialVelocity, Ship firedFrom)
	{
		velocity = initialVelocity;
		fireTime = Time.time;
		fired = true;
	}

	public void DestroyBullet()
	{
		DestroyBullet(base.transform.position);
	}

	public void DestroyBullet(Vector3 destructionLocation)
	{
		if ((bool)hitFx && (!timedOut || ExplodeOnTimeout))
		{
			Object.Instantiate(hitFx, destructionLocation, base.transform.rotation);
		}
		Object.Destroy(base.gameObject);
	}
}
