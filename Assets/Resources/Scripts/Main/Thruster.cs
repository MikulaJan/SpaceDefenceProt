using UnityEngine;
//

public class Thruster : MonoBehaviour
{
	public ThrusterVFX effectPrefab;

	private ThrusterVFX effect;

	[Tooltip("Used by the thruster bank to automatically assign thrusters.")]
	public ThrusterTranslationDirection translateDir;

	[Tooltip("Used by the thruster bank to automatically assign thrusters.")]
	public ThrusterRotationalDirection rotationDir;

	private float power1;

	private float power2;

	private const float DEBUG_RAY_MULT = 5f;

	private void Start()
	{
		if (effectPrefab != null)
		{
			effect = Object.Instantiate(effectPrefab);
			effect.transform.SetParent(base.transform);
			effect.transform.localPosition = Vector3.zero;
			effect.transform.localEulerAngles = Vector3.zero;
		}
		else
		{
			Debug.Log(base.name + ": Missing a ThrusterVFX prefab");
		}
	}

	private void Update()
	{
		Debug.DrawRay(base.transform.position, base.transform.forward * (power1 + power2) * 5f, Color.white);
	}

	public void SetPower1(float power)
	{
		if (effect != null)
		{
			effect.SetPower1(power);
		}
	}

	public void SetPower2(float power)
	{
		if (effect != null)
		{
			effect.SetPower2(power);
		}
	}

	public void DetachAndStopThrusterVFX()
	{
		if (effect != null)
		{
			effect.transform.SetParent(null);
			effect.StopAllParticleEffects();
		}
	}

	private Vector3 CalculateLocalForward()
	{
		Vector3 forward = Vector3.forward;
		Transform transform = GetComponentInParent<Ship>().transform;
		forward = ((!transform) ? base.transform.parent.InverseTransformDirection(base.transform.forward) : transform.InverseTransformDirection(base.transform.forward));
		forward.Normalize();
		return forward;
	}

	private Vector3 CalculateLocalPosition()
	{
		Vector3 zero = Vector3.zero;
		Transform transform = GetComponentInParent<Ship>().transform;
		if ((bool)transform)
		{
			return transform.InverseTransformPoint(base.transform.position);
		}
		return base.transform.parent.InverseTransformPoint(base.transform.position);
	}

	private bool VerticalIsDominantDirection(Vector3 direction)
	{
		return Mathf.Abs(direction.y) > Mathf.Abs(direction.z) && Mathf.Abs(direction.y) > Mathf.Abs(direction.x);
	}

	private bool HorizontalIsDominantDirection(Vector3 direction)
	{
		return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Abs(direction.z);
	}

	private bool LongitudinalIsDominantDirection(Vector3 direction)
	{
		return Mathf.Abs(direction.z) > Mathf.Abs(direction.x) && Mathf.Abs(direction.z) > Mathf.Abs(direction.y);
	}

	[ContextMenu("Auto assign translational thrust direction")]
	public void AutoAssignTranslation()
	{
		Vector3 direction = CalculateLocalForward();
		if (LongitudinalIsDominantDirection(direction))
		{
			translateDir = ((!(direction.z >= 0f)) ? ThrusterTranslationDirection.Forward : ThrusterTranslationDirection.Reverse);
		}
		else if (VerticalIsDominantDirection(direction))
		{
			translateDir = ((!(direction.y >= 0f)) ? ThrusterTranslationDirection.Up : ThrusterTranslationDirection.Down);
		}
		else if (HorizontalIsDominantDirection(direction))
		{
			translateDir = ((!(direction.x >= 0f)) ? ThrusterTranslationDirection.Right : ThrusterTranslationDirection.Left);
		}
		Debug.Log(string.Concat("Assigned translation direction ", translateDir, " to ", base.name));
	}

	[ContextMenu("Auto assign pitch and yaw thrust directions")]
	public void AutoAssignPitchYaw()
	{
		Vector3 direction = CalculateLocalForward();
		Vector3 vector = CalculateLocalPosition();
		if (VerticalIsDominantDirection(direction))
		{
			if (direction.y > 0f)
			{
				if (vector.z > 0f)
				{
					rotationDir = ThrusterRotationalDirection.PitchDown;
				}
				else if (vector.z < 0f)
				{
					rotationDir = ThrusterRotationalDirection.PitchUp;
				}
			}
			else if (direction.y < 0f)
			{
				if (vector.z > 0f)
				{
					rotationDir = ThrusterRotationalDirection.PitchUp;
				}
				else if (vector.z < 0f)
				{
					rotationDir = ThrusterRotationalDirection.PitchDown;
				}
			}
		}
		else
		{
			if (!HorizontalIsDominantDirection(direction))
			{
				return;
			}
			if (direction.x > 0f)
			{
				if (vector.z > 0f)
				{
					rotationDir = ThrusterRotationalDirection.YawLeft;
				}
				else if (vector.z < 0f)
				{
					rotationDir = ThrusterRotationalDirection.YawRight;
				}
			}
			if (direction.x < 0f)
			{
				if (vector.z > 0f)
				{
					rotationDir = ThrusterRotationalDirection.YawRight;
				}
				else if (vector.z < 0f)
				{
					rotationDir = ThrusterRotationalDirection.YawLeft;
				}
			}
		}
	}

	public void AutoAssignRoll()
	{
		Vector3 vector = CalculateLocalForward();
		Vector3 vector2 = CalculateLocalPosition();
		if (vector.y > 0f)
		{
			if (vector2.x > 0f)
			{
				rotationDir = ThrusterRotationalDirection.RollRight;
			}
			else if (vector2.x < 0f)
			{
				rotationDir = ThrusterRotationalDirection.RollLeft;
			}
		}
		else if (vector.y < 0f)
		{
			if (vector2.x > 0f)
			{
				rotationDir = ThrusterRotationalDirection.RollLeft;
			}
			else if (vector2.x < 0f)
			{
				rotationDir = ThrusterRotationalDirection.RollRight;
			}
		}
	}
}
