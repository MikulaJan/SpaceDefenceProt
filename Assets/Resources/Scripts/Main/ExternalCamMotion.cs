using UnityEngine;

public class ExternalCamMotion : MonoBehaviour
{
	private new Transform transform;

	public Transform followTarget;

	public bool useInterpolation;

	private Vector3 lastFramePos;

	private Vector3 lastFrameVel;

	private Vector3 lastFrameAccel;

	private Vector3 targetRot;

	private Vector3 currentRot;
	 
	private Camera extCamera;

	[Range(0f, 10f)]
	public float jitterMagnitude = 2f;

	private void Awake()
	{
		transform = GetComponent<Transform>();
		extCamera = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		SetNewFollowTarget(followTarget);
		transform.parent = null;
		targetRot = transform.eulerAngles;
		currentRot = targetRot;
		lastFramePos = followTarget.position;
	}

	public void SetNewFollowTarget(Transform newTarget)
	{
		followTarget = newTarget;
		Rigidbody componentInParent = followTarget.GetComponentInParent<Rigidbody>();
		if (componentInParent != null && componentInParent.interpolation == RigidbodyInterpolation.Interpolate)
		{
			useInterpolation = true;
		}
	}

	private void Update()
	{
		if (extCamera.enabled)
		{
			float axis = Input.GetAxis("POVHorizontal");
			float axis2 = Input.GetAxis("POVVertical");
			targetRot.x += axis2;
			targetRot.y += axis;
			currentRot = Vector3.Lerp(currentRot, targetRot, 20f * Time.fixedDeltaTime);
			transform.eulerAngles = currentRot;
			extCamera.transform.rotation = Quaternion.Lerp(extCamera.transform.rotation, Quaternion.LookRotation(followTarget.position - extCamera.transform.position, Vector3.up), 5f * Time.deltaTime);
		}
		if (useInterpolation)
		{
			UpdateCameraPosition();
		}
	}

	private void FixedUpdate()
	{
		if (!useInterpolation)
		{
			UpdateCameraPosition();
		}
	}

	private void UpdateCameraPosition()
	{
		float x = jitterMagnitude * Mathf.Sin(Time.time) / 1.3f;
		float y = jitterMagnitude * Mathf.Sin(Time.time) / 1f;
		float z = jitterMagnitude * Mathf.Sin(Time.time) / 1.6f;
		Vector3 vector = new Vector3(x, y, z);
		Vector3 vector2 = (followTarget.position - lastFramePos) / Time.deltaTime;
		lastFramePos = followTarget.position;
		Vector3 b = (vector2 - lastFrameVel) / Time.deltaTime;
		lastFrameVel = vector2;
		b = (lastFrameAccel = Vector3.Lerp(lastFrameAccel, b, 0.3f * Time.deltaTime) / 1f);
		transform.position = followTarget.position + vector - b;
	}
}
