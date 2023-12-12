using UnityEngine;

public class ThrusterPhysics : MonoBehaviour
{
	[Tooltip("X: Left/Right\nY: Up/Down\nZ: Forward/Reverse")]
	public Vector3 thrust;

	public float reverseFraction = 0.5f;

	[Tooltip("X: Pitch\nY: Yaw\nZ: Roll")]
	public Vector3 rotationForce;

	[Tooltip("How long the delay is between input and forward/reverse thrust ramping up.")]
	public float thrustDelay;

	private Vector3 translation;

	private Vector3 rotation;

	private float longitudeThrust;

	private float thrustSpeed;

	private Rigidbody rigidbody;

	private const float FORCE_MULT = 100f;

	public Vector3 AppliedTranslation
	{
		get
		{
			return translation;
		}
	}

	public Vector3 AppliedRotation 
	{
		get
		{
			return rotation;
		}
	}

	public void Awake()
	{
	}

	public void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
		if ((bool)rigidbody)
		{
			float num = ((!(translation.z > 0f)) ? (thrust.z * reverseFraction) : thrust.z);
			longitudeThrust = Mathf.SmoothDamp(longitudeThrust, num * translation.z, ref thrustSpeed, thrustDelay);
			rigidbody.AddRelativeForce(translation.x * thrust.x * 100f, translation.y * thrust.y * 100f, longitudeThrust * 100f);
			rigidbody.AddRelativeTorque(rotation.x * rotationForce.x * 100f, rotation.y * rotationForce.y * 100f, rotation.z * rotationForce.z * 100f);
			DrawDebugRays();
		}
	}

	public void SetAppliedTranslation(Vector3 i_Translation)
	{
		translation = i_Translation;
	}

	public void SetAppliedRotation(Vector3 i_Rotation)
	{
		rotation = i_Rotation;
	}

	public void SetAppliedThrust(Vector3 i_Translation, Vector3 i_Rotation)
	{
		translation = i_Translation;
		rotation = i_Rotation;
	}

	public void GetAppliedThrust(out Vector3 o_translation, out Vector3 o_rotation)
	{
		o_translation = translation;
		o_rotation = rotation;
	}

	private void DrawDebugRays()
	{
		Debug.DrawRay(base.transform.position, base.transform.up * translation.y * 10f, Color.cyan);
		Debug.DrawRay(base.transform.position, base.transform.right * translation.x * 10f, Color.cyan);
		Debug.DrawRay(base.transform.position, base.transform.forward * translation.z * 10f, Color.cyan);
	}
}
