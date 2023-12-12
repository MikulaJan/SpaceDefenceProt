using UnityEngine;
using Utilities;

[RequireComponent(typeof(WeaponGuidance))]
[RequireComponent(typeof(ThrusterPhysics))]
public class Munition : MonoBehaviour
{
	public ITargetable target;

	[Tooltip("Name of weapon as shown on the HUD.")]
	public string hudDisplayName = string.Empty;

	[Tooltip("Time the missile has after launch before it self-destructs.")]
	public float timeToLive = 30f;

	[Tooltip("Missile explodes when it self destructs.")]
	public bool explodeOnTimeOut = true;

	[Tooltip("How long the main motor will burn before shutting off. When set to 0, motor burns infinitely.")]
	public float motorBurnTime;

	[Tooltip("How long the maneuvering thrusters can burn before shutting off. When set to 0, motors burns infinitely.")]
	public float maneuveringBurnTime;

	[Tooltip("Seconds of maneuvering fuel. When nonzero, overrides maneuvering burn time.\n\nWhen maneuvering thrusters fire, they first drain motor fuel, and then drain their own. This means that a maneuvering missile will have a shorter main motor burn time than a non-maneuvering one.")]
	public float maneuveringFuel;

	[Tooltip("Delay between launch and firing of igition/guidance.")]
	public float fireDelay;

	[Tooltip("Boost to velocity that is added when the motor is fired.")]
	public float initialVelocity;

	[Tooltip("When true, guidance starts when the missile's motor fires.\n\nWhen false, guidance starts as soon as the missile is released.")]
	public bool guidanceStartOnFire = true;

	private float timeSinceLaunch;

	private float thrusterRamp;

	private bool isLaunched;

	private bool isFired;

	private Rigidbody rigidbody;

	private WeaponGuidance guidance;

	private ThrusterPhysics thrusters;

	private ThrusterBanks thrusterBanks;

	private Warhead warhead;

	private Vector3 appliedThrust;

	private ArrestingFCS flcs;

	private float _motorFuel;

	private bool hasStarted;

	public bool IsLaunched
	{
		get
		{
			return isLaunched;
		}
	}

	private void Awake()
	{
		guidance = GetComponent<WeaponGuidance>();
		thrusters = GetComponent<ThrusterPhysics>();
		thrusterBanks = GetComponent<ThrusterBanks>();
		warhead = GetComponent<Warhead>();
		flcs = new ArrestingFCS(new PidValues(0.5f, 0f, 0f), new PidValues(0.5f, 0f, 0f), true);
	}

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		_motorFuel = maneuveringFuel + motorBurnTime;
		hasStarted = true;
	}

	private void Update()
	{
		if (isLaunched)
		{
			timeSinceLaunch += Time.deltaTime;
		}
		if (timeSinceLaunch > fireDelay)
		{
			FireMissile();
		}
		if (timeSinceLaunch > timeToLive)
		{
			DestroyMissile(true);
			return;
		}
		guidance.Target = target;
		if (thrusterBanks != null)
		{
			thrusterBanks.SetTranslationRotation(thrusters.AppliedTranslation, thrusters.AppliedRotation);
		}
	}

	private void FixedUpdate()
	{
		if (isFired)
		{
			Vector3 vector = base.transform.InverseTransformDirection(rigidbody.velocity);
			Vector3 normalized = vector.normalized;
			thrusterRamp = Mathf.MoveTowards(thrusterRamp, 1f, 1f * Time.deltaTime);
			appliedThrust.x = RunArrestingHorizontalThrust(vector.x, normalized.z);
			appliedThrust.y = RunArrestingVerticalThrust(vector.y, normalized.z);
			appliedThrust.z = RunMotorThrust();
			thrusters.SetAppliedTranslation(appliedThrust);
		}
	}

	public void DestroyMissile(bool timedOut = false)
	{
		if (warhead != null && (explodeOnTimeOut || !timedOut))
		{
			warhead.Explode(base.transform.position);
		}
		if ((bool)thrusterBanks)
		{
			thrusterBanks.DetachAndStopAllParticleEffects();
		}
		Object.Destroy(base.gameObject);
	}

	private float RunArrestingHorizontalThrust(float currentHorizSpeed, float normalizedForwardSpeed)
	{
		float num = 0f;
		if (maneuveringFuel == 0f)
		{
			if (maneuveringBurnTime == 0f || timeSinceLaunch < maneuveringBurnTime)
			{
				num = flcs.FlyToTargetHorizontalSpeed(currentHorizSpeed * thrusterRamp, 0f, normalizedForwardSpeed);
			}
		}
		else
		{
			if (_motorFuel > 0f)
			{
				num = flcs.FlyToTargetHorizontalSpeed(currentHorizSpeed * thrusterRamp, 0f, normalizedForwardSpeed);
			}
			_motorFuel -= Mathf.Abs(num) * Time.deltaTime;
		}
		return num;
	}

	private float RunArrestingVerticalThrust(float currentVertSpeed, float normalizedForwardSpeed)
	{
		float num = 0f;
		if (maneuveringFuel == 0f)
		{
			if (maneuveringBurnTime == 0f || timeSinceLaunch < maneuveringBurnTime)
			{
				num = flcs.FlyToTargetHorizontalSpeed(currentVertSpeed * thrusterRamp, 0f, normalizedForwardSpeed);
			}
		}
		else
		{
			if (_motorFuel > 0f)
			{
				num = flcs.FlyToTargetHorizontalSpeed(currentVertSpeed * thrusterRamp, 0f, normalizedForwardSpeed);
			}
			_motorFuel -= Mathf.Abs(num) * Time.deltaTime;
		}
		return num;
	}

	private float RunMotorThrust()
	{
		float result = 0f;
		if (maneuveringFuel == 0f)
		{
			if (motorBurnTime == 0f)
			{
				result = 1f;
			}
			else if (timeSinceLaunch < motorBurnTime)
			{
				result = 1f;
			}
		}
		else
		{
			if (_motorFuel > motorBurnTime - maneuveringFuel)
			{
				result = 1f;
			}
			_motorFuel -= Time.deltaTime;
		}
		return result;
	}

	private void FireMissile()
	{
		if (!isFired)
		{
			isFired = true;
			if (guidance != null && guidanceStartOnFire)
			{
				guidance.StartGuidance();
			}
			rigidbody.velocity += base.transform.forward * initialVelocity;
			if (warhead != null)
			{
				warhead.Arm(true);
			}
		}
	}

	public void Launch()
	{
		Launch(Vector3.zero);
	}

	public void Launch(Vector3 inheritedVelocity)
	{
		if (hasStarted)
		{
			base.transform.parent = null;
			rigidbody.velocity += inheritedVelocity;
			rigidbody.isKinematic = false;
			if (guidance != null && !guidanceStartOnFire)
			{
				guidance.StartGuidance();
			}
			isLaunched = true;
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (isFired) 
		{
			DestroyMissile();
		}
	}
}
