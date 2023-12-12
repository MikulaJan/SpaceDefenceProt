using UnityEngine;

public class MouseLook : MonoBehaviour
{
	private float shift = 1f;

	private float lastX;

	private float lastY;

	private float x;

	private float y;

	public bool useFixedUpdate;

	public bool invert = true;

	public float sensitivity = 20f;
	 
	public float speed = 20f;

	public float modSpeed = 60f;

	public float zoomFov = 20f;

	public bool visible = true;

	private bool useModSpeed;

	private Camera cam;

	private float startingFov;

	private void Start()
	{
		cam = GetComponent<Camera>();
		startingFov = cam.fieldOfView;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			visible = !visible;
			if (visible)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			useModSpeed = !useModSpeed;
		}
		if (cam != null)
		{
			float b = startingFov;
			if (Input.GetMouseButton(2))
			{
				b = zoomFov;
			}
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, b, 10f * Time.deltaTime);
		}
		if (!useFixedUpdate)
		{
			MouseLookMovement();
		}
	}

	private void FixedUpdate()
	{
		if (useFixedUpdate)
		{
			MouseLookMovement();
		}
	}

	private void MouseLookMovement()
	{
		float num = ((!useModSpeed) ? speed : modSpeed);
		if (!visible)
		{
			x = Input.GetAxis("Mouse X") + lastX * 0.9f;
			if (invert)
			{
				y = 0f - Input.GetAxis("Mouse Y") + lastY * 0.9f;
			}
			else
			{
				y = Input.GetAxis("Mouse Y") + lastY * 0.9f;
			}
			float b = ((!Input.GetKey(KeyCode.LeftShift)) ? 1f : 2f);
			shift = Mathf.Lerp(shift, b, num * Time.deltaTime);
			lastX = x;
			lastY = y;
			base.transform.Rotate(sensitivity * y * Time.deltaTime, sensitivity * x * Time.deltaTime, 0f);
		}
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.z = 0f;
		base.transform.eulerAngles = eulerAngles;
		base.transform.Translate(num * shift * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, num * shift * Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
