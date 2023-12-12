using UnityEngine;

namespace Arena
{
	[RequireComponent(typeof(Camera))]
	public class FreeCamera : MonoBehaviour
	{
		[Header("Timing")]
		[Tooltip("Move and rotate the camera in the fixed update rather than regular update.")]
		public bool useFixedUpdate;

		public bool updateWhenPaused = true;

		[Header("Rotation")]
		[Tooltip("Inverts the pitch of the camera.")]
		public bool invert = true;

		[Tooltip("Camera is always rolled so that the horizon is level. Should not be changed after start.")]
		public bool alignToHorizon = true;

		[Tooltip("How quickly the mouse can rotate the camera.")]
		public float sensitivity = 2f;

		[Tooltip("Speed of rotation smoothing. High speeds make for a more responsive, but twitchier looking camera.")]
		public float mouseSmooth = 8f;

		[Header("Translation")]
		[Tooltip("Starting speed of the camera. Speed can be adjusted ingame using the mouse wheel.")]
		public float speed = 20f;

		[Tooltip("Speed of movement smoothing. High speeds make for a more responsive, but twitchier looking camera.")]
		public float moveSmooth = 8f;

		[Tooltip("Modifier for how quickly the mouse wheel changes speed.")]
		public float speedScrollModifier = 1f;

		[Header("Field of view")]
		[Tooltip("Zooming in slows down input to keep apparent input roughly the same.")]
		public bool zoomSlowsRotation;

		[Tooltip("Speed of field of view smoothing. High speeds make for a more responsive, but twitchier looking camera.")]
		public float zoomSmooth = 8f;

		[Tooltip("Field of view when zoomed in.")]
		[Range(1f, 179f)]
		public float zoomFov = 20f;

		[SerializeField]
		private bool mouseVisible = true;

		private Camera cam;

		private float shift = 1f;

		private float startingFov;

		private float mouseX;

		private float mouseY;

		private float mouseZ;

		private Vector3 oldVelocity = Vector3.zero;

		private float oldHorizontal;

		private float oldVertical;

		private int frame;

		private int fixedframe;

		private void Start()
		{
			cam = GetComponent<Camera>();
			startingFov = cam.fieldOfView;
		}

		private void Update()
		{
			frame++;
			mouseX = Input.GetAxis("Horizontal");
			mouseY = Input.GetAxis("Vertical");
			mouseZ = Input.mouseScrollDelta.y;
		}

		private void FixedUpdate()
		{
			fixedframe++;
			if (useFixedUpdate)
			{
				RunMouseLook();
			}
		}

		private void LateUpdate()
		{
			if (!useFixedUpdate || (useFixedUpdate && Time.timeScale == 0f))
			{
				RunMouseLook();
			}
		}

		private void RunMouseLook()
		{
			float deltaTime = (updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime);
			UpdateFieldOfView(deltaTime);
			RotateCamera(deltaTime, 1f);
			TranslateCamera(deltaTime);
		}

		private void UpdateFieldOfView(float deltaTime)
		{
			float b = startingFov;
			if (Input.GetMouseButton(2))
			{
				b = zoomFov;
			}
			cam.fieldOfView = SmoothDamp(cam.fieldOfView, b, zoomSmooth, deltaTime);
		}

		private void RotateCamera(float deltaTime, float speedModifier)
		{
			if (!mouseVisible)
			{
				Quaternion rotation = base.transform.rotation;
				float angle = CalculateYaw(deltaTime, speedModifier);
				float angle2 = CalculatePitch(deltaTime, speedModifier);
				Space relativeTo = ((!alignToHorizon) ? Space.Self : Space.World);
				base.transform.Rotate(Vector3.up, angle, relativeTo);
				base.transform.Rotate(Vector3.right, angle2, Space.Self);
			}
			else
			{
				oldVertical = 0f;
				oldHorizontal = 0f;
			}
		}

		private float CalculatePitch(float deltaTime, float speedModifier)
		{
			float num = (invert ? (-1f) : 1f);
			float num2 = mouseY;
			num2 *= sensitivity * num * speedModifier;
			return oldVertical = SmoothDamp(oldVertical, num2, mouseSmooth, deltaTime);
		}

		private float CalculateYaw(float deltaTime, float speedModifier)
		{
			float num = ((alignToHorizon && base.transform.up.y < 0f) ? (-1f) : 1f);
			float num2 = mouseX;
			num2 *= sensitivity * num * speedModifier;
			return oldHorizontal = SmoothDamp(oldHorizontal, num2, mouseSmooth, deltaTime);
		}

		private float GetZoomModifier()
		{
			return cam.fieldOfView / startingFov;
		}

		private void TranslateCamera(float deltaTime)
		{
			float f = mouseZ;
			float num = Mathf.Sign(f);
			if (Mathf.Abs(f) > 0.1f)
			{
				if (speedScrollModifier == 0f)
				{
					speedScrollModifier = 1f;
				}
				float num2 = 1.2f * speedScrollModifier;
				float num3 = 0.8f / speedScrollModifier;
				float num4 = ((num > 0f) ? num2 : num3);
				num4 *= Mathf.Abs(f);
				speed *= num4;
			}
			float num5 = (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);
			shift = num5;
			float num6 = GetKeyInput(KeyCode.W, KeyCode.S) * speed * shift;
			float num7 = GetKeyInput(KeyCode.D, KeyCode.A) * speed * shift;
			float num8 = GetKeyInput(KeyCode.E, KeyCode.Q) * speed * shift;
			Vector3 b = base.transform.forward * num6;
			b += base.transform.right * num7;
			b += Vector3.up * num8;
			Vector3 vector = (oldVelocity = SmoothDamp(oldVelocity, b, moveSmooth, deltaTime));
			base.transform.Translate(vector * deltaTime, Space.World);
		}

		private float GetKeyInput(KeyCode positive, KeyCode negative)
		{
			float num = 0f;
			if (Input.GetKey(positive))
			{
				num += 1f;
			}
			if (Input.GetKey(negative))
			{
				num -= 1f;
			}
			return num;
		}

		private float SmoothDamp(float a, float b, float lambda, float dt)
		{
			return Mathf.Lerp(a, b, 1f - Mathf.Exp((0f - lambda) * dt));
		}

		private Vector3 SmoothDamp(Vector3 a, Vector3 b, float lambda, float dt)
		{
			return Vector3.Lerp(a, b, 1f - Mathf.Exp((0f - lambda) * dt));
		}
	}
}
