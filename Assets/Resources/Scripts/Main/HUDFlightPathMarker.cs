using UnityEngine;
using UnityEngine.UI;

public class HUDFlightPathMarker : MonoBehaviour
{
	public float minSpeed = 1f;

	private Camera refCam;

	private new RectTransform transform;

	private Image fpmImage;

	private void Awake()
	{
		transform = GetComponent<RectTransform>();
		fpmImage = GetComponent<Image>();
	}

	public void SetCamera(Camera cam)
	{
		refCam = cam;
	}

	public void UpdateFPM(Vector3 position, Vector3 velocity)
	{ 
		Vector3 position2 = velocity + position;
		Vector3 position3 = refCam.WorldToScreenPoint(position2);
		fpmImage.enabled = position3.z >= 0f && velocity.sqrMagnitude > minSpeed * minSpeed;
		position3.z = 0f;
		transform.position = position3;
	}
}
