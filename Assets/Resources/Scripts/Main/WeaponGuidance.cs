using System;
using UnityEngine;

public class WeaponGuidance : MonoBehaviour
{
	[Tooltip("Pure pursuit flies directly to the target. This is more wasteful of the motor and less efficient.\n\nLead targeting flies to where the target will be is much more accurate and efficient.")]
	public GuidanceMode guidanceMode;

	[Tooltip("How far off boresight the missile can see.")]
	public float seekerLimit = 30f;

	[Tooltip("Rate at which the missile turns when using game physics.")]
	public float maxTurnRate = 30f;

	[Tooltip("Delay between launch and guidance.")]
	public float guidanceDelay;

	private float timeSinceGuidanceStarted;

	private bool guidanceStarted;

	private ITargetable target;

	private Rigidbody rigidbody; 

	private bool targetTracking = true;

	private const float MINIMUM_GUIDE_SPEED = 10f;

	public ITargetable Target
	{
		set
		{
			target = value;
		}
	}

	public void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		if (guidanceStarted)
		{
			timeSinceGuidanceStarted += Time.deltaTime;
		}
	}

	public void FixedUpdate()
	{
		if (!guidanceStarted || !(timeSinceGuidanceStarted > guidanceDelay))
		{
			return;
		}
		if (target != null)
		{
			if (Vector3.Angle(base.transform.forward, target.Position - base.transform.position) > seekerLimit)
			{
				targetTracking = false;
			}
			if (targetTracking)
			{
				RunMissileTracking();
			}
		}
		else
		{
			CancelRotations();
		}
	}

	public void StartGuidance()
	{
		guidanceStarted = true;
	}

	public void StartGuidance(float guidanceDelay)
	{
		guidanceStarted = true;
		this.guidanceDelay = guidanceDelay;
	}

	private void CancelRotations()
	{
		base.transform.rotation = Quaternion.LookRotation(base.transform.forward);
	}

	private void RunMissileTracking()
	{
		Vector3 vector = target.Position - base.transform.position;
		Vector3 vector2 = vector;
		if (guidanceMode == GuidanceMode.LeadTargeting)
		{
			Vector3 vector3 = CalculateLeadOfTarget(target.Position);
			vector2 = vector3 - base.transform.position;
		}
		Vector3 vector4 = Vector3.RotateTowards(vector, vector2, seekerLimit * ((float)Math.PI / 180f) * 0.9f, 1f);
		Vector3 forward = Vector3.RotateTowards(base.transform.forward, vector4, maxTurnRate * ((float)Math.PI / 180f) * Time.deltaTime, 1f);
		base.transform.rotation = Quaternion.LookRotation(forward);
		rigidbody.maxAngularVelocity = 7f;
	}

	private Vector3 CalculateLeadOfTarget(Vector3 targetPos)
	{
		Vector3 vector = targetPos - base.transform.position;
		float magnitude = rigidbody.velocity.magnitude;
		float magnitude2 = vector.magnitude;
		float num = magnitude2 / Mathf.Max(magnitude, 10f);
		return target.Position + target.Velocity * num;
	}
}
