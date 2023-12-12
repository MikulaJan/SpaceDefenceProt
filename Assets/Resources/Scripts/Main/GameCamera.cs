using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public CameraCategory category;

	private Camera cam;

	private AudioListener listen;

	public Camera Camera
	{
		get
		{
			return cam;
		}
	}

	private void Awake()
	{ 
		cam = GetComponent<Camera>();
		listen = GetComponent<AudioListener>();
		if (CameraManager.activeCam == category)
		{
			EnableGameCam(true);
		}
		else
		{
			EnableGameCam(false);
		}
	}

	private void OnEnable()
	{
		CameraManager.AddCamera(this);
	}

	private void OnDisable()
	{
		CameraManager.RemoveCamera(this);
	}

	public void EnableGameCam(bool enable)
	{
		cam.enabled = enable;
		if (listen != null)
		{
			listen.enabled = enable;
		}
	}

	public void UseDepthOnly(bool depthOnly)
	{
		if (depthOnly)
		{
			cam.clearFlags = CameraClearFlags.Depth;
		}
		else
		{
			cam.clearFlags = CameraClearFlags.Skybox;
		}
	}
}
