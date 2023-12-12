using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScaledCamera : MonoBehaviour
{
	public Camera mainCamera;

	public Transform referencePoint;

	[Range(1f, 20000f)]
	public float scaleFactor = 1f;

	private Camera thisCam;

	private void Awake()
	{
		thisCam = GetComponent<Camera>();
	}

	private void OnEnable()
	{
		CameraManager.AddSceneryCamera(this);
	}

	private void Update()
	{
		if (mainCamera == null)
		{
			mainCamera = CameraManager.GetActiveCamera();
		}
		if (mainCamera != null)
		{
			if (referencePoint != null)
			{
				base.transform.position = referencePoint.transform.position + mainCamera.transform.position * (1f / scaleFactor);
			}
			else
			{
				base.transform.position = mainCamera.transform.position * (1f / scaleFactor);
			}
			thisCam.fieldOfView = mainCamera.fieldOfView;
			base.transform.rotation = mainCamera.transform.rotation;
		}
		else
		{
			Debug.LogError("Scaled camera has no reference camera.");
		}
	}
}
